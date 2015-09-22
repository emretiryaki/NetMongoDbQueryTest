using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mongo.Test.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.Test.Context.Base
{
    public class TestContext : DbContext
    {

        public const string WidgetsCollectionName = "widgets";
        public const string PeopleCollectionName = "people";
        public const string OrderCollectionName = "orders";

        private static readonly IEnumerable<BsonDocument> widgetData;


        protected override string DatabaseName
        {
            get { return "test"; }
        }



        public IMongoCollection<BsonDocument> WidgetsAsBson
        {
            get { return Database.GetCollection<BsonDocument>(WidgetsCollectionName); }
        }

        public IMongoCollection<Widget> Widgets
        {
            get { return Database.GetCollection<Widget>(WidgetsCollectionName); }
        }

        public IMongoCollection<BsonDocument> PeopleAsBson
        {
            get { return Database.GetCollection<BsonDocument>(PeopleCollectionName); }
        }

        public IMongoCollection<Person> People
        {
            get { return Database.GetCollection<Person>(PeopleCollectionName); }
        }

        public IMongoCollection<Order> Order
        {
            get { return Database.GetCollection<Order>(OrderCollectionName); }
        }
        public IMongoCollection<BsonDocument> OrderAsBson
        {
            get { return Database.GetCollection<BsonDocument>(OrderCollectionName); }
        }

        static TestContext()
        {
            widgetData = Enumerable
                .Range(0, 20)
                .Select(x => new BsonDocument("_id", x).Add("x", x));
        }

        private static readonly IEnumerable<Person> peopleData = new Person[]
        {
            new Person { Name = "Emre", Age = 23, Profession = "Developer" },
            new Person { Name = "Fatih", Age = 45, Favorites = new string[] { "ice cream", "beard" } },
            new Person { Name = "Esra", Age = 32, Favorites = new string[] { "beard", "cola" } },
            new Person { Name = "İdris", Age = 35, Favorites = new string[] { "cola", "pretzels", "cheese" } },
            new Person { Name = "Seher", Age = 34, Favorites = new string[] { "cola", "cheese" } },
            new Person { Name = "Mustafa", Age = 46, Profession = "Developer" },
           new Person { Name = "Mehmet", Age = 23, Profession = "Farmer" }
        };

        private static readonly IEnumerable<Order> Orders = new Order[]
        {
            new Order { Orderdate = DateTime.Now ,Name="SaleOrder1" ,Status = "A" , Totalprice = 50  , Items =  new List<Item>{ new Item  {Price = 25, Sku = "XXX", Qty = 1}, new Item{Price = 25, Sku = "YYY", Qty = 1}}},
            new Order { Orderdate = DateTime.Now ,Name="SaleOrder1" ,Status = "A" , Totalprice = 100 , Items =  new List<Item>{ new Item  {Price = 25, Sku = "XXX", Qty = 3}, new Item{Price = 25, Sku = "YYY", Qty = 1}}},
            new Order { Orderdate = DateTime.Now ,Name="SaleOrder2" ,Status = "R" , Totalprice = 20  , Items =  new List<Item>{ new Item  {Price = 20, Sku = "ZZZ", Qty = 1}}},
            new Order { Orderdate = DateTime.Now ,Name="SaleOrder3" ,Status = "A" , Totalprice = 30  , Items =  new List<Item>{new Item  {Price = 20, Sku = "ZZZ", Qty = 1}, new Item{Price = 10, Sku = "AAA", Qty = 1}}},
            new Order { Orderdate = DateTime.Now ,Name="SaleOrder4" ,Status = "R" , Totalprice = 10  , Items =  new List<Item>{new Item  {Price = 10, Sku = "AAA", Qty = 1}}},
            new Order { Orderdate = DateTime.Now ,Name="SaleOrder5" ,Status = "A" , Totalprice = 40  , Items =  new List<Item>{new Item  {Price = 10, Sku = "AAA", Qty = 4}}},
            new Order { Orderdate = DateTime.Now ,Name="SaleOrder7" ,Status = "S" , Totalprice = 100 , Items =  new List<Item>{new Item  {Price = 25, Sku = "XXX", Qty = 1}, new Item{Price = 25, Sku = "yyy", Qty = 3}}}

        };


        public TestContext(IMongoClient mongoClient) : base(mongoClient) { }

        public override async Task ResetData()
        {
            await Database.DropCollectionAsync(WidgetsCollectionName);
            await WidgetsAsBson.InsertManyAsync(widgetData);
            await Widgets.Indexes.CreateOneAsync(Builders<Widget>.IndexKeys.Ascending(x => x.X));

            await Database.DropCollectionAsync(PeopleCollectionName);
            await People.InsertManyAsync(peopleData);
            await People.Indexes.CreateOneAsync(Builders<Person>.IndexKeys.Ascending(x => x.Name).Ascending(x => x.Age));

            await Database.DropCollectionAsync(OrderCollectionName);
            await Order.InsertManyAsync(Orders);

        }
    }
}