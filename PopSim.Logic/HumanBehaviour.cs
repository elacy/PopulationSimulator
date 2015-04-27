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
            Energy = MaxEnergy;
        }

        protected const double MaxEnergy = 10;
        protected double Energy { get; set; }

        public Random RandomNumberGenerator { get; set; }

        const double Speed = 0.3;

        protected override void OnSimObjectUpdating(SimObject simObject, GameState gameState, long elapsedMilliseconds)
        {
            var magnitude = simObject.Velocity.VectorMagnitude();
            if (magnitude > Speed)
            {
                Energy -= elapsedMilliseconds / 1000.0;
            }
        }

        protected override void OnAttached(SimObject simObject)
        {
            var actor = (Actor)simObject;
            GiveRandomDirection(actor,Speed);
        }
        
        protected void GiveRandomDirection(SimObject actor, double speed)
        {
            var angle = Math.PI*2*Random.NextDouble();
            var velocity = new Vector2(angle);
            actor.Velocity = velocity.ScalarMultiply(speed);
        }

        protected override void OnSimObjectCollision(SimObject sender, GameState gameState, long elapsedMilliseconds, List<SimObject> collidingObjects)
        {
            GiveRandomDirection(sender, Speed);
        }
    }
}
