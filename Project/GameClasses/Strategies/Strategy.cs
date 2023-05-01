using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Strategies
{
    internal abstract class Strategy
    {
        protected Enemy owner;
        public Strategy(Enemy enemy)
        {
            owner = enemy;
        }
        public abstract void Behave();

    }
}
