using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Test.Model
{
    public class Student
    {


        [BsonId]
        public string IdentityNumner { get; set; }
        public string Name { get; set; }
        public ICollection<Score> Scores { get; set; }
    }

    public class Score
    {
        public string Type { get; set; }

        [BsonElement("score")]
        public double Point { get; set; }
    }

}