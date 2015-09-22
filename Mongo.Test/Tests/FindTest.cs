using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mongo.Test.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Mongo.Test.Tests
{
    public class FindTest : BaseTest
    {
        #region Find (BsonDocument)

        [Fact]
        public async Task FindBsonDocumentWithCursor()
        {
            List<BsonDocument> bsList = new List<BsonDocument>();
            BsonDocument filter = new BsonDocument(); // All People
            using (var cursor = await TestContext.PeopleAsBson.Find(filter).ToCursorAsync())
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var doc in cursor.Current)
                    {
                        bsList.Add(doc);
                    }
                }
            }

            bsList.Should().HaveCount(7);
        }

        [Fact]
        public async Task FindBsonDocumentWithForEach()
        {
            List<BsonDocument> bsList = new List<BsonDocument>();

            BsonDocument filter = new BsonDocument();
            await TestContext.PeopleAsBson.Find(filter).ForEachAsync(x => bsList.Add(x));

            bsList.Should().HaveCount(7);
        }

        [Fact]
        public async Task FindFilteredWithBsonDocument()
        {
            var filter = new BsonDocument("name", "Mehmet");
            var list = await TestContext.PeopleAsBson.Find(filter).ToListAsync();
            list.Should().HaveCount(1);
        }

        [Fact]
        public async Task FindFiltered2WithBsonDocument()
        {
            var filter = new BsonDocument("$and", new BsonArray()
            {
                new BsonDocument("profession","Developer"), new BsonDocument("age", new BsonDocument("$lte",24))
            });
            var list = await TestContext.PeopleAsBson.Find(filter).ToListAsync();
            list.Should().HaveCount(1);
        }


        [Fact]
        public async Task FindFilteredWithBuilder()
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Lt("age", 30) & builder.Eq("name", "Mehmet");
            var list = await TestContext.PeopleAsBson.Find(filter).ToListAsync();
            list.Should().HaveCount(1);
        }

        [Fact]
        public async Task FindFilteredArrayInWithBuilder()
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.In("name", new string[] { "Mehmet", "İdris" });
            List<BsonDocument> list = await TestContext.PeopleAsBson.Find(filter).ToListAsync();

            list.Should().HaveCount(2);
           
        }

        #endregion

        #region Find (Model)

        [Fact]
        public async Task FindModelWithToList()
        {
            var filter = new BsonDocument();
            List<Person> person = await TestContext.People.Find(filter).ToListAsync();
            person.Should().HaveCount(7);
        }

        [Fact]
        public async Task FindFilteredWithExpressionTree()
        {
            List<Person> personList = await TestContext.People.Find(x => x.Age < 30).ToListAsync();
            personList.Should().HaveCount(2);
        }

        [Fact]
        public async Task FindFilteredArrayAllWithBuilderOfModel()
        {
            FilterDefinition<Person> filter = Builders<Person>.Filter.All(x => x.Favorites, new string[] { "beard" });
            List<Person> bsonDocumentList = await TestContext.People.Find(filter).ToListAsync();

            bsonDocumentList.Should().HaveCount(2);
          
        }

        [Fact]
        public async Task FindProjectionWithEntity()
        {
            BsonDocument filter = new BsonDocument();
            var pList = await TestContext.People.Find(filter).Project(new BsonDocument("name",true).Add("_id",false)).ToListAsync();
            pList.Should().HaveCount(7);
            pList.Any(x => x.Contains("_id")).Should().BeFalse();
        }

        #endregion

    }
}