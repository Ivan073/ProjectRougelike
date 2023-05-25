using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Project.GameClasses.Enemies;
using Project.GameClasses.EnviromentObjects.RoundEnvirometObjects;
using Project.GameClasses.EnviromentObjects.SquareEnviromentObjects;
using Project.GameClasses.Items;
using Project.GameClasses.Items.Weapons;

namespace Project.GameClasses
{
    static public class Game
    {
        private static System.Windows.Forms.Timer refreshTimer = new System.Windows.Forms.Timer();
        private static System.Threading.Timer? autosave = null;
        private static Window? window = null;

        public static Player? Player = null;

        public static List<Enemy> Enemies = new ();
        public static object EnemyLock = new();

        public static List<List<Entity?>> EnviromentGrid = new(); //32x18, поле 1600x900

        public static List<PickableItemEntity> Items = new();

        public static int Level = 1;

        public static void SetWindow(Window _window) {
            window = _window;
        }
        public static void Start()
        {
            createEntities();

            /*autosave = new System.Threading.Timer(new TimerCallback((s) =>  //автосохранение каждые 2 секунды
            {
                var serializer = new Serialization.JSONSerializer();
                var jsonEnemies = JsonSerializer.Serialize(Enemies);
                File.WriteAllText("enemies.json", jsonEnemies);
                var jsonEnviroment = JsonSerializer.Serialize(EnviromentGrid);
                File.WriteAllText("enviroment.json", jsonEnviroment);
                var jsonPlayer = JsonSerializer.Serialize(Player);
                File.WriteAllText("player.json", jsonPlayer);
            }), null, 0, 2000);*/

            refreshTimer.Interval = 15;
            refreshTimer.Tick += windowRefresh;
            refreshTimer.Start();
        }
        
        private static void createEntities() //начальная инциализация некоторых сущностей
        {
            Player = new Player();
            window.Entities.Add(Player);
            Player.ExitReached += createLevel;

            createLevel();

            /* if (File.Exists("enemies.json"))    //загрузка сохраненных врагов из файла, если есть
             {
                 string jsonPlayer = File.ReadAllText("player.json");
                 Player = JsonSerializer.Deserialize<Player>(jsonPlayer);

                 string jsonEnviroment = File.ReadAllText("enviroment.json");
                 EnviromentGrid = JsonSerializer.Deserialize<List<List<Entity?>>>(jsonEnviroment);
                 foreach (var list in EnviromentGrid)
                 {
                     foreach (var item in list)
                     {
                         window.Entities.Add(item);
                     }
                 }

                 string jsonEnemies = File.ReadAllText("enemies.json");
                 foreach (Enemy item in JsonSerializer.Deserialize<List<Enemy>>(jsonEnemies))
                 {
                     Enemies.Add(item);
                 }
                 foreach (Enemy enemy in Enemies)
                 {
                     window.Entities.Add(enemy);
                 }

                 Player.ExitReached += createLevel;



             }
             else {
                 Player = new Player();
                 window.Entities.Add(Player);
                 Player.ExitReached += createLevel;

                 createLevel(); }
            */
        }

        public static void createLevel()        //генерация уровня
        {
            Player.X = 800;
            Player.Y = 825;

            lock (window.EntityLock) {
                window.Entities.Clear();
                window.Entities.Add(Player);
            }

            Game.Items.Clear();

            EnviromentGrid = new();
            for (int i = 0; i < 18; i++)
            {
                EnviromentGrid.Add(new List<Entity?>());
                for (int j = 0; j < 32; j++)
                {
                    EnviromentGrid[i].Add(null);
                }
            }

            for (int i = 0; i < 32; i++)
            {
                if (i == 16 || i == 15) { continue; }
                addEnviromentObject(typeof(Wall), "Wall", i, 0);
            }
            addEnviromentObject(typeof(Door), "Door", 16, 0);
            addEnviromentObject(typeof(Door), "Door", 15, 0);
            for (int i = 0; i < 32; i++)
            {
                if (i == 16 || i == 15) { continue; }
                addEnviromentObject(typeof(Wall), "Wall", i, 17);
            }
            addEnviromentObject(typeof(Door), "Door", 16, 17);
            addEnviromentObject(typeof(Door), "Door", 15, 17);
            for (int i = 1; i < 17; i++)
            {
                addEnviromentObject(typeof(Wall), "Wall", 0, i);
            }
            for (int i = 1; i < 17; i++)
            {
                addEnviromentObject(typeof(Wall), "Wall", 31, i);
            }

            Type[] enviromentTypes = { typeof(Box), typeof(Barrel), typeof(Pot), typeof(Rock) };

            Random rnd = new Random();
            for(int i = 0; i < 30; i++)
            {
                int y = rnd.Next(11)+1;
                int x = rnd.Next(30)+1;
                if (x == 15) { x -= 1; }
                if (x == 16) { x += 1; }
                for (int j = 0; j <rnd.Next(5)+1; j++)
                {
                    addEnviromentObject(enviromentTypes[rnd.Next(4)], "RandomEnviroment", x, y+j);
                }
            }

            Type[] weakEnemies = { typeof(Bat), typeof(Bomb), typeof(Cyclone) };
            Type[] strongEnemies = { typeof(Bonfire), typeof(Cloud), typeof(Snowman), typeof(LavaSlime) };

            for (int i = 0; i < Level * 3; i++)
            {
                int x = rnd.Next(28) + 1;
                while(x>14 && x < 18) { x = rnd.Next(28) + 1; }
                int y = rnd.Next(13) + 1;
                while (EnviromentGrid[y][x]!=null)
                {
                    x = rnd.Next(28) + 1;
                    while (x > 14 && x < 18) { x = rnd.Next(28) + 1; }
                    y = rnd.Next(13) + 1;
                }

                AddEnemy(weakEnemies[rnd.Next(3)], "Enemy" + i, x*50, y*50, 10+Level*5) ;
            }
            for (int i = 0; i < Level; i++)
            {
                int x = rnd.Next(28) + 2;
                while (x > 14 && x < 18) { x = rnd.Next(30) + 1; }
                int y = rnd.Next(13) + 2;
                while (EnviromentGrid[y][x] != null)
                {
                    x = rnd.Next(28) + 2;
                    while (x > 14 && x < 18) { x = rnd.Next(28) + 1; }
                    x = rnd.Next(13) + 2;
                }
                AddEnemy(strongEnemies[rnd.Next(4)], "Enemy" + i, x * 50+25, y * 50+25, 10 + Level * 5);
            }


        }

        public static void RemoveEntity(Entity entity)      //общая функция для уничтожения промежуточных сущностей без дополнительной логики
        {
            lock (window.EntityLock) { window.Entities.Remove(entity); }
           
        }


        private static Entity addEnviromentObject(Type type, string name = "NoName", int x = 0, int y = 0, int size = 50, double hp = 50)
        {
            object[] constructorParameters = new object[] { size / 2 + x * size, size / 2 + y * size, name, size, hp };
            Entity obj = (Entity)Activator.CreateInstance(type, constructorParameters);
            if (EnviromentGrid[y][x] != null) { RemoveEnviroment(EnviromentGrid[y][x]); }
            EnviromentGrid[y][x] = obj;
            lock (window.EntityLock) { window.Entities.Add(obj); }
            return obj;
        }

        public static Enemy AddEnemy(Type type,string name = "NoName", int x = 0, int y = 0, double damage = 100)      //добавить хп 
        {
             object[] constructorParameters = new object[] { name, x, y, damage };
            Enemy enemy = (Enemy) Activator.CreateInstance(type, constructorParameters);
            lock(EnemyLock) { Enemies.Add(enemy); } 
            lock (window.EntityLock) { window.Entities.Add(enemy); }
            return enemy;
        }

        public static PickableItemEntity AddItem(Item item, int x = 800, int y = 800)
        {
            PickableItemEntity entity = new PickableItemEntity(item, x,y);
            Items.Add(entity);
            lock (window.EntityLock) { window.Entities.Add(entity); }
            return entity;
        }

        public static WeaponEntity AddWeaponEntity(Weapon weapon)
        {
            WeaponEntity product = new WeaponEntity();
            if(weapon is AreaWeapon)
            {
                product = new AreaWeaponEntity((AreaWeapon)weapon);
            }

            if (weapon is ProjectileWeapon)
            {
                product = new ProjectileWeaponEntity((ProjectileWeapon)weapon);
            }

            window.Entities.Add(product);
            return product;
        }


        public static void RemoveEnviroment(Entity enviroment)
        {
            lock (window.EntityLock) { window.Entities.Remove(enviroment); }
            for(int i = 0; i < EnviromentGrid.Count; i++)
            {
                for (int j = 0; j < EnviromentGrid[i].Count; j++)
                {
                    if (EnviromentGrid[i][j] == enviroment)
                    {
                        EnviromentGrid[i][j] = null;
                        break;
                    }
                }
            }
        }

        public static void RemoveEnemy(Enemy enemy)
        {
            lock (EnemyLock)
            {
                Enemies.Remove(enemy);
            }
            lock (window.EntityLock) { 
                window.Entities.Remove(enemy);
                if (Enemies.Count == 0)
                {
                    RemoveEnviroment(EnviromentGrid[0][16]);
                    RemoveEnviroment(EnviromentGrid[0][15]);

                    Level++;
                    Random rnd = new Random();
                    List<Item> randItems = new List<Item>() {
                    new ProjectileWeapon(100 + Level * 20, 60 + Level * 20, 0.8 + Level * 0.1, 10, 3+(double)rnd.Next(71) /10.0),
                    new AreaWeapon(50 + Level * 20, 300 + Level * 20, 1.5 + Level * 0.1, 0+(double)rnd.Next(51) /10.0),
                    new Armor(10 - 8 * Level/(Level+10), 0.8 + 0.2*(Level)/(Level+200)),
                    new ConsumableHeal(Math.Min(10*Level,100),0)
                };
                    AddItem(randItems[rnd.Next(4)], 800, 600);
                }
            }

            
        }

        public static void RemovePickup()
        {
            foreach(PickableItemEntity item in Items)
            {
                if (Math.Sqrt(Math.Pow(Player.Y - item.Y, 2) + Math.Pow(Player.X - item.X, 2)) <= Player.Size / 2 + item.Size / 2 )       //игрок касается
                {
                    Items.Remove(item);
                    lock (window.EntityLock)
                    {
                        window.Entities.Remove(item);
                    }
                    break;
                }
            }
        }

        public static void PlayerDeath()
        {
            window.Window_Close();
        }

        private static void windowRefresh(object sender, EventArgs e)
        {
            window.Invalidate();
        }
        
    }
}
