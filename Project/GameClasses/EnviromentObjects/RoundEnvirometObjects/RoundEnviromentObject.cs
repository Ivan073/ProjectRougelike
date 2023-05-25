using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.EnviromentObjects.RoundEnvirometObjects
{
    public class RoundEnviromentObject : Damageable
    {
        public RoundEnviromentObject()
        {
        }

        public RoundEnviromentObject(int x, int y, string name, int size, double health)
        {
            X = x; Y = y; Name = name;
            this.health = health;
            maxHealth = health;
            Size = size;
        }

        protected override void destruction()
        {
            Game.RemoveEnviroment(this);
        }
    }
}
