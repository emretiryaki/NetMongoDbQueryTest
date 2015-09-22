using System;

namespace Mongo.Test.Model
{
    public class Comment
    {
        public string Author { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}