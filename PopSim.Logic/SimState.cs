using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic
{
    public class SimState
    {
        public SimState(long millisecondsSinceLastUpdate, long millisecondsSinceSimStart)
        {
            MillisecondsSinceLastUpdate = millisecondsSinceLastUpdate;
            MillisecondsSinceSimStart = millisecondsSinceSimStart;
        }
        public long MillisecondsSinceLastUpdate { get; private set; }
        public long MillisecondsSinceSimStart { get; private set; }
    }
}
