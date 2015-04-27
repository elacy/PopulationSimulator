using System.Collections.Generic;

namespace PopSim.Logic
{
    public class GameStateUpdateEventArgs
    {
        public GameState GameState { get; private set; }
        public long ElapsedMilliseconds { get; private set; }

        public GameStateUpdateEventArgs(GameState gameState, long elapsedMilliseconds)
        {
            GameState = gameState;
            ElapsedMilliseconds = elapsedMilliseconds;
        }
    }
    public class GameCollisionEventArgs
    {
        public GameState GameState { get; private set; }
        public long ElapsedMilliseconds { get; private set; }
        public List<SimObject> CollidingObjects { get; private set; }

        public GameCollisionEventArgs(GameState gameState, long elapsedMilliseconds, List<SimObject> collidingObjects )
        {
            GameState = gameState;
            ElapsedMilliseconds = elapsedMilliseconds;
            CollidingObjects = collidingObjects;
        }
    }
}