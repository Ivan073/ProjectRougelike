using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Items.Weapons
{
    
    internal class ProjectileWeapon : Weapon
    {
        public double Speed { get; set; } = 10;
        public ProjectileWeapon(double damage, double size, double duration, double speed, double cost) : base(damage, size, duration, cost)
        {
            Speed = speed;
        }

        public override void Use()       //создать объект-оружие
        {
            if (Game.Player.UseMana(Cost))
            {
                Game.AddWeaponEntity(this);
            }
        }
    }
}
