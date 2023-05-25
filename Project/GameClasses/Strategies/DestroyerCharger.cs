using Project.GameClasses.Enemies;
using Project.GameClasses.EnviromentObjects.RoundEnvirometObjects;
using Project.GameClasses.EnviromentObjects.SquareEnviromentObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Strategies
{
    internal class DestroyerCharger : Charger
    {
        public DestroyerCharger(Enemy enemy) : base(enemy) { }
        protected override void moveUp()
        {
            short collisions = 0;
            Entity? collisionTarget = null;
            List<Entity?> potentialCollisions = getNearEnviroment();    //проверка только соседних элементов в сетке окружения

            foreach (var obj in potentialCollisions)
            {
                if (obj is RoundEnviromentObject)        //коллизия круглых объектов
                {
                    if (Math.Sqrt(Math.Pow(owner.Y - speed - obj.Y, 2) + Math.Pow(owner.X - obj.X, 2)) < owner.Size / 2 + obj.Size / 2)
                    {
                        ((RoundEnviromentObject)obj).DecreaseHealth(owner.Damage / 100);
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)        //коллизия квадратных объектов
                {
                    if (Math.Abs(owner.Y - speed - obj.Y) < owner.Size / 2 + obj.Size / 2 && Math.Abs(owner.X - obj.X) < owner.Size / 2 + obj.Size / 2)
                    {
                        ((SquareEnviromentObject)obj).DecreaseHealth(owner.Damage / 100);
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1) collides = true;

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)        //результат коллизии круглого объекта
            {
                if (owner.X < collisionTarget.X) owner.X -= 1;
                else owner.X += 1;
                if (Math.Sqrt(Math.Pow(owner.Y - 1 - collisionTarget.Y, 2) + Math.Pow(owner.X - collisionTarget.X, 2)) > owner.Size / 2 + collisionTarget.Size / 2) owner.Y -= 1;       //"скользкость" круглых объектов за счет движения по другой оси,если ты врезаешься
                return;
            }

            if (collisions == 1) return;    //результат коллизии квадратного объекта

            collides = false;

            if (owner.Y - owner.Size / 2 - speed < 0) return;
            owner.Y -= speed;
        }

        protected override void moveDown()
        {
            short collisions = 0;
            Entity? collisionTarget = null;
            List<Entity?> potentialCollisions = getNearEnviroment();

            foreach (var obj in potentialCollisions)
            {
                if (obj is RoundEnviromentObject)
                {
                    if (Math.Sqrt(Math.Pow(owner.Y + speed - obj.Y, 2) + Math.Pow(owner.X - obj.X, 2)) < owner.Size / 2 + obj.Size / 2)
                    {
                        ((RoundEnviromentObject)obj).DecreaseHealth(owner.Damage / 100);
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)
                {
                    if (Math.Abs(owner.Y + speed - obj.Y) < owner.Size / 2 + obj.Size / 2 && Math.Abs(owner.X - obj.X) < owner.Size / 2 + obj.Size / 2)
                    {
                        ((SquareEnviromentObject)obj).DecreaseHealth(owner.Damage / 100);
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1) collides = true;

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)
            {
                if (owner.X < collisionTarget.X) owner.X -= 1;
                else owner.X += 1;
                if (Math.Sqrt(Math.Pow(owner.Y + 1 - collisionTarget.Y, 2) + Math.Pow(owner.X - collisionTarget.X, 2)) > owner.Size / 2 + collisionTarget.Size / 2) owner.Y += 1;    //"скользкость" за счет движения по другой оси,если ты врезаешься
                return;
            }

            if (collisions == 1) return;

            collides = false;

            if (owner.Y + owner.Size / 2 + speed > 900) return;
            owner.Y += speed;
        }

        protected override void moveLeft()
        {
            short collisions = 0;
            Entity? collisionTarget = null;
            List<Entity?> potentialCollisions = getNearEnviroment();

            foreach (var obj in potentialCollisions)
            {
                if (obj is RoundEnviromentObject)
                {
                    if (Math.Sqrt(Math.Pow(owner.Y - obj.Y, 2) + Math.Pow(owner.X - speed - obj.X, 2)) < owner.Size / 2 + obj.Size / 2)
                    {
                        ((RoundEnviromentObject)obj).DecreaseHealth(owner.Damage / 100);
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)
                {
                    if (Math.Abs(owner.Y - obj.Y) < owner.Size / 2 + obj.Size / 2 && Math.Abs(owner.X - speed - obj.X) < owner.Size / 2 + obj.Size / 2)
                    {
                        ((SquareEnviromentObject)obj).DecreaseHealth(owner.Damage / 100);
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1) collides = true;

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)
            {
                if (owner.Y < collisionTarget.Y) owner.Y -= 1;
                else owner.Y += 1;
                if (Math.Sqrt(Math.Pow(owner.Y - collisionTarget.Y, 2) + Math.Pow(owner.X - 1 - collisionTarget.X, 2)) > owner.Size / 2 + collisionTarget.Size / 2) owner.X -= 1;
                return;
            }

            if (collisions == 1) return;

            collides = false;

            if (owner.X - owner.Size / 2 - speed < 0) return;
            owner.X -= speed;
        }

        protected override void moveRight()
        {
            short collisions = 0;
            Entity? collisionTarget = null;
            List<Entity?> potentialCollisions = getNearEnviroment();

            foreach (var obj in potentialCollisions)
            {
                if (obj is RoundEnviromentObject)
                {
                    if (Math.Sqrt(Math.Pow(owner.Y - obj.Y, 2) + Math.Pow(owner.X + speed - obj.X, 2)) < owner.Size / 2 + obj.Size / 2)
                    {
                        ((RoundEnviromentObject)obj).DecreaseHealth(owner.Damage / 100);
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)
                {
                    if (Math.Abs(owner.Y - obj.Y) < owner.Size / 2 + obj.Size / 2 && Math.Abs(owner.X + speed - obj.X) < owner.Size / 2 + obj.Size / 2)
                    {
                        ((SquareEnviromentObject)obj).DecreaseHealth(owner.Damage / 100);
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1) collides = true;

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)
            {
                if (owner.Y < collisionTarget.Y) owner.Y -= 1;
                else owner.Y += 1;
                if (Math.Sqrt(Math.Pow(owner.Y - collisionTarget.Y, 2) + Math.Pow(owner.X + 1 - collisionTarget.X, 2)) > owner.Size / 2 + collisionTarget.Size / 2) owner.X += 1;
                return;
            }

            if (collisions == 1) return;

            collides = false;

            if (owner.X + owner.Size / 2 + speed > 1600) return;
            owner.X += speed;
        }
    }
}
