using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PopSim.Logic
{
    public class HumanBehaviour:Behaviour
    {
        protected readonly Random Random;

        public HumanBehaviour(Random random)
        {
            Random = random;
        }

        public Random RandomNumberGenerator { get; set; }

        const double Speed = 0.3;

        protected override void OnAttached(SimObject simObject)
        {
            var actor = (Actor)simObject;
            GiveRandomDirection(actor);
        }
        
        protected void GiveRandomDirection(Actor actor)
        {
            double x = Speed, y = Speed;
            if (RandomBool())
            {
                x = -x;
            }
            if (RandomBool())
            {
                y = -y;
            }
            actor.Velocity = new Vector2(x,y);
        }

        private bool RandomBool()
        {
            return Random.Next(1, 1000) % 2 == 0;
        }

        protected override void OnSimObjectCollision(SimObject sender, GameState gameState, long elapsedMilliseconds, List<SimObject> collidingObjects)
        {
            GiveRandomDirection((Actor)sender);
        }
    }
}
