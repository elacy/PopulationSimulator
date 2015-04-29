using System.Collections.Generic;

namespace PopSim.Logic
{
    public class GameCollisionEventArgs
    {
        public SimModel SimModel { get; private set; }
        public SimState SimState { get; private set; }
        public List<SimObject> CollidingObjects { get; private set; }

        public GameCollisionEventArgs(SimModel simModel, SimState simState, List<SimObject> collidingObjects )
        {
            SimModel = simModel;
            SimState = simState;
            CollidingObjects = collidingObjects;
        }
    }
}