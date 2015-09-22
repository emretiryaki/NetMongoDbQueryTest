using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Mongo.Test.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Mongo.Test.Tests
{


    public class InsertTests : BaseTest
    {

        #region Insert Bson Document

        [Fact]
        public void InsertOneBsonDocument()
        {
            var bsonDocument = new BsonDocument()
            {
                {"Name","Mehmet Çakır"},
                {"Email","er@efer.com"}
            };
            bsonDocument.Contains("_id").Should().BeFalse();
            Func<Task> act = () => BlogContext.UserAsBson.InsertOneAsync(bsonDocument);
            act.ShouldNotThrow();
            bsonDocument.Contains("_id").Should().BeTrue();
            bsonDocument["_id"].Should().NotBe(ObjectId.Empty);
        }

        [Fact]
        public void InsertManyBsonDocument()
        {
            var bsonDocument1 = new BsonDocument()
            {
                {"Name","Mehmet Çakır"},
                {"Email","er@efer.com"}
            };
            var bsonDocument2 = new BsonDocument()
            {
                {"Name","Mehmet Çakır"},
                {"Email","er@efer.com"}
            };


            bsonDocument1.Contains("_id").Should().BeFalse();
            bsonDocument2.Contains("_id").Should().BeFalse();


            Func<Task> act = () => BlogContext.UserAsBson.InsertManyAsync(new[] { bsonDocument1, bsonDocument2 });
            act.ShouldNotThrow();
            bsonDocument1["_id"].Should().NotBe(ObjectId.Empty);
            bsonDocument2["_id"].Should().NotBe(ObjectId.Empty);
        }

        #endregion

        #region Insert Model (Customer Primary Key)
        [Fact]
        public void InsertOneModel()
        {
            var studentList = new List<Student>
            {
                new Student {IdentityNumner = "050305008" ,Name = "Emre", Scores = new List<Score> {new Score{Point = 10,Type = "Turkish"} }}

            };

            Func<Task> insertFunc = () => SchoolContext.Students.InsertManyAsync(studentList);
            insertFunc.ShouldNotThrow();
            studentList.ForEach(x => x.Should().NotBe(ObjectId.Empty));
        }

        [Fact]
        public void ErrorWhenInsertDuplicateIdentiyNumberModel()
        {

            var student =
                new Student
                {
                    IdentityNumner = "050305009",
                    Name = "Esra",
                    Scores = new List<Score> { new Score { Point = 10, Type = "Turkish" } }
                };
            SchoolContext.Students.InsertOneAsync(student);



            var student2 =
               new Student
               {
                   IdentityNumner = "050305009",
                   Name = "Cafer",
                   Scores = new List<Score> { new Score { Point = 10, Type = "Turkish" } }
               };


            Func<Task> insertFunc = () => SchoolContext.Students.InsertOneAsync(student2);
            insertFunc.ShouldThrow<MongoException>();

        }
        #endregion

    }
}