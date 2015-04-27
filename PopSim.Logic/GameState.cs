using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PopSim.Logic
{
    public class GameState
    {
        private readonly CollisionDetection _collisionDetection;
        private readonly Random _random;
        public double Width { get; set; }
        public double Height { get; set; }
        public ObservableCollection<SimObject> SimObjects { get; set; }

        public GameState(CollisionDetection collisionDetection, Random random)
        {
            _collisionDetection = collisionDetection;
            _random = random;
            SimObjects = new ObservableCollection<SimObject>();
        }

        private const int WallSize = 2;

        public IEnumerable<SimObject> DetectCollisions(SimObject collidingObject)
        {
            foreach (var simObject in SimObjects)
            {
                if (collidingObject != simObject && _collisionDetection.IsCollision(collidingObject, simObject))
                {
                    yield return simObject;
                }
            }
        }

        public void CreateInitialPopulation(double width, double height)
        {
            Width = width;
            Height = height;

            //Added four walls
            SimObjects.Add(new WallObject(new Vector2(0, 0), new Size2(Width, WallSize)));
            SimObjects.Add(new WallObject(new Vector2(0, 0), new Size2(WallSize, Height)));
            SimObjects.Add(new WallObject(new Vector2(Width - WallSize, 0), new Size2(WallSize, Height)));
            SimObjects.Add(new WallObject(new Vector2(0, Height - WallSize), new Size2(Width, WallSize)));

            for (var i = 0; i < 100; i++)
            {
                Actor actor;
                do
                {
                    var initialLocation = new Vector2(_random.NextDouble(10, Width), _random.NextDouble(WallSize, Height - WallSize));
                    actor = new Actor(initialLocation);
                } while (DetectCollisions(actor).Any());
                if (i == 0)
                {
                    actor.Behaviours.Add(new ZombieBehaviour(_random));
                }
                else
                {
                    actor.Behaviours.Add(new HumanBehaviour(_random));
                }
                SimObjects.Add(actor);
            }
        }

        public void Update(long elapsedMilliseconds)
        {
            foreach (var simObject in SimObjects)
            {
                simObject.Update(this, elapsedMilliseconds);
            }
        }
    }
}