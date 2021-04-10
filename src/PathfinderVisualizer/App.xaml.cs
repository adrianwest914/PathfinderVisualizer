using System.Windows;
using System.Windows.Media;


namespace PathfinderVisualizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly Brush SDefault = System.Windows.Media.Brushes.Gray;
        public static readonly Brush SWall = System.Windows.Media.Brushes.Black;
        public static readonly Brush SWeight = System.Windows.Media.Brushes.DarkBlue;
        public static readonly Brush SGoal = System.Windows.Media.Brushes.Red;
        public static readonly Brush SStart = System.Windows.Media.Brushes.Green;
        public static readonly Brush SDefaultVisited = new SolidColorBrush(Color.FromRgb(50, 50, 50));
        public static readonly Brush SWeightVisited = System.Windows.Media.Brushes.LightSkyBlue;
        public static readonly Brush SDefaultPath = System.Windows.Media.Brushes.White;
        public static readonly Brush SWeightPath = System.Windows.Media.Brushes.White;
    }
}
