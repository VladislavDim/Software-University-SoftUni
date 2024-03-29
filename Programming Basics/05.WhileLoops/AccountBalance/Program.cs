﻿using System;

namespace AccountBalance
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string input = Console.ReadLine();
            double balance = 0;

            while (input!= "NoMoreMoney")
            {
                double currentValue = double.Parse(input);
                if (currentValue < 0)
                {
                    Console.WriteLine("Invalid operation!");
                    break;
                }

                balance += currentValue;
                Console.WriteLine($"Increase: {currentValue:f2}");

                input = Console.ReadLine();
            }

            Console.WriteLine($"Total: {balance:f2}");



        }
    }
}
