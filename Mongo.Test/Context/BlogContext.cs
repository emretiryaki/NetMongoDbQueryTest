using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mongo.Test.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.Test.Context.Base
{
    public class BlogContext : DbContext
    {
        public const string PostsCollectionName = "posts";
        public const string UsersCollectionName = "users";
       
        public BlogContext(IMongoClient mongoClient)
            : base(mongoClient)
        {

        }

        protected override string DatabaseName
        {
            get { return "blog"; }
        }

    

        private static readonly IEnumerable<User> UserDataList = new User[]
        {
            new User { Name = "Emre Tiryaki", Email = "emr@tir.com" },
            new User { Name = "Mustafa Tiryaki", Email = null },
            new User { Name = "Cafer Tiryaki", Email = "cfr@tir.com" },
            new User { Name = "Erdi Tiryaki", Email = null },
            new User { Name = "İdris Özkan", Email = "idris@oz.com" },
            
        };

        private static readonly IEnumerable<Post> PostDataList = new Post[]
        {
            new Post
            {
                Author = "Okay Tiryakioğlu",
                Content = String.Empty,
                CreatedAtUtc = DateTime.UtcNow,
                Comments = null,
                Tags = new string[] { "Novel" },
                Title = "Gazi Osman Paşa"
            },
            new Post
            {
                Author = "Yavuz Bahadıroğlu",
                Content = String.Empty,
                CreatedAtUtc = DateTime.UtcNow,
                Comments = null,
                Tags = new string[] { "History","Novel" },
                Title = "Mısıra Doğru"
            },
            new Post
            {
                Author = "Eric Lippert",
                Content = String.Empty,
                CreatedAtUtc = DateTime.UtcNow,
                Comments = null,
                Tags = new string[] { "Software Development", "Programming", ".NET", "LINQ" },
                Title = "Computing a Cartesian Product"
            }
        };
        public IMongoCollection<Post> Posts
        {
            get { return Database.GetCollection<Post>(PostsCollectionName); }
        }

        public IMongoCollection<User> Users
        {
            get { return Database.GetCollection<User>(UsersCollectionName); }
        }
        public IMongoCollection<BsonDocument> UserAsBson
        {
            get { return Database.GetCollection<BsonDocument>(UsersCollectionName); }
        }
        public IMongoCollection<BsonDocument> PostAsBson
        {
            get { return Database.GetCollection<BsonDocument>(PostsCollectionName); }
        }

        public override async Task ResetData()
        {
            await Database.DropCollectionAsync(PostsCollectionName);
            await Database.DropCollectionAsync(UsersCollectionName);

            await Users.InsertManyAsync(UserDataList);
            await Posts.InsertManyAsync(PostDataList);

            await Users.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Ascending(x => x.Name));
        }
    }
}