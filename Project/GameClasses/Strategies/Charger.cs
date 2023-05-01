using Project.GameClasses.EnviromentObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Strategies
{
    internal class Charger:Strategy
    {
        const int speed = 2;
         public Charger(Enemy enemy) : base(enemy) { }

        public override void Behave()
        {
            if (owner.X - speed >= Game.Player.X) { moveLeft(); }
            else if (owner.X + speed <= Game.Player.X) { moveRight(); }

            if (owner.Y - speed >= Game.Player.Y) { moveUp(); }
            else if (owner.Y + speed <= Game.Player.Y) { moveDown(); }

            if (Math.Sqrt(Math.Pow(owner.Y - Game.Player.Y, 2) + Math.Pow(owner.X + speed - Game.Player.X, 2)) < Game.Player.Size / 2 + owner.Size / 2)
            {
                Game.Player.DecreaseHealth(owner.Damage / 100);
            }
        }

        private List<Entity?> getNearEnviroment()
        {
            int approximateGridX = owner.X / 50;
            int approximateGridY = owner.Y / 50;

            if (approximateGridY < 0 || approximateGridY > 17 || approximateGridX < 0 || approximateGridX > 31) { return new List<Entity?>(); }
            List<Entity?> potentialCollisions = new List<Entity?>() { Game.EnviromentGrid[approximateGridY][approximateGridX] };

            if (approximateGridY > 0) potentialCollisions.Add(Game.EnviromentGrid[approximateGridY - 1][approximateGridX]);
            if (approximateGridY < 17) potentialCollisions.Add(Game.EnviromentGrid[approximateGridY + 1][approximateGridX]);
            if (approximateGridX > 0) potentialCollisions.Add(Game.EnviromentGrid[approximateGridY][approximateGridX - 1]);
            if (approximateGridX < 31) potentialCollisions.Add(Game.EnviromentGrid[approximateGridY][approximateGridX + 1]);

            if (approximateGridY > 0 && approximateGridX > 0) potentialCollisions.Add(Game.EnviromentGrid[approximateGridY - 1][approximateGridX - 1]);
            if (approximateGridY < 17 && approximateGridX > 0) potentialCollisions.Add(Game.EnviromentGrid[approximateGridY + 1][approximateGridX - 1]);
            if (approximateGridY > 0 && approximateGridX < 31) potentialCollisions.Add(Game.EnviromentGrid[approximateGridY - 1][approximateGridX + 1]);
            if (approximateGridY < 17 && approximateGridX < 31) potentialCollisions.Add(Game.EnviromentGrid[approximateGridY + 1][approximateGridX + 1]);
            return potentialCollisions;
        }
        private void moveUp()
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
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)        //коллизия квадратных объектов
                {
                    if (Math.Abs(owner.Y - speed - obj.Y) < owner.Size / 2 + obj.Size / 2 && Math.Abs(owner.X - obj.X) < owner.Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)        //результат коллизии круглого объекта
            {
                if (owner.X < collisionTarget.X) owner.X -= 1;
                else owner.X += 1;
                if (Math.Sqrt(Math.Pow(owner.Y - 1 - collisionTarget.Y, 2) + Math.Pow(owner.X - collisionTarget.X, 2)) > owner.Size / 2 + collisionTarget.Size / 2) owner.Y -= 1;       //"скользкость" круглых объектов за счет движения по другой оси,если ты врезаешься
                return;
            }

            if (collisions == 1) return;    //результат коллизии квадратного объекта

            if (owner.Y - owner.Size / 2 - speed < 0) return;
            owner.Y -= speed;  //можно обработать диагональность
        }

        private void moveDown()
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
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)
                {
                    if (Math.Abs(owner.Y + speed - obj.Y) < owner.Size / 2 + obj.Size / 2 && Math.Abs(owner.X - obj.X) < owner.Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)
            {
                if (owner.X < collisionTarget.X) owner.X -= 1;
                else owner.X += 1;
                if (Math.Sqrt(Math.Pow(owner.Y + 1 - collisionTarget.Y, 2) + Math.Pow(owner.X - collisionTarget.X, 2)) > owner.Size / 2 + collisionTarget.Size / 2) owner.Y += 1;    //"скользкость" за счет движения по другой оси,если ты врезаешься
                return;
            }

            if (collisions == 1) return;

            if (owner.Y + owner.Size / 2 + speed > 900) return;
            owner.Y += speed;
        }

        private void moveLeft()
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
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)
                {
                    if (Math.Abs(owner.Y - obj.Y) < owner.Size / 2 + obj.Size / 2 && Math.Abs(owner.X - speed - obj.X) < owner.Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)
            {
                if (owner.Y < collisionTarget.Y) owner.Y -= 1;
                else owner.Y += 1;
                if (Math.Sqrt(Math.Pow(owner.Y - collisionTarget.Y, 2) + Math.Pow(owner.X - 1 - collisionTarget.X, 2)) > owner.Size / 2 + collisionTarget.Size / 2) owner.X -= 1;
                return;
            }

            if (collisions == 1) return;

            if (owner.X - owner.Size / 2 - speed < 0) return;
            owner.X -= speed;
        }

        private void moveRight()
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
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)
                {
                    if (Math.Abs(owner.Y - obj.Y) < owner.Size / 2 + obj.Size / 2 && Math.Abs(owner.X + speed - obj.X) < owner.Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)
            {
                if (owner.Y < collisionTarget.Y) owner.Y -= 1;
                else owner.Y += 1;
                if (Math.Sqrt(Math.Pow(owner.Y - collisionTarget.Y, 2) + Math.Pow(owner.X + 1 - collisionTarget.X, 2)) > owner.Size / 2 + collisionTarget.Size / 2) owner.X += 1;
                return;
            }

            if (collisions == 1) return;

            if  (owner.X + owner.Size / 2 + speed > 1600) return;
            owner.X += speed;
        }
        

        
    }
}
