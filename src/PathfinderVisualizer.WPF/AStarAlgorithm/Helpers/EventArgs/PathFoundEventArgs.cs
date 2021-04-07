using System;
using System.Collections;

namespace PathfinderVisualizer.WPF.AStarAlgorithm.AStarImplementation.AlgorithmThread.Helpers
{
    public class PathFoundEventArgs : EventArgs
    {
        public ArrayList Path { get; set; }
        public PathFoundEventArgs(ArrayList path)
        {
            Path = path;
        }
    }
}
