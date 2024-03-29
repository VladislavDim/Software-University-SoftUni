﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoveNegativesAndReverse
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .Where(x => x > 0)
                .Reverse()
                .ToList();

            if (numbers.Count > 0)
            {
                Console.WriteLine(string.Join(" ",numbers));
            }
            else
            {
                Console.WriteLine("empty");
            }
        }
    }
}
