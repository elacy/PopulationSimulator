using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PopSim.Logic;

namespace PopSim.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            GameState = new GameState(new CollisionDetection(), new Random());
            GameState.CreateInitialPopulation(800, 800);
            UpdateTimer = new Timer(PerformUpdate, null, 0, 50);
            Stopwatch = new Stopwatch();
            DataContext = GameState;
        }

        public Stopwatch Stopwatch { get; set; }

        protected override void OnClosing(CancelEventArgs e)
        {
            UpdateTimer.Dispose();
            base.OnClosing(e);
        }


        public Timer UpdateTimer { get; set; }

        public Timer DrawTimer { get; set; }

        public GameState GameState { get; set; }


        private void PerformUpdate(object state)
        {
            GameState.Update(Stopwatch.ElapsedMilliseconds);
            Stopwatch.Restart();
        }
    }
}
