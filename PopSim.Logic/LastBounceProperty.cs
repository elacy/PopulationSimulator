using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic
{
    public class LastBounceProperty : SimProperty
    {
        private long _lastBounceTime;

        public long LastBounceTime
        {
            get { return _lastBounceTime; }
            private set
            {
                if (_lastBounceTime == value)
                {
                    return;
                }
                _lastBounceTime = value;
                OnPropertyChanged();
            }
        }


        public void UpdateLastBounce(SimState simState)
        {
            LastBounceTime = simState.MillisecondsSinceSimStart;
        }
    }
}
