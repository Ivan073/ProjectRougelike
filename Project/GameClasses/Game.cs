using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.GameClasses.EnviromentObjects;
using Project.GameClasses.Items;

namespace Project.GameClasses
{
    static public class Game
    {
        private static System.Windows.Forms.Timer refreshTimer = new System.Windows.Forms.Timer();
        private static Window? window = null;

        internal static Player? Player = null;

        internal static List<Enemy> Enemies = new ();
        public static object EnemyLock = new();

        internal static List<List<Entity?>> EnviromentGrid = new(); //32x18, поле 1600x900

        public static int Level = 1;

        public static void SetWindow(Window _window) {
            window = _window;
        }
        public static void Start()
        {
            for(int i = 0; i < 18; i++)
            {
                EnviromentGrid.Add(new List<Entity?>());
                for (int j = 0; j < 32; j++)
                {
                    EnviromentGrid[i].Add(null);
                }
            }
            createEntities();

            refreshTimer.Interval = 10;
            refreshTimer.Tick += windowRefresh;
            refreshTimer.Start();
        }

        private static void createEntities()   //пока что промежуточный метод для добавления сущностей на экран
        {
            Player = new Player();
            window.Entities.Add(Player);

            var rock = addRoundEnviromentObject("Rock", 2, 5);
            var rock2 = addRoundEnviromentObject("Rock2", 3, 6);
            var rock3 = addRoundEnviromentObject("Rock3", 7, 2);
            var rock4 = addRoundEnviromentObject("Rock4", 29, 2);

            var pot = addRoundEnviromentObject("Pot", 10, 10);

            for(int i = 0; i < 32; i++)
            {
                addSquareEnviromentObject("Wall", i, 0);
            }
            for (int i = 0; i < 32; i++)
            {
                addSquareEnviromentObject("Wall", i, 17);
            }
            for (int i = 1; i < 17; i++)
            {
                addSquareEnviromentObject("Wall", 0, i);
            }
            for (int i = 1; i < 17; i++)
            {
                addSquareEnviromentObject("Wall", 31, i);
            }

          /*  Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                addEnemy("Enemy"+i, rnd.Next(500), rnd.Next(500));
            }*/

            var testenemy = addEnemy("Enemy1", 500,500);
        }

        public static void RemoveEntity(Entity entity) //общая функция для уничтожения промежуточных сущностей
        {
            lock (window.EntityLock) { window.Entities.Remove(entity); }
           
        }

        //(потом добавить прокидывание класса-типа) в функции-создатели
        private static RoundEnviromentObject addRoundEnviromentObject(string name = "NoName", int x = 0, int y = 0, int size = 50 ,double hp = 100)      //Фабрика круглых объектов окружения
        {
            RoundEnviromentObject obj = new RoundEnviromentObject(size/2+x*size,size/2+y*size,name,size,hp);
            EnviromentGrid[y][x] = obj;
            window.Entities.Add(obj);
            return obj;
        }

        private static SquareEnviromentObject addSquareEnviromentObject(string name = "NoName", int x = 0, int y = 0, int size = 50, double hp = 100)      //Фабрика квадратных объектов окружения
        {
            SquareEnviromentObject obj = new SquareEnviromentObject(size / 2 + x * size, size / 2 + y * size, name, size, hp);
            EnviromentGrid[y][x] = obj;
            window.Entities.Add(obj);
            return obj;
        }

        private static Enemy addEnemy(string name = "NoName", int x = 0, int y = 0)      //Фабрика врагов - пока что дефолтных, но такие не нужны 
        {
            Enemy enemy = new Enemy(name, x, y);
            Enemies.Add(enemy);
            window.Entities.Add(enemy);
            return enemy;
        }

        internal static WeaponEntity AddWeaponEntity(Weapon weapon)
        {
            WeaponEntity product = new WeaponEntity();
            if(weapon is AreaWeapon)
            {
                product = new AreaWeaponEntity((AreaWeapon)weapon);
            }
            window.Entities.Add(product);
            return product;
        }

        internal static void RemoveEnviroment(Entity enviroment)
        {
            foreach(var list in EnviromentGrid)
            {
                list.Remove(enviroment);
            }
            lock (window.EntityLock) { window.Entities.Remove(enviroment); }
        }

        internal static void RemoveEnemy(Enemy enemy)
        {
            lock (EnemyLock)
            {
                Enemies.Remove(enemy);
            }
            lock (window.EntityLock) { window.Entities.Remove(enemy); }
            //временная надстройка для бесконечного геймплея
            if(Enemies.Count == 0)
            {
                Level++;
                Random rnd = new Random();
                for (int i = 0; i < Level * Level; i++)
                {
                    
                    addEnemy("Enemy"+i, rnd.Next(100,600), rnd.Next(100, 600));
                }
            }
        }



        internal static void PlayerDeath()
        {
            window.Window_Close();
        }

        private static void windowRefresh(object sender, EventArgs e)
        {
            window.Invalidate();
        }
        
    }
}
