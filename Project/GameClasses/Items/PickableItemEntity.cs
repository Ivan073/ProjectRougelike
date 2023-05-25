using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Items
{
    public class PickableItemEntity : Entity
    {
        public new static Image? Sprite = Image.FromFile("Images/bag.png");

        public Item AssociatedItem; 
        public PickableItemEntity(Item item, int x, int y) {

            X = x;
            Y = y;
            AssociatedItem = item;
            Size = 30;
            //Name = Item.Name
            Name = item.GetType() + "Pickup";
        }
    }
}
