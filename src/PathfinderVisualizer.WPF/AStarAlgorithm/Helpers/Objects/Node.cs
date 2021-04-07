using System;

namespace PathfinderVisualizer.WPF.AStarAlgorithm.AStarImplementation.AlgorithmThread
{
    public class Node : IComparable
    {
        public double Heuristic;
        public double MovementCost;
        public double TotalCost { get { return Heuristic + MovementCost * 500; } }
        public int RowIndex;
        public int ColumnIndex;
        public bool IsWalkable;
        public Node Parent;
        private static int Count = 0;
        private int Id;

        public Node(AStarTile tile, Node parent = null)
        {
            Id = Count;
            Count++;

            RowIndex = tile.RowIndex;
            ColumnIndex = tile.ColumnIndex;
            Parent = parent;
            IsWalkable = !(tile.TileType == Tile.Wall);
        }
        public int CompareTo(object obj)
        {
            Node node = (Node)obj;

            int result = TotalCost.CompareTo(node.TotalCost);

            return result;
        }
    }
}
