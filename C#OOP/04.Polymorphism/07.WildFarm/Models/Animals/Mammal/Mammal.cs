﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WildFarm.Models.Animals.Mammal
{
    public abstract class Mammal : Animal
    {
        protected Mammal(
            string name,
            double weight,
            HashSet<string> allowedFoods,
            double weightModifier,
            string livingRegion
            )
            : base(name, weight, allowedFoods, weightModifier)
        {
            LivingRegion = livingRegion;
        }
        public string LivingRegion { get; private set; }
        public override string ToString()
        {
            return $"{this.GetType().Name} [{Name}, {Weight}, {LivingRegion}, {FoodEaten}]";
        }
    }
}
