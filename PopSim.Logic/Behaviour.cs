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
            OnSimObjectCollision((SimObject)sender,e.GameState,e.ElapsedMilliseconds,e.CollidingObjects);
        }

        protected virtual void OnSimObjectCollision(SimObject sender, GameState gameState, long elapsedMilliseconds, List<SimObject> collidingObjects)
        {
            
        }

        private void SimObjectOnUpdating(object sender, GameStateUpdateEventArgs e)
        {
            OnSimObjectUpdating((SimObject)sender, e.GameState, e.ElapsedMilliseconds);
        }

        protected abstract void OnSimObjectUpdating(SimObject simObject, GameState gameState, long elapsedMilliseconds);

        public void Detach(SimObject simObject)
        {
            simObject.Updating -= SimObjectOnUpdating;
        }
    }
}