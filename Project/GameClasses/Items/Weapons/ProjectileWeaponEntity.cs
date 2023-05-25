using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project.GameClasses.Items.Weapons
{
    internal class ProjectileWeaponEntity : WeaponEntity
    {
        public new static Image? Sprite = Image.FromFile("Images/projectileWeapon.png");

        ProjectileWeapon AssociatedWeapon;

        private System.Threading.Timer? duration = null;
        private System.Threading.Timer? damageTimer = null;
        private System.Threading.Timer? moveTimer = null;

        double moveRatio;
        bool topDir;
        bool leftDir;

        public ProjectileWeaponEntity(ProjectileWeapon associatedWeapon)
        {
            AssociatedWeapon = associatedWeapon;
            X = (int)(Game.Player.X);
            Y = (int)(Game.Player.Y);
            Size = (int)associatedWeapon.Size;
            Name = "ProjectileWeaponEntity";
            duration = new System.Threading.Timer(new TimerCallback((s) =>
            {
                Game.RemoveEntity(this);
                duration?.Dispose();
                damageTimer?.Dispose();
                moveTimer?.Dispose();
                damageTimer = null;
                duration = null;
                moveTimer = null;
            }), null, (int)(associatedWeapon.Duration * 1000), -1);

            damageTimer = new System.Threading.Timer(new TimerCallback((s) =>
            {
                lock (Game.EnemyLock)
                {
                    foreach (var enemy in Game.Enemies)
                    {
                        if (Math.Sqrt(Math.Pow(Y - enemy.Y, 2) + Math.Pow(X - enemy.X, 2)) < Size / 2 + enemy.Size / 2)
                        {
                            Task.Factory.StartNew(() => enemy.DecreaseHealth(associatedWeapon.Damage / 20));
                        }
                    }
                }
            }), null, 0, 5);

            
            int Xdif = (int)(Control.MousePosition.X / Window.ScreenScale) - X;
            
            int Ydif = (int)(Control.MousePosition.Y / Window.ScreenScale) - Y;
            
            if (Ydif == 0) { Ydif = 1; }
            if (Xdif == 0) { Xdif = 1; }
            topDir = Ydif < 0;
            leftDir = Xdif < 0;
            moveRatio = Math.Abs((double)Xdif / Ydif);

            moveTimer = new System.Threading.Timer(new TimerCallback((s) =>
            {
                X += (int)( associatedWeapon.Speed / (Math.Sqrt(1+1/(moveRatio*moveRatio))) ) * ( (leftDir)?-1:1);
                Y += (int)(associatedWeapon.Speed / (Math.Sqrt(1 + 1 / (moveRatio * moveRatio))) /moveRatio ) * ((topDir) ? -1 : 1);
            }), null, 0, 5);
        }
    }
}
