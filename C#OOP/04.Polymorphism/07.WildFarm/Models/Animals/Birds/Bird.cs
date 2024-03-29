﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WildFarm.Models.Animals.Birds
{
    public abstract class Bird : Animal
    {
        protected Bird(
            string name,
            double weight,
            HashSet<string> allowedFoods,
            double weightModifier,
            double wingSize
            )
            : base(name, weight, allowedFoods, weightModifier)
        {
            WingSize = wingSize;
        }
        public double WingSize { get; private set; }

        public override string ToString()
        {
            return $"{this.GetType().Name} [{Name}, {WingSize}, {Weight}, {FoodEaten}]";
        }
    }
}
