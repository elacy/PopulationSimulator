using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic.BeeSim
{
    public class FlowerBehaviour:Behaviour
    {
        protected override void OnSimObjectUpdating(SimObject simObject, SimModel simModel, SimState simState)
        {
            var nectarProp = simObject.GetProperty<NectarProperty>();
            if (nectarProp == null)
            {
                nectarProp = new NectarProperty();
                simObject.Properties.Add(nectarProp);
            }
            nectarProp.FillNectar();
        }
    }
}
