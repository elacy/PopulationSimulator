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
            GiveRandomDirection(actor,Speed);
        }
        
        protected void GiveRandomDirection(Actor actor, double speed)
        {
            var angle = Math.PI*2*Random.NextDouble();
            var velocity = new Vector2(angle);
            actor.Velocity = velocity.ScalarMultiply(speed);
        }

        protected override void OnSimObjectCollision(SimObject sender, GameState gameState, long elapsedMilliseconds, List<SimObject> collidingObjects)
        {
            GiveRandomDirection((Actor)sender, Speed);
        }
    }
}
