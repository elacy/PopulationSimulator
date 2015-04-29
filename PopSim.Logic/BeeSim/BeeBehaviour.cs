using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PopSim.Logic.BeeSim
{
    public class BeeBehaviour:Behaviour
    {
        private readonly Random _random;
        private readonly SimObject _hive;

        public BeeBehaviour(Random random, SimObject hive)
        {
            _random = random;
            _hive = hive;
        }

        protected const double Speed = 2;

        private const double MaxViewDistance = 50;

        protected override void OnAttached(SimObject simObject)
        {
            simObject.RegisterPropertyUpdateAction<NectarProperty>(NectarUpdated);
            simObject.RegisterPropertyUpdateAction<EnergyProperty>(EnergyUpdated);
            simObject.Color = Colors.Black;
            GiveRandomDirection(simObject,Speed);
            simObject.Properties.Add(new EnergyProperty(30,30));
        }

        private bool _halt;
        private bool _canHuntNectar;

        private void EnergyUpdated(EnergyProperty prop, SimObject simObject)
        {
            if (prop.Energy < 1)
            {
                simObject.Velocity = new Vector2(0, 0);
                simObject.Color = Colors.Red;
                _halt = true;
                _canHuntNectar = false;
            }
            else if (prop.Energy < 15)
            {
                _canHuntNectar = false;
            }
            else
            {
                _halt = true;
                _canHuntNectar = true;
            }
        }

        private void NectarUpdated(NectarProperty nectarProperty, SimObject simObject)
        {
            if (nectarProperty.HasNectar)
            {
                simObject.Color = Colors.Green;
                simObject.Velocity = simObject.Location.GetDirection(_hive.Location).ScalarMultiply(Speed);
            }
            else if(!nectarProperty.HasNectar)
            {
                GiveRandomDirection(simObject, Speed);
            }
        }

        protected override void OnSimObjectUpdating(SimObject simObject, SimModel simModel, SimState simState)
        {
            if (_halt)
            {
                return;
            }

            var bouncedRecently = simState.MillisecondsSinceSimStart - _lastBounce < 2000;
            if (!bouncedRecently && _canHuntNectar)
            {
                simObject.Color = Colors.Black;
                var closestStaticObjectWithNectar = simModel.SimObjects
                    .Where(x => x.Location.GetDistance(simObject.Location) < MaxViewDistance)
                    .Select(x => new {SimObject = x, Nectar = x.GetProperty<NectarProperty>()})
                    .Where(x => x.Nectar != null && x.Nectar.HasNectar)
                    .Select(x => x.SimObject)
                    .FirstOrDefault();

                if (closestStaticObjectWithNectar != null)
                {
                    simObject.Velocity = simObject.Location.GetDirection(closestStaticObjectWithNectar.Location).ScalarMultiply(Speed);
                }
            }

        }
        protected void GiveRandomDirection(SimObject actor, double speed)
        {
            var angle = Math.PI * 2 * _random.NextDouble();
            var velocity = new Vector2(angle);
            actor.Velocity = velocity.ScalarMultiply(speed);
        }

        protected override void OnSimObjectCollision(SimObject sender, SimModel simModel, SimState simState, List<SimObject> collidingObjects)
        {
            _lastBounce = simState.MillisecondsSinceSimStart;
            GiveRandomDirection(sender, Speed);
        }

        private long _lastBounce = 0;
    }
}
