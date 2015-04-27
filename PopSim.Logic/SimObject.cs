using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using PopSim.Logic.Annotations;
using PopSim.Logic._Mixins;

namespace PopSim.Logic
{
    public abstract class SimObject:INotifyPropertyChanged
    {

        protected SimObject()
        {
            Behaviours = new ObservableCollection<Behaviour>();
            Behaviours.CollectionChanged += OnBehavioursCollectionChanged;
            Velocity = new Vector2(0,0);
        }

        #region Behaviours
        private void OnBehavioursCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.Handle<Behaviour>(AddBehaviour,RemoveBehaviour);
        }

        private void RemoveBehaviour(Behaviour behaviour)
        {
            behaviour.Detach(this);
        }

        private void AddBehaviour(Behaviour behaviour)
        {
            behaviour.Attach(this);
        }
        #endregion

        //Top Left
        #region Location Property 

        private Vector2 _location;
        public Vector2 Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Color Property

        private Color _color;

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Size Property

        private Size2 _size;

        public Size2 Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }

        #endregion


        public Vector2 Velocity { get; set; }

        public ObservableCollection<Behaviour> Behaviours { get; private set; } 

        public abstract bool CanHaveCollisions { get;}

        public event EventHandler<GameStateUpdateEventArgs> Updating;

        public event EventHandler<GameCollisionEventArgs> Collision;

        protected void RaiseCollision(GameState gameState, long elapsedMilliseconds, List<SimObject> collidingObjects )
        {
            Collision.Raise(this,new GameCollisionEventArgs(gameState,elapsedMilliseconds,collidingObjects));
        }

        public void Update(GameState gameState, long elapsedMilliseconds)
        {
            Updating.Raise(this, new GameStateUpdateEventArgs(gameState, elapsedMilliseconds));
            OnUpdate(gameState, elapsedMilliseconds);
            TryMove(gameState, elapsedMilliseconds, Location.Add(Velocity));
        }


        protected virtual void OnUpdate(GameState gameState, long elapsedMilliseconds)
        {
            
        }

        protected void TryMove(GameState gameState, long elapsedMilliseconds, Vector2 newLocation)
        {
            var oldLocation = Location;
            Location = newLocation;
            var collidingObjects = gameState.DetectCollisions(this).ToList();
            if (collidingObjects.Count > 0)
            {
                Location = oldLocation;
                RaiseCollision(gameState,elapsedMilliseconds,collidingObjects);
            }
        }

        #region PropertyChangedEventHandling
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
