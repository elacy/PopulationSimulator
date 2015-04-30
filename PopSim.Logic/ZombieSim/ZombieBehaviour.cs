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
            simObject.RegisterPropertyUpdateAction<EnergyProperty>(EnergyUpdated);
        }

        private double _speed = Speed;

        private void UpdateSpeed(double speed, SimObject simObject)
        {
            simObject.Velocity = simObject.Velocity.UnitVector().ScalarMultiply(speed);
        }

        private void SetDestination(Vector2 vector2, SimObject simObject)
        {
            var speed = simObject.Velocity.VectorMagnitude();
            simObject.Velocity = simObject.Location.GetDirection(vector2).ScalarMultiply(speed);
        }


        private void EnergyUpdated(EnergyProperty prop, SimObject simObject)
        {
            if (prop.Energy < 15)
            {
                UpdateSpeed(SlowSpeed, simObject);
                simObject.Color = Colors.Indigo;
            }
            else if (prop.Energy <= 0)
            {
                UpdateSpeed(0, simObject);
                simObject.Color = Colors.Black;
            }
            else
            {
                UpdateSpeed(Speed, simObject);
                simObject.Color = Colors.Red;
            }
        }


        protected override void OnSimObjectUpdating(SimObject simObject, SimModel simModel, SimState simState)
        {
            if (Prey != null && (!CanBeVictim(Prey) || simObject.Location.GetDistance(Prey.Location) > ViewDistance))
            {
                if (CanBeVictim(Prey))
                {
                    Prey.Color = Colors.Black;
                }
                Prey = null;
                GiveRandomDirection(simObject, simObject.Velocity.VectorMagnitude());
            }
            if (Prey == null)
            {
                Prey = GetObjectsInView(simModel, simObject)
                    .Where(CanBeVictim)
                    .FirstOrDefault();
            }
            if (Prey != null)
            {
                Prey.Color = Colors.Green;
                SetDestination(Prey.Location,simObject);
                var energy = simObject.GetProperty<EnergyProperty>();
                if (energy != null && energy.Energy > 15)
                {
                    UpdateSpeed(RunSpeed,simObject);
                }
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
                        var energyProp = sender.GetProperty<EnergyProperty>();
                        energyProp.AddEnergy(10);
                    }
                }
            }
            GiveRandomDirection((Actor)sender, Speed);
        }
    }
}
