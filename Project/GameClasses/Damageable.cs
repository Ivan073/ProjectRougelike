using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses
{
    public abstract class Damageable : Entity
    {
        protected double maxHealth = 0;
        protected double health = 0;

        public Damageable()
        {
        }

        public Damageable(int x, int y, string name, double health)
        {
            X = x; Y = y; Name = name;
            this.health = health;
            maxHealth = health;
        }

        public virtual void DecreaseHealth(double delta) {
            health -= delta;
            if (health < 0)
            {
                health = 0;
            }
            if(health == 0)
            {
                destruction();
            }
        }

        public void IncreaseHealth(double delta)
        {
            health += delta;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }

        protected abstract void destruction();
    }
}
