using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design.Behavior;
using Project.GameClasses.Enemies;

namespace Project.GameClasses.Strategies
{
    public abstract class Strategy
    {
        protected Enemy owner;
        public System.Threading.Timer? BehaveTimer = null;
        public Strategy(Enemy enemy)
        {
            owner = enemy;
        }
        public abstract void Behave();

       

    }
}
