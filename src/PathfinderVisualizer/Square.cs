using System;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace PathfinderVisualizer
{
    public class Square : IComparable<Square>
    {
        public int x { get; set; }
        public int y { get; set; }
        public int weight = 1;
        public bool isWall = false;
        public List<Square> neighbors = new List<Square>();
        private readonly Rectangle uiRef;

        public Square(Rectangle uiRef)
        {
            this.uiRef = uiRef;
        }
        public Square(Rectangle uiRef, int x, int y)
        {
            this.uiRef = uiRef;
            this.x = x;
            this.y = y;
        }
        public int CompareTo(Square other)
        {
            if (weight < other.weight)
                return -1;
            else if (weight > other.weight)
                return 1;
            else
                return 0;
        }
        public void ResetColor()
        {
            uiRef.Fill = App.SDefault;
        }
        public void ColorStart()
        {
            uiRef.Fill = App.SStart;
        }
        public void ColorGoal()
        {
            uiRef.Fill = App.SGoal;
        }
        public void ColorChecked()
        {
            if (weight == 1)
                uiRef.Fill = App.SDefaultVisited;
            else
                uiRef.Fill = App.SWeightVisited;
        }
        public void ColorPath()
        {
            if (weight == 1)
                uiRef.Fill = App.SDefaultPath;
            else
                uiRef.Fill = App.SWeightPath;
        }
        public void ColorWall()
        {
            uiRef.Fill = App.SWall;
        }
        public void ColorWeight()
        {
            uiRef.Fill = App.SWeight;
        }
        public override string ToString()
        {
            return "Square: (" + x + ", " + y + ")";
        }
    }
}
