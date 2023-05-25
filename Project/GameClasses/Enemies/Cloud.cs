using Project.GameClasses.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design.Behavior;

namespace Project.GameClasses.Enemies
{
    internal class Cloud : Enemy
    {
        public new static Image? Sprite = Image.FromFile("Images/cloud.png");
        public Cloud(string name, int x, int y, double damage) : base(name, x, y, damage)
        {
            Behaviour = new Spawner(this, typeof(Droplet));
        }
    }
}
