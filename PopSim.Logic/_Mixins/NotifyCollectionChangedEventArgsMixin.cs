using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic._Mixins
{
    public static class NotifyCollectionChangedEventArgsMixin
    {
        public static void Handle<T>(this NotifyCollectionChangedEventArgs args, Action<T> newItemHandler,
            Action<T> oldItemHandler)
        {
            if (args.OldItems != null)
            {
                foreach (T item in args.OldItems)
                {
                    oldItemHandler(item);
                }
            }
            if (args.NewItems != null)
            {
                foreach (T item in args.NewItems)
                {
                    newItemHandler(item);
                }
            }
        }
    }
}
