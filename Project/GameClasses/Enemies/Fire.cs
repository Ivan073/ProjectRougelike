using Project.GameClasses.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Enemies
{
    internal class Fire : Enemy
    {
        public new static Image? Sprite = Image.FromFile("Images/fire.png");
        public Fire(string name, int x, int y, double damage) : base(name, x, y, damage)
        {
            Behaviour = new DestroyerCharger(this);
        }
    }
}
