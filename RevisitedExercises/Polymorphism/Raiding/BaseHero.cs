﻿namespace Raiding
{
    public abstract class BaseHero : IHero
    {
        public BaseHero(string name, int power)
        {
            this.Name = name;
            this.Power = power;
        }
        public string Name { get; set; }
        public int Power { get; set; }

        public abstract string CastAbility();
    }
}
