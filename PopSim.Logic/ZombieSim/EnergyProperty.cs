using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopSim.Logic.ZombieSim
{
    public class EnergyProperty:SimProperty
    {
        private readonly double _maxEnergy;
        private double _energy;

        public EnergyProperty(double maxEnergy, double initialEnergy)
        {
            _maxEnergy = maxEnergy;
            Energy = initialEnergy;
        }

        public double Energy
        {
            get { return _energy; }
            private set
            {
                _energy = value;
                OnPropertyChanged();
            }
        }

        public void ChangeEnergy(double amount)
        {
            Energy = Math.Min(_maxEnergy, Math.Max(Energy + amount, 0));
        }

    }
}
