using System;

namespace Mongo.Test.Model
{
    public class Person : BaseModel
    {
    
        public string Name { get; set; }
        public int Age { get; set; }
        public string Profession { get; set; }
        public string[] Favorites { get; set; }

        public override string ToString()
        {
            return String.Format("Id: {0}, Name: '{1}', Age: {2}, Profession: '{3}'", Id, Name, Age, Profession);
        }
    }
}