using System.Threading.Tasks;
using FluentAssertions;
using Mongo.Test.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Mongo.Test.Tests
{
    public class CountTests : BaseTest
    {
        [Fact]
        public async Task CountAll()
        {
            long count = await SchoolContext.StudentsAsBson.CountAsync(new BsonDocument());
            count.Should().BeGreaterOrEqualTo(1);
        }
        [Fact]
        public async Task CountFilteredWithBsonDocument()
        {
            BsonDocument filter = new BsonDocument("_id", "050305007");
            long count = await SchoolContext.StudentsAsBson.CountAsync(filter);

            count.Should().Be(1);
        }
        [Fact]
        public async Task CountFilteredWithBuilderOfModel()
        {
            FilterDefinitionBuilder<Student> builder = Builders<Student>.Filter;
            FilterDefinition<Student> filter = builder.Eq(x => x.IdentityNumner, "050305007");

            long hackersCount = await SchoolContext.Students.CountAsync(filter);

            hackersCount.Should().Be(1);
        }
     
    }
}