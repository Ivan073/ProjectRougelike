using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.EnviromentObjects.SquareEnviromentObjects
{
    internal class Box:SquareEnviromentObject
    {
        public new static Image? Sprite = Image.FromFile("Images/box.png");

        public Box(int x, int y, string name, int size, double health) : base(x, y, name, size, health)
        {
        }
    }
}
