using Project.GameClasses.EnviromentObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Items.Weapons
{
    public class AreaWeaponEntity : WeaponEntity
    {
        public new static Image? Sprite = Image.FromFile("Images/areaWeapon.png");

        AreaWeapon AssociatedWeapon;

        private System.Threading.Timer? duration = null;
        private System.Threading.Timer? damageTimer = null;

        public AreaWeaponEntity(AreaWeapon associatedWeapon)
        {
            AssociatedWeapon = associatedWeapon;
            X = (int)(Control.MousePosition.X / Window.ScreenScale);
            Y = (int)(Control.MousePosition.Y / Window.ScreenScale);
            Size = (int)associatedWeapon.Size;
            Name = "AreaWeaponEntity";
            duration = new System.Threading.Timer(new TimerCallback((s) =>
            {
                Game.RemoveEntity(this);
                duration.Dispose();
                damageTimer.Dispose();
                damageTimer = null;
                duration = null;
            }), null, (int)(associatedWeapon.Duration * 1000), -1);

            damageTimer = new System.Threading.Timer(new TimerCallback((s) =>
            {
                lock (Game.EnemyLock)
                {
                    foreach (var enemy in Game.Enemies)
                    {
                        if (Math.Sqrt(Math.Pow(Y - enemy.Y, 2) + Math.Pow(X - enemy.X, 2)) < Size / 2 + enemy.Size / 2)
                        {
                            Task.Factory.StartNew(() => enemy.DecreaseHealth(associatedWeapon.Damage / 200));
                        }
                    }
                }
            }), null, 0, 5);
        }
    }
}
