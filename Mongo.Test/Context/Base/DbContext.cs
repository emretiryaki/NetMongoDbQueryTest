using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Mongo.Test.Context.Base
{
    public abstract class DbContext
    {
        protected abstract string DatabaseName { get; }

        private   IMongoDatabase _database;

        private readonly IMongoClient _mongoClient;


        protected DbContext(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public IMongoDatabase Database
        {
            get { return _database ?? (_database = _mongoClient.GetDatabase(DatabaseName)); }
        }
        public virtual async Task ResetData()
        {
            await new Task(() => { });
        }
      

    }
}
