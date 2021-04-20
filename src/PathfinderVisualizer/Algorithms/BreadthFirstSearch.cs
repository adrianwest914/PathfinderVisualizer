using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.Generic;

namespace PathfinderVisualizer.Algorithms
{
    class BreadthFirstSearch
    {
        public static async Task<List<Square>> GetPath(Grid grid)
        {
            Queue<Square> ToCheck = new Queue<Square>();
            ToCheck.Enqueue(grid.start);
            Dictionary<Square, Square> PreviousMoves = new Dictionary<Square, Square>
            {
                //Adds previous moves
                { grid.start, null }
            };
            Square current;

            while (ToCheck.Count > 0)
            {
                current = ToCheck.Dequeue();

                if (current.Equals(grid.goal))
                    break;

                for (int i = 0; i < current.neighbors.Count; i++)
                {
                    if (current.neighbors[i].isWall || PreviousMoves.ContainsKey(current.neighbors[i]))
                        continue;
                    else if (!ToCheck.Contains(current.neighbors[i]))
                    {
                        ToCheck.Enqueue(current.neighbors[i]);
                        PreviousMoves.Add(current.neighbors[i], current);
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
