using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project.GameClasses.EnviromentObjects.SquareEnviromentObjects
{
    public class SquareEnviromentObject : Damageable
    {
        public SquareEnviromentObject()
        {
        }

        public SquareEnviromentObject(int x, int y, string name, int size, double health)
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
