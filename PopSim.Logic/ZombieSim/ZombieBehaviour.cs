using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace PopSim.Logic.ZombieSim
{
    public class ZombieBehaviour : HumanBehaviour
    {
        public ZombieBehaviour(Random random)
            : base(random)
        {
        }

        private SimObject Prey { get; set; }

        protected override void OnAttached(SimObject simObject)
        {
            GiveRandomDirection(simObject, Speed);
            simObject.Color = Colors.Red;
        }


        protected override void OnSimObjectUpdating(SimObject zombie, SimModel simModel, SimState simState)
        {
            var magnitude = zombie.Velocity.VectorMagnitude();
            Energy -= magnitude * simState.MillisecondsSinceLastUpdate / 1000.0;

            if (Prey != null && (!CanBeVictim(Prey) || zombie.Location.GetDistance(Prey.Location) > ViewDistance))
            {
                if (CanBeVictim(Prey))
                {
                    Prey.Color = Colors.Black;
                }
                Prey = null;
                GiveRandomDirection(zombie, Speed);
            }
            if (Prey == null)
            {
                Prey = GetObjectsInView(simModel,zombie)
                    .Where(CanBeVictim)
                    .FirstOrDefault();
            }
            if (Prey != null)
            {
                Prey.Color = Colors.Green;
                zombie.Velocity = zombie.Location.GetDirection(Prey.Location).ScalarMultiply(RunSpeed);
            }
            if (Energy < 15)
            {
                zombie.Velocity = zombie.Velocity.UnitVector().ScalarMultiply(SlowSpeed);
                zombie.Color = Colors.Indigo;
            }
            else if (Energy <= 0)
            {
                zombie.Velocity = new Vector2(0, 0);
                zombie.Color = Colors.Black;
            }
            else
            {
                zombie.Color = Colors.Red;
            }
        }

        private static bool CanBeVictim(SimObject simObject)
        {
            return simObject.Behaviours.Any(x => !(x is ZombieBehaviour));
        }

        protected override void OnSimObjectCollision(SimObject sender, SimModel simModel, SimState simState, List<SimObject> collidingObjects)
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
                        Energy = Math.Max(Energy + 10, MaxEnergy);
                    }
                }
            }
            GiveRandomDirection((Actor)sender, Speed);
        }
    }
}
