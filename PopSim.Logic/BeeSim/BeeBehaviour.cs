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

        private const double MaxTransportDistance = 50;

        private const double MaxViewDistance = 50;

        private Vector2 NectarPickupLocation { get; set; }
        private Vector2 NectarDropLocation { get; set; }

        private bool _nectar = false;
        private bool _dancing = false;

        private readonly object _nectarLock = new object();

        public bool GetNectar()
        {
            lock (_nectarLock)
            {
                if (!_nectar) 
                    return false;

                _nectar = false;
                _dancing = false;
                return true;
            }
        }

        private void NectarReceived(SimObject simObject)
        {
            lock (_nectarLock)
            {
                if (!_nectar)
                {
                    NectarPickupLocation = simObject.Location;
                    simObject.Velocity = simObject.Location.GetDirection(_hive.Location).ScalarMultiply(Speed);
                    simObject.Color = Colors.Green;
                    _nectar = true;
                }
            }
        }

        protected override void OnAttached(SimObject simObject)
        {
            simObject.Color = Colors.Black;
            GiveRandomDirection(simObject,Speed);
        }

        protected override void OnSimObjectUpdating(SimObject simObject, SimModel simModel, SimState simState)
        {
            if (_dancing)
            {
                simObject.Color = Colors.Blue;
            }
            else if (_nectar)
            {
                if (simObject.Location.GetDistance(NectarPickupLocation) > MaxTransportDistance)
                {
                    _dancing = true;
                    simObject.Velocity = new Vector2(0,0);
                }
            }
            else
            {
                simObject.Color = Colors.Black;
                var dancingBee = simModel.SimObjects.OfType<Actor>()
                    .FirstOrDefault(x => x.Color == Colors.Blue && x.Location.GetDistance(simObject.Location) < MaxViewDistance);
                if (dancingBee != null)
                {
                    simObject.Velocity = simObject.Location.GetDirection(dancingBee.Location).ScalarMultiply(Speed);
                }
                if (NectarPickupLocation != null)
                {
                    if (NectarPickupLocation.GetDistance(simObject.Location) < 1)
                    {
                        simObject.Velocity = new Vector2(0, 0);
                    }
                    else
                    {
                        simObject.Velocity = simObject.Location.GetDirection(NectarPickupLocation).ScalarMultiply(Speed);
                    }
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
            var flower = collidingObjects.OfType<Flower>().FirstOrDefault();
            if (flower != null)
            {
                NectarReceived(sender);
            }
            else
            {
                var bees = collidingObjects.OfType<Actor>().SelectMany(x => x.Behaviours.OfType<BeeBehaviour>());
                if (bees.Any(bee => bee.GetNectar()))
                {
                    NectarReceived(sender);
                }
            }
            

            GiveRandomDirection(sender, Speed);
        }
    }
}
