using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic.BeeSim
{
    public class NectarProperty:SimProperty
    {
        private bool _hasNectar;

        public bool HasNectar
        {
            get { return _hasNectar; }
            private set
            {
                if (_hasNectar == value)
                {
                    return;
                }
                _hasNectar = value;
                OnPropertyChanged();
            }
        }

        private readonly object _nectarLockObject = new object();

        public void FillNectar()
        {
            lock (_nectarLockObject)
            {
                HasNectar = true;
            }
        }

        public void DrainNectar()
        {
            lock (_nectarLockObject)
            {
                HasNectar = false;
            }
        }

        public bool GetNectarFrom(NectarProperty nectarProperty)
        {
            if (nectarProperty == null)
            {
                return false;
            }
            lock (_nectarLockObject)
            {
                lock (nectarProperty._nectarLockObject)
                {
                    if (HasNectar || !nectarProperty.HasNectar)
                    {
                        return false;
                    }
                    HasNectar = true;
                    nectarProperty.HasNectar = false;
                    return true;
                }
            }
        }
    }
}
