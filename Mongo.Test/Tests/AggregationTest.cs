using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Mongo.Test.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Mongo.Test.Tests
{
    public class AggregationTests : BaseTest
    {
        #region Group - Count
        [Fact]
        public async Task GroupCountWithBsonDocument()
        {
            BsonDocument groupProjection = new BsonDocument
            {
              { "_id","null"},
              {"count",new BsonDocument { { "$sum", 1} }}

            };

            List<BsonDocument> list = await TestContext.OrderAsBson.Aggregate()
               .Group(groupProjection)
               .ToListAsync();

            list.Should().Contain(x => x["count"] == 7);

        }

        [Fact]
        public async Task GroupCount()
        {

            var list = await TestContext.Order.Aggregate()
                .Group(x => x.Status, g => new { Id = g.Key, NumberOfStatus = g.Sum(i => 1) }).ToListAsync();

            list.Should().HaveCount(3);
            list.Should().Contain(x => x.Id == "A" && x.NumberOfStatus == 4);
            list.Should().Contain(x => x.Id == "R" && x.NumberOfStatus == 2);
            list.Should().Contain(x => x.Id == "S" && x.NumberOfStatus == 1);

        }
        #endregion

        #region Group - Average

        [Fact]
        public async Task GroupAverageWithBsonDocument()
        {
            BsonDocument groupProjection = new BsonDocument
           {
                 { "_id","$status"},
                 {"avg_age", new BsonDocument {{ "$avg","$totalprice"}}}
           };
            List<BsonDocument> list = await TestContext.OrderAsBson.Aggregate()
                .Group(groupProjection)
                .ToListAsync();

            list.Should().HaveCount(3);
        }

        [Fact]
        public async Task GroupAverage()
        {
            var list =
                await
                    TestContext.Order.Aggregate()
                        .Group(x => x.Status, g => new { Id = g.Key, Avg_Price = g.Average(i => i.Totalprice) })
                        .ToListAsync();
            list.Should().HaveCount(3);

            list.Should().Contain(x => x.Id == "S" && x.Avg_Price == 100);
            ;
        }

        #endregion ENDOF: Group - Average

        #region Group - Max
        [Fact]
        public async Task GroupMaxWithBsonDocument()
        {
            BsonDocument groupProjection = new BsonDocument
            {
                { "_id", "$status" },
                { "max_price", new BsonDocument { { "$max", "$totalprice" } } }
            };

            List<BsonDocument> list = await TestContext.OrderAsBson.Aggregate()
               .Group(groupProjection)
               .ToListAsync();

            list.Should().HaveCount(3);
            list.Should().Contain(x => x["max_price"] == 100);

        }

        [Fact]
        public async Task GroupMax()
        {
            var list = await TestContext.Order.Aggregate()
                .Group(x => x.Status, g => new { Id = g.Key, MaxPrice = g.Max(i => i.Totalprice) })
                .ToListAsync();

            list.Should().HaveCount(3);
            list.Should().Contain(x => x.MaxPrice == 100);
        }

        #endregion ENDOF: Group - Max

        #region Group - Min
        [Fact]
        public async Task GroupMinWithBsonDocument()
        {
            BsonDocument groupProjection = new BsonDocument
            {
                { "_id", "$status" },
                { "min_price", new BsonDocument { { "$min", "$totalprice" } } }
            };

            List<BsonDocument> list = await TestContext.Order.Aggregate()
               .Group(groupProjection)
               .ToListAsync();

            list.Should().HaveCount(3);
            list.Should().Contain(x => x["min_price"] == 10);

        }

        [Fact]
        public async Task GroupMin()
        {
            var list = await TestContext.Order.Aggregate()
                .Group(x => x.Status, g => new { Id = g.Key, MinPrice = g.Min(i => i.Totalprice) })
                .ToListAsync();

            list.Should().HaveCount(3);
            list.Should().Contain(x => x.MinPrice == 10);
        }

        #endregion ENDOF: Group - Max

        #region Group - Push

        [Fact]
        public async Task GroupPushWithBsonDocument()
        {
            BsonDocument groupProjection = new BsonDocument
            {
                { "_id", "$status" },
                { "items", new BsonDocument { { "$push", "$items" } } }
            };

            var list = await TestContext.Order.Aggregate().Group(groupProjection).ToListAsync();
            list.Should().HaveCount(3);
        }

        #endregion

        #region Project

        [Fact]
        public async Task Projec()
        {
            var products = await TestContext.Order.Aggregate().Project
                (
                    p => new
                    {
                        Name = p.Name,
                        Status = p.Status,
                        Detail = new { Price = p.Totalprice * 10 },

                    }
                ).ToListAsync();

            products.Should().HaveCount(7);
        }

        #endregion

        #region Match

        [Fact]
        public async Task MatchWithBsonDocument()
        {
            BsonDocument filter = new BsonDocument { { "name", "SaleOrder1" } };

            var list = await TestContext.Order.Aggregate()
                .Match(filter)
                .ToListAsync();
            list.Should().HaveCount(2);
        }

        [Fact]
        public async Task MatchWithExpressionTree()
        {
            List<Order> productsOfApple = await TestContext.Order.Aggregate()
                .Match(x => x.Status == "A")
                .ToListAsync();
            productsOfApple.Should().HaveCount(4);
        }

        #endregion ENDOF: Match

        #region Sort

        [Fact]
        public async Task SortWithBsonDocument()
        {
            var sortDefinition = new BsonDocument { { "totalprice",-1}};
            var list = await TestContext.OrderAsBson.Aggregate().Sort(sortDefinition).ToListAsync();
            list.Should().HaveCount(7);
            list.First()["totalprice"].Should().Be(100);
           
        }

        [Fact]
        public async Task Sort()
        {
            List<Order> list = await TestContext.Order.Aggregate()
                .SortByDescending(x => x.Totalprice)
                .ToListAsync();

            list.Should().HaveCount(7);
            list.First().Totalprice.Should().Be(100);
            list.Last().Totalprice.Should().Be(10);
        }

        #endregion ENDOF

        #region Skip

        [Fact]
        public async Task SkipAndLimitWithBsonDocument()
        {
            BsonDocument sortDefinition = new BsonDocument { { "totalprice", -1 } };

            List<BsonDocument> products = await TestContext.OrderAsBson.Aggregate()
               .Sort(sortDefinition)
               .Skip(1)
               .Limit(1)
               .ToListAsync();

            products.Should().HaveCount(1);
            products.First()["totalprice"].Should().Be(100);
        }

        [Fact]
        public async Task SkipAndLimit()
        {
            var list =
                await TestContext.Order.Aggregate().SortByDescending(x => x.Totalprice).Skip(1).Limit(1).ToListAsync();

            list.Should().HaveCount(1);
            list.First().Totalprice.Should().Be(100);
        }

        #endregion

        #region First and Last

        [Fact]
        public async Task LastWithExpressionTree()
        {
            var list =
                await
                    TestContext.Order.Aggregate()
                        .SortBy(x => x.Totalprice)
                        .ThenByDescending(x => x.Status)
                        .Group(x => x.Status, x => new {Id = x.Key, x.Last().Totalprice})
                        .ToListAsync();
            list.Should().HaveCount(3);
            list.First().Id.Should().BeOneOf("A","S");
            list.First().Totalprice.Should().Be(100);
            list.Last().Id.Should().Be("R");
            list.Last().Totalprice.Should().Be(20);
        }

        #endregion ENDOF: First and Last
    }
}
