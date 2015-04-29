using System;

namespace PopSim.Logic
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

        private readonly object _energyLockObject = new object();

        public void AddEnergy(double amount)
        {
            lock (_energyLockObject)
            {
                Energy = Math.Min(_maxEnergy, Math.Max(Energy + amount, 0));
            }
        }

        public void UseEnergy(double amount)
        {
            AddEnergy(-Math.Abs(amount));
        }
    }
}
