using Project.GameClasses.EnviromentObjects.RoundEnvirometObjects;
using Project.GameClasses.EnviromentObjects.SquareEnviromentObjects;
using Project.GameClasses.Items;
using Project.GameClasses.Items.Weapons;
using System.Drawing;
using System.Security.Policy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project.GameClasses
{
    public class Player : Damageable
    {
        public new static Image? Sprite = Image.FromFile("Images/player.png");
        private string direction = "up";

        public delegate void Empty();
        public event Empty ExitReached;

        private static System.Threading.Timer? upTimer = null;
        private static System.Threading.Timer? downTimer = null;
        private static System.Threading.Timer? leftTimer = null;
        private static System.Threading.Timer? rightTimer = null;

        private static System.Threading.Timer? manaRestore = null;

        public double Health { get { return health; } }
        public double MaxHealth { get { return maxHealth; } }

        private double mana;
        private double maxMana;
        public double Mana { get { return mana; } }
        public double MaxMana { get { return maxMana; } }

        public Armor? Armor { get; set; } = null;
         
        public Consumable? Consumable { get; set; } = null;
        public Weapon? FirstWeapon { get; set; } = null;
        public System.Threading.Timer? FirstWeaponCooldownTimer = null;
        public Weapon? SecondWeapon { get; set; } = null;
        public System.Threading.Timer? SecondWeaponCooldownTimer = null;



        public Player()
        {
            Sprite.RotateFlip(RotateFlipType.Rotate90FlipNone);
            X = 800;
            Y = 825;
            health = 100;
            maxHealth = 100;
            mana = 100;
            maxMana = 100;
            Size = 50;
            Name = "Player";

            manaRestore = new System.Threading.Timer(new TimerCallback((s) =>
            {
                if (Armor != null)
                if (!Armor.Active && mana + 1 <=maxMana) { mana++; }
            }), null, 0, 250);

            Armor = new Armor(10, 0.8);
            Consumable = new ConsumableHeal(100,25);
            FirstWeapon = new AreaWeapon(50, 300, 1.5, 3);
            SecondWeapon = new ProjectileWeapon(100, 60, 0.8, 10, 3);
        }

        public bool UseMana(double toUse)
        {
            if (toUse<Mana) { mana -= toUse; return true; }
            else return false;
        }
        
        public void Attack() { }
        public void Move(Keys key)
        {
            switch (key)
            {
                case Keys.W:
                    {
                        string[] dirs = { "up", "left", "down", "right" };
                        for (int i = 0; i < 4; i++)
                        {
                            if (direction != dirs[i]) { Sprite.RotateFlip(RotateFlipType.Rotate90FlipNone); }
                            else { break; }
                        }
                        direction = "up";



                        downTimer?.Dispose();
                        upTimer?.Dispose();
                        upTimer = new System.Threading.Timer(new TimerCallback((s) =>
                        {
                            moveUp();
                        }), null, 0, 5);
                        break;
                    }
                case Keys.A:
                    {
                        string[] dirs = {  "left", "down", "right", "up", };
                        for (int i = 0; i < 4; i++)
                        {
                            if (direction != dirs[i]) { Sprite.RotateFlip(RotateFlipType.Rotate90FlipNone); }
                            else { break; }
                        }
                        direction = "left";
                        rightTimer?.Dispose();
                        leftTimer?.Dispose();
                        leftTimer = new System.Threading.Timer(new TimerCallback((s) =>
                        {
                            moveLeft();
                        }), null, 0, 5);
                        break;
                    }
                case Keys.S:
                    {
                        string[] dirs = {  "down", "right", "up", "left" };
                        for (int i = 0; i < 4; i++)
                        {
                            if (Game.Player.direction != dirs[i]) { Sprite.RotateFlip(RotateFlipType.Rotate90FlipNone); }
                            else { break; }
                        }
                        direction = "down";
                        upTimer?.Dispose();
                        downTimer?.Dispose();
                        downTimer = new System.Threading.Timer(new TimerCallback((s) =>
                        {
                            moveDown();
                        }), null, 0, 5);
                        break;
                    }
                case Keys.D:
                    {
                        string[] dirs = {  "right", "up", "left", "down" };
                        for (int i = 0; i < 4; i++)
                        {
                            if (Game.Player.direction != dirs[i]) { Sprite.RotateFlip(RotateFlipType.Rotate90FlipNone); }
                            else { break; }
                        }
                        direction = "right";
                        leftTimer?.Dispose();
                        rightTimer?.Dispose();
                        rightTimer = new System.Threading.Timer(new TimerCallback((s) =>
                        {
                            moveRight();
                        }), null, 0, 5);
                        break;
                    }
            }
        }

        public void Stop(Keys key)
        {
            switch (key)
            {
                case Keys.W:
                    {
                        upTimer?.Dispose();
                        break;
                    }
                case Keys.A:
                    {
                        leftTimer?.Dispose();
                        break;
                    }
                case Keys.S:
                    {
                        downTimer?.Dispose();
                        break;
                    }
                case Keys.D:
                    {
                        rightTimer?.Dispose();
                        break;
                    }
            }
        }

        public void UseArmor() {
            Armor?.Use();
        }

        public void UseConsumable()
        {
            Consumable?.Use();
        }

        public void UseFirstWeapon()
        {
            if (FirstWeaponCooldownTimer == null ) {
                FirstWeapon?.Use();
                FirstWeaponCooldownTimer = new System.Threading.Timer(new TimerCallback((s) =>
                {
                    FirstWeaponCooldownTimer?.Dispose();
                    FirstWeaponCooldownTimer = null;
                }), null, 500, Timeout.Infinite);
            }
            
        }

        public void UseSecondWeapon()
        {
            if (SecondWeaponCooldownTimer == null)
            {
                SecondWeapon?.Use();
                SecondWeaponCooldownTimer = new System.Threading.Timer(new TimerCallback((s) =>
                {
                    SecondWeaponCooldownTimer?.Dispose();
                    SecondWeaponCooldownTimer = null;
                }), null, 500, Timeout.Infinite);
            }
        }

        public void PickupItem()
        {
            
            foreach (PickableItemEntity item in Game.Items)
            {
                if (Math.Sqrt(Math.Pow(Y - item.Y, 2) + Math.Pow(X - item.X, 2)) <= Size / 2 + item.Size / 2)       //игрок касается
                {
                    
                    if (item.AssociatedItem is Weapon)
                    {
                        var temp = SecondWeapon;
                        SecondWeapon = FirstWeapon;
                        FirstWeapon =(Weapon) item.AssociatedItem;
                        Game.RemovePickup();
                        Game.AddItem(temp, item.X, item.Y);
                        break;
                    }

                    if (item.AssociatedItem is Armor)
                    {
                        var temp = Armor;
                        Armor = (Armor)item.AssociatedItem;
                        Game.RemovePickup();
                        Game.AddItem(temp, item.X, item.Y);
                        break;
                    }

                    if (item.AssociatedItem is Consumable)
                    {
                        var temp = Consumable;
                        Consumable = (Consumable)item.AssociatedItem;
                        Game.RemovePickup();
                        Game.AddItem(temp, item.X, item.Y);
                        break;
                    }

                }
            }
        }

        public override void DecreaseHealth(double delta)
        {
            if (Armor!=null && Armor.Active)
            {
                health -= delta * (1-Armor.DefencePercent);
            }
            else health -= delta;
            if (health < 0)
            {
                health = 0;
            }
            if (health == 0)
            {
                destruction();
            }
        }



        protected override void destruction()
        {
            Game.PlayerDeath();
        }

        private List<Entity?> getNearEnviroment()
        {
            int approximateGridX = X / 50;
            int approximateGridY = Y / 50;

            if (approximateGridY<0 || approximateGridY>17 || approximateGridX <0 || approximateGridX > 31) { return new List<Entity?>(); }
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
            if (Y < 30) { ExitReached(); }
            short collisions = 0;
            Entity? collisionTarget = null;
            List<Entity?> potentialCollisions = getNearEnviroment();    //проверка только соседних элементов в сетке окружения

            foreach (var obj in potentialCollisions)
            {
                if (obj is RoundEnviromentObject)        //коллизия круглых объектов
                {
                    if (Math.Sqrt(Math.Pow(Y - 5 - obj.Y, 2) + Math.Pow(X - obj.X, 2)) < Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)        //коллизия квадратных объектов
                {
                    if (Math.Abs(Y - 5 - obj.Y) < Size / 2 + obj.Size / 2 && Math.Abs(X - obj.X) < Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)        //результат коллизии круглого объекта
            {
                if (X < collisionTarget.X) X -= 1;
                else X += 1;
                if (Math.Sqrt(Math.Pow(Y - 1 - collisionTarget.Y, 2) + Math.Pow(X - collisionTarget.X, 2)) > Size / 2 + collisionTarget.Size / 2) Y -= 1;       //"скользкость" круглых объектов за счет движения по другой оси,если ты врезаешься
                return;
            }

            if (collisions == 1) return;    //результат коллизии квадратного объекта



            if (Y - Size / 2 - 5 < 0)return;
            Y -= 5;
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
                    if (Math.Sqrt(Math.Pow(Y + 5 - obj.Y, 2) + Math.Pow(X - obj.X, 2)) < Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)
                {
                    if (Math.Abs(Y + 5 - obj.Y) < Size / 2 + obj.Size / 2 && Math.Abs(X - obj.X) < Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)
            {
                if (X < collisionTarget.X) X -= 1;
                else X += 1;
                if (Math.Sqrt(Math.Pow(Y + 1 - collisionTarget.Y, 2) + Math.Pow(X - collisionTarget.X, 2)) > Size / 2 + collisionTarget.Size / 2) Y += 1;    //"скользкость" за счет движения по другой оси,если ты врезаешься
                return;
            }

            if (collisions == 1) return;

            if (Y + Size / 2 + 5 > 900)return;
            Y += 5;
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
                    if (Math.Sqrt(Math.Pow(Y - obj.Y, 2) + Math.Pow(X - 5 - obj.X, 2)) < Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)
                {
                    if (Math.Abs(Y - obj.Y) < Size / 2 + obj.Size / 2 && Math.Abs(X - 5 - obj.X) < Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)
            {
                if (Y < collisionTarget.Y) Y -= 1;
                else Y += 1;
                if (Math.Sqrt(Math.Pow(Y - collisionTarget.Y, 2) + Math.Pow(X - 1 - collisionTarget.X, 2)) > Size / 2 + collisionTarget.Size / 2) X -= 1;
                return;
            }

            if (collisions == 1) return;

            if (X - Size / 2 - 5 < 0) return;
            X -= 5;
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
                    if (Math.Sqrt(Math.Pow(Y - obj.Y, 2) + Math.Pow(X + 5 - obj.X, 2)) < Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }

                if (obj is SquareEnviromentObject)
                {
                    if (Math.Abs(Y - obj.Y) < Size / 2 + obj.Size / 2 && Math.Abs(X + 5 - obj.X) < Size / 2 + obj.Size / 2)
                    {
                        collisions++;
                        collisionTarget = obj;
                        if (collisions > 1) { return; }
                    }
                }
            }

            if (collisions == 1 && collisionTarget is RoundEnviromentObject)
            {
                if (Y < collisionTarget.Y) Y -= 1;
                else Y += 1;
                if (Math.Sqrt(Math.Pow(Y - collisionTarget.Y, 2) + Math.Pow(X + 1 - collisionTarget.X, 2)) > Size / 2 + collisionTarget.Size / 2) X += 1;
                return;
            }

            if (collisions == 1) return;

            if (X + Size / 2 + 5 > 1600) return;
            X += 5;
        }

        
    }
}
