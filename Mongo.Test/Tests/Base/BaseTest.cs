using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mongo.Test.Context;
using Mongo.Test.Context.Base;
using Mongo.Test.Model;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Mongo.Test
{
    public class BaseTest
    {
        protected readonly BlogContext BlogContext;
        protected readonly SchoolContext SchoolContext;
        protected readonly TestContext TestContext;
        private static string MongoServerAddress
        {
            get { return ConfigurationManager.AppSettings["MongoServerAddress"]; }
        }

        private static string MongoServerPort
        {
            get { return ConfigurationManager.AppSettings["MongoServerPort"]; }
        }



        protected BaseTest()
        {
            //  http://stackoverflow.com/questions/19521626/mongodb-convention-packs
            ConventionPack conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, x => true);

            string connectionString = String.Format("mongodb://{0}:{1}", MongoServerAddress, MongoServerPort);
            var mongoClient = new MongoClient(connectionString);

            BlogContext = new BlogContext(mongoClient);
            Task blogContextResetDataTask = BlogContext.ResetData();


            SchoolContext = new SchoolContext(mongoClient);
            Task schoolContextResetDataTask = SchoolContext.ResetData();


            TestContext = new TestContext(mongoClient);
            Task testContextResetDataTask = TestContext.ResetData();


            testContextResetDataTask.Wait();
            blogContextResetDataTask.Wait();
            schoolContextResetDataTask.Wait();

        }

    }


}
