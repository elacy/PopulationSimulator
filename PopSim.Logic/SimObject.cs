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
            Properties = new ObservableCollection<SimProperty>();

        }

        public ObservableCollection<SimProperty> Properties { get; set; }

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

        protected void RaiseCollision(SimModel simModel, SimState simState, List<SimObject> collidingObjects)
        {
            Collision.Raise(this, new GameCollisionEventArgs(simModel, simState, collidingObjects));
        }

        public void Update(SimModel simModel, SimState simState)
        {
            Updating.Raise(this, new GameStateUpdateEventArgs(simModel, simState));
            OnUpdate(simModel, simState);
            TryMove(simModel, simState, Location.Add(Velocity));
        }


        protected virtual void OnUpdate(SimModel simModel, SimState simState)
        {
            
        }

        protected void TryMove(SimModel simModel, SimState simState, Vector2 newLocation)
        {
            var oldLocation = Location;
            Location = newLocation;
            var collidingObjects = simModel.DetectCollisions(this).ToList();
            if (collidingObjects.Count > 0)
            {
                Location = oldLocation;
                RaiseCollision(simModel, simState, collidingObjects);
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
