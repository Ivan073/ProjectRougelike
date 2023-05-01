using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Items
{
    internal abstract class Weapon : Item
    {
        public double Damage { get; set; }
        public double Size { get; set; }
        public double Duration { get; set; }
        public Weapon(double damage, double size, double duration,double cost) { 
            Damage = damage;
            Duration = duration;
            Size = size;
            Cost = cost;
        }

    }
}
