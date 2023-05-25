using Project.GameClasses.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Enemies
{
    internal class Bonfire : Enemy
    {
        public new static Image? Sprite = Image.FromFile("Images/bonfire.png");
        public Bonfire(string name, int x, int y, double damage) : base(name, x, y, damage)
        {
            Behaviour = new Spawner(this, typeof(Fire));
        }
    }
}
