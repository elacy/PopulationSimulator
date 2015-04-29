using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic.BeeSim
{
    public class NectarCarrierBehaviour:Behaviour
    {
        protected override void OnAttached(SimObject simObject)
        {
            simObject.Properties.Add(new NectarProperty());
        }

        protected override void OnSimObjectUpdating(SimObject simObject, SimModel simModel, SimState simState)
        {
            var energy = simObject.GetProperty<EnergyProperty>();
            if (energy == null)
            {
                return;
            }
            var nectar = simObject.GetProperty<NectarProperty>();
            if (nectar == null || !nectar.HasNectar)
            {
                energy.AddEnergy(1 * simState.MillisecondsSinceLastUpdate / 1000.0);
            }
            else
            {
                var magnitude = simObject.Velocity.VectorMagnitude();
                energy.UseEnergy(magnitude * simState.MillisecondsSinceLastUpdate / 1000.0);
            }
        }

        protected override void OnSimObjectCollision(SimObject sender, SimModel simModel, SimState simState, List<SimObject> collidingObjects)
        {
            var myNectar = sender.GetProperty<NectarProperty>();
            if (myNectar != null && !myNectar.HasNectar)
            {
                foreach (var collidingObject in collidingObjects)
                {
                    var theirNectar = collidingObject.GetProperty<NectarProperty>();
                    if (myNectar.GetNectarFrom(theirNectar))
                    {
                        return;
                    }
                }
            }
            
        }
    }
}
