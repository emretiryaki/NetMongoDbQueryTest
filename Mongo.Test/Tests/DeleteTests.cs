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
    public class DeleteTests : BaseTest
    {
        [Fact]
        public async Task DeleteOneEtity()
        {
            DeleteResult result = await TestContext.Widgets.DeleteOneAsync(x => x.X > 6);

            result.IsAcknowledged.Should().BeTrue();
            result.DeletedCount.Should().Be(1);
        }

        [Fact]
        public async Task DeleteMultiEtity()
        {
            DeleteResult result = await TestContext.Widgets.DeleteManyAsync(x => x.X > 6);
            result.IsAcknowledged.Should().BeTrue();
            result.DeletedCount.Should().BeGreaterThan(1);
        }
        [Fact]
        public async Task DeleteOneBsonObject()
        {
            var bsonDocument = new BsonDocument
            {
                {"x",1}
            };

            var result = await TestContext.WidgetsAsBson.DeleteOneAsync(bsonDocument);
            result.IsAcknowledged.Should().BeTrue();
            result.DeletedCount.Should().Be(1);
        }

        [Fact]
        public async Task DeleteManyBsonObject()
        {
            var document = Builders<BsonDocument>.Filter.Eq("x", 2);
            var result = await TestContext.WidgetsAsBson.DeleteOneAsync(document);
            result.IsAcknowledged.Should().BeTrue();
            result.DeletedCount.Should().Be(1);
        }
    }
}
