using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PopSim.Logic
{
    public class ZombieBehaviour : Behaviour
    {
        private readonly Random _random;

        public ZombieBehaviour(Random random)
        {
            _random = random;
        }

        private SimObject Prey { get; set; }

        const double Speed = 0.2;

        const double HuntingSpeed = 0.5;
        private const double ChaseDistance = 50;

        protected override void OnAttached(SimObject simObject)
        {
            GiveRandomDirection((Actor)simObject);
            simObject.Color = Colors.Red;
        }

        protected override void OnSimObjectUpdating(SimObject simObject, GameState gameState, long elapsedMilliseconds)
        {
            var zombie = (Actor) simObject;
            if (Prey != null && (!CanBeVictim(Prey) || zombie.Location.GetDistance(Prey.Location) > ChaseDistance))
            {
                if (CanBeVictim(Prey))
                {
                    Prey.Color = Colors.Black;
                }
                Prey = null;
                GiveRandomDirection(zombie);
            }
            if (Prey == null)
            {
                Prey = gameState.SimObjects
                    .Where(CanBeVictim)
                    .Select(sim => new { sim, Distance = zombie.Location.GetDistance(sim.Location) })
                    .Where(x => x.Distance < ChaseDistance)
                    .OrderBy(x => x.Distance)
                    .Select(x => x.sim)
                    .FirstOrDefault();
            }
            if (Prey != null)
            {
                Prey.Color = Colors.Green;
                zombie.Velocity = zombie.Location.GetVelocity(Prey.Location, HuntingSpeed);
            }
        }

        private static bool CanBeVictim(SimObject simObject)
        {
            return simObject.Behaviours.Any(x => x is HumanBehaviour);
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
            actor.Velocity = new Vector2(x, y);
        }

        private bool RandomBool()
        {
            return _random.Next(1, 1000) % 2 == 0;
        }

        protected override void OnSimObjectCollision(SimObject sender, GameState gameState, long elapsedMilliseconds, List<SimObject> collidingObjects)
        {
            foreach (var collidingObject in collidingObjects)
            {
                foreach (var behaviour in collidingObject.Behaviours.ToList())
                {
                    if (behaviour is HumanBehaviour)
                    {
                        collidingObject.Behaviours.Remove(behaviour);
                        collidingObject.Behaviours.Add(new ZombieBehaviour(_random));
                        Prey = null;
                        GiveRandomDirection((Actor)sender);
                    }
                }
            }
        }
    }
}
