using System.Collections.Generic;

namespace PopSim.Logic
{
    public abstract class Behaviour
    {
        public void Attach(SimObject simObject)
        {
            simObject.Updating += SimObjectOnUpdating;
            simObject.Collision += SimObjectOnCollision;
            OnAttached(simObject);
        }

        protected virtual void OnAttached(SimObject simObject)
        {
            
        }


        private void SimObjectOnCollision(object sender, GameCollisionEventArgs e)
        {
            OnSimObjectCollision((SimObject)sender,e.SimModel,e.SimState,e.CollidingObjects);
        }

        protected virtual void OnSimObjectCollision(SimObject sender, SimModel simModel, SimState simState, List<SimObject> collidingObjects)
        {
            
        }

        private void SimObjectOnUpdating(object sender, GameStateUpdateEventArgs e)
        {
            OnSimObjectUpdating((SimObject)sender, e.SimModel, e.SimState);
        }

        protected virtual void OnSimObjectUpdating(SimObject simObject, SimModel simModel, SimState simState)
        {
            
        }

        public void Detach(SimObject simObject)
        {
            simObject.Updating -= SimObjectOnUpdating;
        }
    }
}