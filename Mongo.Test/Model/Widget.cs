﻿using System;

namespace Mongo.Test.Model
{
    public class Widget
    {
        public int Id { get; set; }
        public int X { get; set; }

        public override string ToString()
        {
            return String.Format("Id: {0}, X: {1}", Id, X);
        }
    }
}