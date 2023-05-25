using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.EnviromentObjects.SquareEnviromentObjects
{
    
    internal class Wall: SquareEnviromentObject
    {
        public new static Image? Sprite = Image.FromFile("Images/wall.png");

        public Wall(int x, int y, string name, int size, double health) : base(x,y,name,size,health)
        {
        }
    }
}
