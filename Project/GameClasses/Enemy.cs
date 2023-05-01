using Project.GameClasses.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses
{

    internal class Enemy : Damageable
    {
        public double Damage { get; set; } = 100;
        private Strategy behaviour;
        private System.Threading.Timer? behaveTimer = null;
        public Enemy(string name,int x, int y)
        {
            X = x; Y = y;
            health = 100;
            maxHealth = 100;
            Size = 50;
            Name = name;
            behaviour = new Charger(this);

            behaveTimer = new System.Threading.Timer(new TimerCallback((s) =>
            {
                behaviour.Behave();
            }), null, 0, 5);
        }

        protected override void destruction()
        {
            lock (Game.EnemyLock)
            {
                Game.RemoveEnemy(this);
            }
            behaveTimer?.Dispose();
        }

    }
}
