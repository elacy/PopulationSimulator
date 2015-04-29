namespace PopSim.Logic
{
    public class GameStateUpdateEventArgs
    {
        public SimModel SimModel { get; private set; }
        public SimState SimState { get; private  set; }

        public GameStateUpdateEventArgs(SimModel simModel, SimState simState)
        {
            SimModel = simModel;
            SimState = simState;
        }
    }
}