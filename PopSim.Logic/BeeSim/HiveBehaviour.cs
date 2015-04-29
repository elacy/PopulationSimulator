using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic.BeeSim
{
    public class HiveBehaviour:Behaviour
    {
        private readonly Random _random;

        public HiveBehaviour(Random random)
        {
            _random = random;
        }

        private List<SimObject> Bees { get; set; }

        private Vector2 SpawnLocation { get; set; }

        protected override void OnAttached(SimObject simObject)
        {
            Bees = new List<SimObject>();
            SpawnLocation = simObject.Location.Add(new Vector2(simObject.Size.Width/2, simObject.Size.Height + 5));
        }

        protected override void OnSimObjectUpdating(SimObject simObject, SimModel simModel, SimState simState)
        {
            if (Bees.Count < 30 && Bees.Count < simState.MillisecondsSinceSimStart / 1000.0)
            {
                var bee = new Actor(SpawnLocation);
                if (simModel.DetectCollisions(bee).Any())
                {
                    return;
                }
                bee.Behaviours.Add(new BeeBehaviour(_random,simObject));
                Bees.Add(bee);
                simModel.SimObjects.Add(bee);
            }
        }
    }
}
