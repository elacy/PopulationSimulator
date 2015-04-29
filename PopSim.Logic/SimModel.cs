using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PopSim.Logic.BeeSim;
using PopSim.Logic.ZombieSim;
using PopSim.Wpf;

namespace PopSim.Logic
{
    public class SimModel
    {
        private readonly CollisionDetection _collisionDetection;
        private readonly Random _random;
        public double Width { get; set; }
        public double Height { get; set; }
        public ObservableList<SimObject> SimObjects { get; set; }

        public SimModel(CollisionDetection collisionDetection, Random random)
        {
            _collisionDetection = collisionDetection;
            _random = random;
            SimObjects = new ObservableList<SimObject>();
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
        
        public void CreateZombiePopulation(double width, double height)
        {
            Width = width;
            Height = height;

            const double MaxEnergy = 50;

            CreateBoundaryWalls();

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
                actor.Properties.Add(new EnergyProperty(MaxEnergy, MaxEnergy));
                actor.Behaviours.Add(new EnergyUseBehaviour());
                SimObjects.Add(actor);
            }
        }

        public void CreateHiveAndFlower(double width, double height)
        {
            Width = width;
            Height = height;

            CreateBoundaryWalls();

            var hive = new Hive(new Vector2(10, 10), new Size2(30, 30));
            hive.Behaviours.Add(new HiveBehaviour(_random));
            SimObjects.Add(hive);

            
            var flower = new Flower(new Vector2(Width - 60, Height -60), new Size2(47, 44));
            SimObjects.Add(flower);
        }

        private void CreateBoundaryWalls()
        {
            //Added four walls
            SimObjects.Add(new WallObject(new Vector2(0, 0), new Size2(Width, WallSize)));
            SimObjects.Add(new WallObject(new Vector2(0, 0), new Size2(WallSize, Height)));
            SimObjects.Add(new WallObject(new Vector2(Width - WallSize, 0), new Size2(WallSize, Height)));
            SimObjects.Add(new WallObject(new Vector2(0, Height - WallSize), new Size2(Width, WallSize)));
        }


        public void Update(SimState simState)
        {
            foreach (var simObject in SimObjects.ToList())
            {
                simObject.Update(this, simState);
            }
        }
    }
}