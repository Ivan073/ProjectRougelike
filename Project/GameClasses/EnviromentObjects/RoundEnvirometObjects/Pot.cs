using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameClasses.EnviromentObjects.RoundEnvirometObjects
{
    internal class Pot : RoundEnviromentObject
    {
        public new static Image? Sprite = Image.FromFile("Images/pot.png");
        public Pot(int x, int y, string name, int size, double health) : base(x, y, name, size, health)
        {
        }
    }
}
