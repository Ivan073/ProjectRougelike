using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Items
{
    public class ConsumableHeal : Consumable
    {
        public new static Image? Sprite = Image.FromFile("Images/consumable.png");
        public double Heal { get; set; }
        
        public ConsumableHeal(double heal,double cost)
        {
            Heal = heal;
            Cost = cost;
        }
        public override void Use()
        {
            if (Game.Player.Health!=Game.Player.MaxHealth && Game.Player.UseMana(Cost))
            {
                Game.Player.Consumable = null;
                Game.Player.IncreaseHealth(Heal);
            }
        }
    }
}
