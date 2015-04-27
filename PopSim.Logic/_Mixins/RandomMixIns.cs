using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic
{
    public static class RandomMixIns
    {
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return (random.NextDouble() * (maxValue - minValue)) + minValue;
        }
    }
}
