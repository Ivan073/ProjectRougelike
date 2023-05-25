using Project.GameClasses.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Strategies
{
    internal class Spawner : Strategy
    {
        private Type creatable;
        public Spawner(Enemy enemy, Type creatable) : base(enemy)
        {
            this.creatable = creatable;
            owner = enemy;
            BehaveTimer = new System.Threading.Timer(new TimerCallback((s) =>
            {
                Behave();
            }), null, 2000, 1000);
        }

        public override void Behave()
        {
            Game.AddEnemy(creatable,"Droplet",owner.X,owner.Y,owner.Damage);  
        }
    }
}
