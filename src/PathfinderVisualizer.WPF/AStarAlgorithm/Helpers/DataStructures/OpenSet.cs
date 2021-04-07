﻿using System.Collections.Generic;
using PathfinderVisualizer.Library.DataStructures;

namespace PathfinderVisualizer.WPF.AStarAlgorithm.AStarImplementation.AlgorithmThread.Helpers
{
    public class OpenSet
    {
        MinPriorityQueue<double, Node> openSet;
        HashSet<Node> hashSet = new HashSet<Node>();
        private int _count = 0;
        public int Count { get { return _count; } }

        public OpenSet(int Capacity)
        {
            openSet = new MinPriorityQueue<double, Node>(Capacity);
        }
        public void Add(Node node)
        {
            if (Contains(node))
                return;

            hashSet.Add(node);
            openSet.Add(node.TotalCost, node);
            CallEvent(node);
            _count++;
        }
        public void Remove(Node node)
        {
            if (!Contains(node))
                return;

            hashSet.Remove(node);
            _count--;
        }
        public bool Contains(Node node)
        {
            return hashSet.Contains(node);
        }
        public Node Pop()
        {
            Node result = openSet.GetMinPriorityPair().Value;

            while (!hashSet.Contains(result))
                result = openSet.GetMinPriorityPair().Value;

            return result;
        }

        private void CallEvent(Node node)
        {
            var handler = NodeAddedToCollection;

            if (handler != null)
            {
                var args = new NodeAddedToCollectionEventArgs(node);
                NodeAddedToCollection(this, args);
            }
        }
        public delegate void NodeAddedToCollectionEventHandler(object sender, NodeAddedToCollectionEventArgs args);
        public static event NodeAddedToCollectionEventHandler NodeAddedToCollection;
    }
}
