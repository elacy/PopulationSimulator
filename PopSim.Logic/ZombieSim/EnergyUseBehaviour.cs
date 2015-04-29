using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic.ZombieSim
{
    public class EnergyUseBehaviour:Behaviour
    {
        protected override void OnSimObjectUpdating(SimObject simObject, SimModel simModel, SimState simState)
        {
            var energy = simObject.GetProperty<EnergyProperty>();
            if (energy== null)
            {
                return;
            }
            var magnitude = simObject.Velocity.VectorMagnitude();
            energy.UseEnergy(magnitude * simState.MillisecondsSinceLastUpdate / 1000.0);
        }
    }
}
