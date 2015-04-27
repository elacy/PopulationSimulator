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

        const double SlowSpeed = 0.1;
        const double Speed = 0.3;

        const double HuntingSpeed = 0.7;
        private const double ChaseDistance = 50;

        protected override void OnAttached(SimObject simObject)
        {
            GiveRandomDirection(simObject,Speed);
            simObject.Color = Colors.Red;
        }

        protected override void OnSimObjectUpdating(SimObject zombie, GameState gameState, long elapsedMilliseconds)
        {
            base.OnSimObjectUpdating(zombie, gameState, elapsedMilliseconds);

            if (Prey != null && (!CanBeVictim(Prey) || zombie.Location.GetDistance(Prey.Location) > ChaseDistance))
            {
                if (CanBeVictim(Prey))
                {
                    Prey.Color = Colors.Black;
                }
                Prey = null;
                GiveRandomDirection(zombie,Speed);
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
            if (Energy < 3)
            {
                zombie.Velocity = new Vector2(0,0);
                zombie.Color = Colors.Indigo;
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
                        Energy = Math.Max(Energy + 4, MaxEnergy);
                    }
                }
            }
            GiveRandomDirection((Actor)sender, Speed);
        }
    }
}
