using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Mongo.Test.Context.Base;
using Mongo.Test.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.Test.Context
{
    public class SchoolContext : DbContext
    {
        public const string StudentsCollectionName = "students";

        public SchoolContext(IMongoClient mongoClient)
            : base(mongoClient)
        {

        }

        protected override string DatabaseName
        {
            get { return "school"; }
        }

        private static readonly IEnumerable<Student> StudentDataList = new List<Student>()
        {
            new Student
            {
                
                Name = "Fatih Tiryaki",
                IdentityNumner = "050305007",
                Scores = new List<Score>() {new Score() { Point = 70,Type = "Maths"}, new Score() { Point = 80,Type = "Geo"}},
               
                
                
            }
        };
       
        public IMongoCollection<BsonDocument> StudentsAsBson
        {
            get { return Database.GetCollection<BsonDocument>(StudentsCollectionName); }
        }

        public IMongoCollection<Student> Students
        {
            get
            {
                return Database.GetCollection<Student>(StudentsCollectionName);
            }
        }
        public override async Task ResetData()
        {
            await Database.DropCollectionAsync(StudentsCollectionName);
            await Students.InsertManyAsync(StudentDataList);

            var keys = Builders<Student>.IndexKeys.Ascending(x => x.Name);
            await Students.Indexes.CreateOneAsync(keys);
        }
    }
}
