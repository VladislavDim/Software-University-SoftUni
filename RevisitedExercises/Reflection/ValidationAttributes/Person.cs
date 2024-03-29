﻿using System;
using System.Collections.Generic;
using System.Text;
using ValidationAttributes.Attributes;

namespace ValidationAttributes
{
    public class Person
    {
        private const int MinAge = 19;
        private const int MaxAge = 90;
        public Person(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }

        [MyRequired]
        public string Name { get; set; }

        [MyRange(MinAge, MaxAge)]
        public int Age { get; set; }
    }
}
