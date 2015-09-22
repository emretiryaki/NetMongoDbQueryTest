using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Mongo.Test.Model;
using MongoDB.Driver;
using Xunit;

namespace Mongo.Test.Tests
{
    public class FindOneTests : BaseTest 
    {

        [Fact]
        public async Task FindOneAndUpdate()
        {
            UpdateDefinition<Widget> replacement = Builders<Widget>.Update.Inc(x => x.X, 1);
            Widget result = await TestContext.Widgets.FindOneAndUpdateAsync(x => x.X>18, replacement);
            result.X.Should().Be(19); 
        }
    }
}
