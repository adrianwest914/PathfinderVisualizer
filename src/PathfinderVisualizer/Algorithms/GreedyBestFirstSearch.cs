using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.Generic;

namespace PathfinderVisualizer.Algorithms
{
    class GreedyBestFirstSearch
    {
        public static async Task<List<Square>> GetPath(Grid field)
        {
            PriorityQueue<Square> ToCheck = new PriorityQueue<Square>();
            ToCheck.Enqueue(field.start, 0);
            Dictionary<Square, Square> PreviousMoves = new Dictionary<Square, Square>();
            PreviousMoves.Add(field.start, null);
            Square current;

            while (ToCheck.Count() > 0)
            {
                current = ToCheck.Dequeue();

                if (current.Equals(field.goal))
                    break;

                for (int i = 0; i < current.neighbors.Count; i++)
                {
                    Square next = current.neighbors[i];
                    if (next.isWall)
                        continue;
                    else if (!PreviousMoves.ContainsKey(next))
                    {
                        int distance = Grid.MeasureDistance(field.goal, next);
                        ToCheck.Enqueue(next, distance);
                        PreviousMoves[next] = current;
                    }
                }

                if (current != field.start)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => current.ColorChecked()));
                    int.TryParse(App.Current.Properties["Speed"].ToString(), out int Speed);
                    await Task.Delay(50 * Speed + 20);
                }
            }

            List<Square> Path = new List<Square>();
            current = field.goal;
            while (current != field.start)
            {
                Path.Add(current);
                current = PreviousMoves[current];
            }
            Path.Add(field.start);
            Path.Reverse();
            return Path;
        }
    }
}
