using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using PopSim.Logic;

namespace PopSim.Presentation
{
    public class SimulationRunner
    {
        private readonly SimModel _simModel;
        private Stopwatch _updateStopwatch;
        private Stopwatch _overallStopwatch;
        private Timer _updateTimer;

        public SimulationRunner(SimModel simModel)
        {
            _simModel = simModel;
        }


        public void Start(string title)
        {
            var simulationWindow = new SimulationWindow
            {
                DataContext = _simModel,
                Title = title,
                Left = 0,
                Top = 0
            };
            _updateTimer = new Timer(PerformUpdate, null, 0, 50);
            _updateStopwatch = new Stopwatch();
            _overallStopwatch = new Stopwatch();
            _overallStopwatch.Start();
            simulationWindow.Closing += SimulationWindowOnClosing;
            simulationWindow.Show();
        }

        private void SimulationWindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            Stop();
        }

        public void Stop()
        {
            _updateTimer.Dispose();
        }

        private void PerformUpdate(object state)
        {
            _simModel.Update(new SimState(_updateStopwatch.ElapsedMilliseconds,_overallStopwatch.ElapsedMilliseconds));
            _updateStopwatch.Restart();
        }
    }
}