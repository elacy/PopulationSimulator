using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PopSim.Logic
{
    public class ZombieBehaviour : HumanBehaviour
    {

        public ZombieBehaviour(Random random):base(random)
        {
        }

        private SimObject Prey { get; set; }

        const double Speed = 0.4;

        const double HuntingSpeed = 1;
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
                zombie.Velocity = zombie.Location.GetDirection(Prey.Location).ScalarMultiply(HuntingSpeed);
            }
        }

        private static bool CanBeVictim(SimObject simObject)
        {
            return simObject.Behaviours.Any(x => !(x is ZombieBehaviour));
        }



        protected override void OnSimObjectCollision(SimObject sender, GameState gameState, long elapsedMilliseconds, List<SimObject> collidingObjects)
        {
            foreach (var collidingObject in collidingObjects)
            {
                foreach (var behaviour in collidingObject.Behaviours.ToList())
                {
                    if (!(behaviour is ZombieBehaviour))
                    {
                        collidingObject.Behaviours.Remove(behaviour);
                        collidingObject.Behaviours.Add(new ZombieBehaviour(Random));
                        Prey = null;
                        GiveRandomDirection((Actor)sender);
                    }
                }
            }
        }
    }
}
