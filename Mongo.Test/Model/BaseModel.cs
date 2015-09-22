using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Test.Model
{
    public class BaseModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}