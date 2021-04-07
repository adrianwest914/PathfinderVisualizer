using System;

namespace PathfinderVisualizer.WPF.AStarAlgorithm.AStarImplementation.AlgorithmThread.Helpers
{
    public class NodeAddedToCollectionEventArgs : EventArgs
    {
        public Node node { get; set; }
        public NodeAddedToCollectionEventArgs(Node node)
        {
            this.node = node;
        }

        
    }
}
