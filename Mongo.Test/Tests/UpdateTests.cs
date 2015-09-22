using System.Threading.Tasks;
using FluentAssertions;
using Mongo.Test.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Mongo.Test.Tests
{
    public class UpdateTests : BaseTest
    {
        #region Update BsonDocument

        [Fact]
        public async Task UpdateOneBsonDocument()
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("x", 5);
            BsonDocument replacement = new BsonDocument("$inc", new BsonDocument("x", 10));

            var result = await TestContext.WidgetsAsBson.UpdateOneAsync(filter, replacement);

            result.MatchedCount.Should().Be(1);
            result.ModifiedCount.Should().Be(1);
        }

        [Fact]
        public async Task UpdateOneBsonDocumentWithBuilders()
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("x", 4);
            UpdateDefinition<BsonDocument> replacement = Builders<BsonDocument>.Update.Inc("x", 1);

            var result = await TestContext.WidgetsAsBson.UpdateOneAsync(filter, replacement);

            result.MatchedCount.Should().Be(1);
            result.ModifiedCount.Should().Be(1);

        }

        [Fact]
        public async Task UpdateManyBsonDocumentWithBuilders()
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Lte("x", 9);
            UpdateDefinition<BsonDocument> replacement = Builders<BsonDocument>.Update.Inc("x", 3);

            var result = await TestContext.WidgetsAsBson.UpdateManyAsync(filter, replacement);

            result.MatchedCount.Should().BeGreaterThan(1);
            result.ModifiedCount.Should().BeGreaterThan(1);

        }
        #endregion

        #region Update Model

        [Fact]
        public async Task UpdateEntityDocument()
        {

            UpdateDefinition<Widget> replacement = Builders<Widget>.Update.Inc(x => x.X, 11);
            UpdateResult result = await TestContext.Widgets.UpdateOneAsync(x => x.X > 5, replacement);
            result.MatchedCount.Should().Be(1);
            result.ModifiedCount.Should().Be(1);


        }
        [Fact]
        public async Task UpdateMultiEntityDocument()
        {

            UpdateDefinition<Widget> replacement = Builders<Widget>.Update.Inc(x => x.X, 11);
            UpdateResult result = await TestContext.Widgets.UpdateManyAsync(x => x.X > 5, replacement);
            result.MatchedCount.Should().BeGreaterThan(1);
            result.ModifiedCount.Should().BeGreaterThan(1);


        }

        #endregion
    }
}