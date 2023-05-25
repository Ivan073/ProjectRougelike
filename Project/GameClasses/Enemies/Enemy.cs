using Project.GameClasses.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project.GameClasses.Enemies
{

    public class Enemy : Damageable
    {
        public double Damage { get; set; } = 100;
        protected Strategy Behaviour;

        //public new static Image? Sprite = Image.FromFile("Images/droplet.png");
        public new static Image? Sprite = null;

        public Enemy()
        {
            X = 0; Y = 0;
            health = 100;
            maxHealth = 100;
            Size = 50;
            Name = "NoName";
            Behaviour = new Charger(this);
        }
        public Enemy(string name, int x, int y, double damage)
        {
            X = x; Y = y;
            health = 100;
            maxHealth = 100;
            Size = 50;
            Name = name;
            //Behaviour = new Charger(this);
            //Behaviour = new DestroyerCharger(this);
            Damage = damage;
        }

        protected override void destruction()
        {
            lock (Game.EnemyLock)
            {
                Game.RemoveEnemy(this);
            }
            Behaviour?.BehaveTimer?.Dispose();
        }

    }
}
