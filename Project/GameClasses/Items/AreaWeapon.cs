using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Items
{
    internal class AreaWeapon : Weapon
    {
        public AreaWeapon(double damage, double size, double duration, double cost) : base(damage, size, duration, cost)
        {
        }

        public override void Use()       //создать экземпляр
        {
            if (Game.Player.UseMana(Cost)) {
                Game.AddWeaponEntity(this);
            }
        }
    }
}
