using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.Generic;

namespace PathfinderVisualizer.Algorithms
{
    public static class Dijikstra
    {
        public static async Task<List<Square>> GetPath(Grid grid)
        {
            PriorityQueue<Square> ToCheck = new PriorityQueue<Square>();
            ToCheck.Enqueue(grid.start, 0);
            Dictionary<Square, Square> PreviousMoves = new Dictionary<Square, Square>();
            Dictionary<Square, int> CostSoFar = new Dictionary<Square, int>();
            PreviousMoves.Add(grid.start, null);
            CostSoFar[grid.start] = 0;
            Square current;

            while (ToCheck.Count() > 0)
            {
                current = ToCheck.Dequeue();

                if (current.Equals(grid.goal))
                    break;

                for (int i = 0; i < current.neighbors.Count; i++)
                {
                    Square next = current.neighbors[i];
                    int newCost = CostSoFar[current] + next.weight;
                    if (next.isWall)
                        continue;
                    else if (next == grid.goal)
                    {
                        ToCheck.Enqueue(next, 0);
                        PreviousMoves[next] = current;
                    }
                    else if (!CostSoFar.ContainsKey(next) || newCost < CostSoFar[next])
                    {
                        CostSoFar[next] = newCost;
                        ToCheck.Enqueue(next, newCost);
                        PreviousMoves[next] = current;
                    }
                }

                if (current != grid.start)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => current.ColorChecked()));
                    int.TryParse(App.Current.Properties["Speed"].ToString(), out int Speed);
                    await Task.Delay(50 * Speed + 20);
                }
            }

            List<Square> Path = new List<Square>();
            current = grid.goal;
            while (current != grid.start)
            {
                Path.Add(current);
                current = PreviousMoves[current];
            }
            Path.Add(grid.start);
            Path.Reverse();
            return Path;
        }
    }
}
