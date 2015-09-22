using System;
using System.Collections.Generic;

namespace Mongo.Test.Model
{
    public class Order : BaseModel
    {
        public DateTime Orderdate { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int Totalprice { get; set; }
        public List<Item> Items { get; set; }
    }
}