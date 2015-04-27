using System.Collections.Generic;
using System.Windows.Media;

namespace PopSim.Logic
{
    public class Actor:SimObject
    {
        public Actor(Vector2 initialLocation) 
        {
            Size = new Size2(5,5);
            Location = initialLocation;
            Color = Colors.Blue;
            Velocity = new Vector2(0, 0);
        }

        public override bool CanHaveCollisions
        {
            get { return true; }
        }
    }
}