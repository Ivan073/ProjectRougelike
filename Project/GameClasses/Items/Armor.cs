using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Items
{
    internal class Armor: Item
    {
        public double DefencePercent { get; set; } //правильная дробь

        private bool active = false;
        public bool Active { get { return active; } }

        private System.Threading.Timer? manaDrain = null;
        public Armor(double cost, double defencePercent) {
            active = false;
            Cost = cost;
            DefencePercent = defencePercent;
        }
        public override void Use()
        {
           if(manaDrain == null) {
                manaDrain = new System.Threading.Timer(new TimerCallback((s) =>
                {
                    Game.Player?.UseMana(Cost / 10);
                }), null, 0, 100);
                active = true;
            }
            
            else { manaDrain.Dispose();manaDrain = null; active = false; }
        }
    }
}
