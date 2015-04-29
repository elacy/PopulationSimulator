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
        private static readonly double ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
        private static readonly double ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void StartZombieSimulator(object sender, RoutedEventArgs e)
        {
            var gameState = new SimModel(new CollisionDetection(), new Random());
            gameState.CreateZombiePopulation(ScreenWidth/2, ScreenHeight/2);

            var simulation = new SimulationRunner(gameState);
            simulation.Start("Zombie Simulation");
        }

        private void StartBeeSimulator(object sender, RoutedEventArgs e)
        {
            var gameState = new SimModel(new CollisionDetection(), new Random());
            gameState.CreateHiveAndFlower(ScreenWidth / 2, ScreenHeight / 2);

            var simulation = new SimulationRunner(gameState);
            simulation.Start("Bee Simulation");
        }
    }
}
