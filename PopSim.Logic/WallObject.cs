using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PopSim.Logic
{
    public class WallObject:SimObject
    {
        public WallObject(Vector2 location, Size2 size)
        {
            Location = location;
            Size = size;
            Color = Colors.Black;
        }

        public override bool CanHaveCollisions
        {
            get { return true; }
        }

        protected override void OnUpdate(GameState gameState, long elapsedMilliseconds)
        {
            
        }
    }
}
