using System;
using System.Collections.Generic;
using System.Linq;

namespace PopSim.Logic.ZombieSim
{
    public class HumanBehaviour:Behaviour
    {
        protected readonly Random Random;

        public HumanBehaviour(Random random)
        {
            Random = random;
            Energy = MaxEnergy;
        }

        protected const double ViewDistance = 50;
        protected const double MaxEnergy = 20;
        protected double Energy { get; set; }

        public Random RandomNumberGenerator { get; set; }

        protected const double SlowSpeed = 0.3;
        protected const double Speed = 0.3;
        protected const double RunSpeed = 0.7;

        protected IEnumerable<SimObject> GetObjectsInView(SimModel simModel, SimObject simObject)
        {
            return simModel.SimObjects
                .Select(sim => new {sim, Distance = simObject.Location.GetDistance(sim.Location)})
                .Where(x => x.Distance < ViewDistance)
                .OrderBy(x => x.Distance)
                .Select(x => x.sim);
        }

        protected override void OnSimObjectUpdating(SimObject simObject, SimModel simModel, SimState simState)
        {
            //Energy += Math.Max(MaxEnergy,elapsedMilliseconds/1000.0 / 5);
            //var magnitude = simObject.Velocity.VectorMagnitude();
            //if (magnitude > Speed)
            //{
            //    Energy -= magnitude * elapsedMilliseconds / 1000.0;
            //}
            //var runningObject = GetObjectsInView(gameState, simObject)
            //    .FirstOrDefault(x => x.Velocity.VectorMagnitude() > Speed);

            //if (runningObject == null || Energy < 10)
            //{
            //    simObject.Velocity = simObject.Velocity.UnitVector().ScalarMultiply(Speed);
            //}
            //else
            //{
            //    simObject.Velocity = runningObject.Velocity.UnitVector().ScalarMultiply(RunSpeed);
            //}
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

        protected override void OnSimObjectCollision(SimObject sender, SimModel simModel, SimState simState, List<SimObject> collidingObjects)
        {
            GiveRandomDirection(sender, Speed);
        }
    }
}
