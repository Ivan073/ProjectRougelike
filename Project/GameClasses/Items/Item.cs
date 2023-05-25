using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.Items
{
    public abstract class Item
    {
        public double Cost { get; set; }
        public abstract void Use();
    }
}
