using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic
{
    public static class EventHandlerMixIn
    {
        public static void Raise<T>(this EventHandler<T> handler, object sender, T e = default(T))
        {
            var temp = handler;
            if (temp != null)
            {
                temp(sender, e);
            }
        }
    }
}
