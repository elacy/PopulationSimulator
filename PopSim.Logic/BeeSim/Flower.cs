using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic.BeeSim
{
    public class Flower:SimObject
    {
        public Flower(Vector2 location, Size2 size)
        {
            Location = location;
            Size = size;
        }
        public override bool CanHaveCollisions
        {
            get { return true; }
        }
    }
}
