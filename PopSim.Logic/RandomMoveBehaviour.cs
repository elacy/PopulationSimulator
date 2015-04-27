using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic
{
    public class RandomMoveBehaviour:Behaviour
    {
        private readonly Random _random;

        public RandomMoveBehaviour(Random random)
        {
            _random = random;
        }

        public Random RandomNumberGenerator { get; set; }

        const double Speed = 0.2;
        protected override void OnSimObjectUpdating(SimObject simObject, GameState gameState, long elapsedMilliseconds)
        {
            var actor = (Actor)simObject;
            if(Math.Abs(Math.Abs(actor.Velocity.X) - Speed) > double.Epsilon || Math.Abs(Math.Abs(actor.Velocity.Y) - Speed) > double.Epsilon)
            {
                GiveRandomDirection(actor);
            }
        }

        private void GiveRandomDirection(Actor actor)
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
            return _random.Next(1, 1000) % 2 == 0;
        }

        protected override void OnSimObjectCollision(SimObject sender, GameState gameState, long elapsedMilliseconds, List<SimObject> collidingObjects)
        {
            GiveRandomDirection((Actor)sender);
        }
    }
}
