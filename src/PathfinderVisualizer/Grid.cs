using System;
using System.Collections.Generic;

namespace PathfinderVisualizer
{
    public class Grid
    {
        public Square start;
        public Square goal;
        public Square[,] grid;
        public static Grid current = null;

        public Grid(int rows, int columns)
        {
            grid = new Square[rows, columns];
        }
        public void AddSquare(Square square)
        {
            grid[square.x, square.y] = square;
        }
        public void SetStart(int x, int y)
        {
            start?.ResetColor();
            start = grid[x, y];
            start.ColorStart();
        }
        public void SetGoal(int x, int y)
        {
            goal?.ResetColor();
            goal = grid[x, y];
            goal.ColorGoal();
        }
        public void SetWall(int x, int y, bool set = true)
        {
            grid[x, y].isWall = set;
            if (set)
            {
                grid[x, y].weight = 1;
                grid[x, y].ColorWall();
            }
            else
                grid[x, y].ResetColor();
        }
        public void SetWeight(int x, int y, int weight = 10)
        {
            grid[x, y].weight = weight;

            if (weight > 1)
                grid[x, y].ColorWeight();
            else
                grid[x, y].ResetColor();
        }
        public void ClearWalls()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    grid[i, j].isWall = false;
        }
        public void AddNeighbors()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    grid[i, j].neighbors = FindNeighbors(grid[i, j]);
        }
        public static (int, int) GetCoordinates(string name)
        {
            int x = int.Parse(name.Substring(1, 2));
            int y = int.Parse(name.Substring(3, 2));
            return (x, y);
        }
        public static int MeasureDistance(Square a, Square b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        private List<Square> FindNeighbors(Square square)
        {
            List<Square> neighbors = new List<Square>();
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) //self node
                        continue;

                    int neighborX = square.x + i;
                    int neighborY = square.y + j;

                    if (neighborX != square.x && neighborY != square.y) //diagonals arent neighbors
                        continue;

                    if ((neighborX >= 0 && neighborX < grid.GetLength(0)) && neighborY >= 0 && neighborY < grid.GetLength(1))
                        neighbors.Add(grid[neighborX, neighborY]);
                }

            return neighbors;
        }
    }
}
