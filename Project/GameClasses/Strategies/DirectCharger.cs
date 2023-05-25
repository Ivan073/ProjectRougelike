using Project.GameClasses.Enemies;
using Project.GameClasses.EnviromentObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Strategies
{
    internal class DirectCharger : Charger
    {
        public DirectCharger(Enemy enemy) : base(enemy) { }

        protected override void moveUp()
        {
            if (owner.Y - owner.Size / 2 - speed < 0) return;
            owner.Y -= speed;
        }

        protected override void moveDown()
        {
            if (owner.Y + owner.Size / 2 + speed > 900) return;
            owner.Y += speed;
        }

        protected override void moveLeft()
        {
            if (owner.X - owner.Size / 2 - speed < 0) return;
            owner.X -= speed;
        }

        protected override void moveRight()
        {
            if (owner.X + owner.Size / 2 + speed > 1600) return;
            owner.X += speed;
        }
    }
}
