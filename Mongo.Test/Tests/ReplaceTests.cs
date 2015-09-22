using System;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Mongo.Test.Tests
{
    public class ReplaceTests : BaseTest
    {

        [Fact]
        public async Task ReplaceOneBsonDocument()
        {
            var bsonDocument = new BsonDocument()
            {
                {"Name","Mehmet Tiryaki"},
                {"Email","mtirytaki@efer.com"}
            };
            bsonDocument.Contains("_id").Should().BeFalse();
            await BlogContext.UserAsBson.InsertOneAsync(bsonDocument);
            var objectID = bsonDocument["_id"];

            var filter = new BsonDocument("_id", objectID);
            var replacement = new BsonDocument{
                { "_id", objectID },
                { "Email", "mtiryaki@hotmail.com" },
                 {"Name","Mehmet Tiryaki"},
            };

            var result = await BlogContext.UserAsBson.ReplaceOneAsync(filter, replacement);

            result.IsAcknowledged.Should().BeTrue();
            result.IsModifiedCountAvailable.Should().BeTrue();
            result.MatchedCount.Should().Be(1);
            result.ModifiedCount.Should().Be(1);
        }

        [Fact]
        public async Task ErrorWithReplaceOneBsonDocumentWhenObjectIdChange()
        {
            var bsonDocument = new BsonDocument()
            {
                {"Name","Mehmet Tiryaki"},
                {"Email","mtirytaki@efer.com"}
            };
            bsonDocument.Contains("_id").Should().BeFalse();
            await BlogContext.UserAsBson.InsertOneAsync(bsonDocument);
            var objectID = bsonDocument["_id"];

            var filter = new BsonDocument("_id", objectID);
            var replacement = new BsonDocument{
                { "_id", 5 },
                { "Email", "mtiryaki@hotmail.com" },
                 {"Name","Mehmet Tiryaki"},
            };

            Func<Task> updaFunc = () => BlogContext.UserAsBson.ReplaceOneAsync(filter, replacement);

            updaFunc.ShouldThrow<MongoWriteException>("Id can not be changed!");
           
        }

    }
}