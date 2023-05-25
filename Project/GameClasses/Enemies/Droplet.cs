using Project.GameClasses.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Enemies
{
    internal class Droplet:Enemy
    {
        public new static Image? Sprite = Image.FromFile("Images/droplet.png");
        public Droplet(string name, int x, int y, double damage) : base(name, x, y, damage)
        {
            Behaviour = new Charger(this);
        }
    }
}
