using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic
{
    public class CollisionDetection
    {
        public bool IsCollision(SimObject first, SimObject second)
        {
            if (!first.CanHaveCollisions || !second.CanHaveCollisions)
            {
                return false;
            }
            var xOverlap = IsOverlap(first.Location.X, first.Location.X + first.Size.Width, second.Location.X, second.Location.X + second.Size.Width);
            var yOverlap = IsOverlap(first.Location.Y, first.Location.Y + first.Size.Height, second.Location.Y, second.Location.Y + second.Size.Height);
            return xOverlap && yOverlap;
        }

        private static bool IsOverlap(double firstStart, double firstEnd, double secondStart, double secondEnd)
        {
            if (firstStart < secondStart)
            {
                if (firstEnd > secondStart)
                {
                    return true;
                }
            }
            else
            {
                if (secondEnd > firstStart)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
