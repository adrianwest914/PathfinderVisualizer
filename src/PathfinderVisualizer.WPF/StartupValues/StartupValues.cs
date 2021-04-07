using System;

namespace PathfinderVisualizer.WPF.AStarVisualizer
{
    static class StartupValues
    {
        //Grid Dimensions
        public static readonly int NumGridRows = 40;
        public static readonly int NumGridColumns = 40;
        public static int MaxDimension => Math.Max(NumGridRows, NumGridColumns);

        //Delay Properties
        public static readonly uint Currentelay = 0;
        public static readonly uint MinDelay = 0;
        public static readonly uint MaxDelay = 250;

        //Diagnonal Paths
        public static readonly bool DiagonalPathsEnabled = true;
    }
}
