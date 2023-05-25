using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.EnviromentObjects.SquareEnviromentObjects
{
    internal class Door : SquareEnviromentObject
    {
        public new static Image? Sprite = Image.FromFile("Images/door.png");

        public Door(int x, int y, string name, int size, double health) : base(x, y, name, size, health)
        {
        }
    }
}
