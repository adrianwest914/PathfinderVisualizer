using System;
using System.Windows;
using System.Threading;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;
using PathfinderVisualizer.Algorithms;


namespace PathfinderVisualizer
{
    /// <summary>
    /// Issue in ClearPathBtnClick method
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool m_choosingStart = true;
        private bool m_choosingGoal = false;
        private bool m_drawingWalls = false;
        private bool m_drawingWeights = false;

        public MainWindow()
        {
            InitializeComponent();
            App.Current.Properties["Speed"] = 0;
        }

        private void PopulateGrid()
        {
            int rowNumber = 10;
            int columnNumber = 25;

            if (PathGrid.ActualWidth != 0 && PathGrid.Width != 0)
            {
                rowNumber = (int)PathGrid.ActualHeight / 30;
                columnNumber = (int)PathGrid.ActualWidth / 30;
                PathGrid.RowDefinitions.Clear();
                PathGrid.ColumnDefinitions.Clear();
                PathGrid.Children.Clear();
            }

            Grid.current = new Grid(rowNumber, columnNumber);

            for (int i = 0; i <= rowNumber; i++)
                PathGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < columnNumber; i++)
                PathGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < rowNumber; i++)
                for (int j = 0; j < columnNumber; j++)
                {
                    Rectangle square = new Rectangle();
                    string name = "_";

                    if (i < 10)
                        name += "0";
                    name += i;

                    if (j < 10)
                        name += "0";
                    name += j;

                    square.Name = name;
                    square.AllowDrop = true;
                    System.Windows.Controls.Grid.SetRow(square, i);
                    System.Windows.Controls.Grid.SetColumn(square, j);
                    square.Fill = App.SDefault;
                    square.MouseLeftButtonUp += new MouseButtonEventHandler((s, e) => SquareOnLeftMouseButtonUp(s, e));
                    square.MouseMove += new MouseEventHandler((s, e) => SquareOnMouseMove(s, e));
                    square.DragEnter += new DragEventHandler((s, e) => SquareDragEnter(s, e));
                    PathGrid.Children.Add(square);
                    Grid.current.AddSquare(new Square(square, i, j));
                }
            Grid.current.AddNeighbors();
        }
        private void UpdateUI()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate (object par)
            {
                frame.Continue = false;
                return null;
            }), null);
        }
        private void SquareOnLeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle uiSquare = (Rectangle)sender;
            var coord = Grid.GetCoordinates(uiSquare.Name);

            //possible optimization here. 
            //instead of else-if maybe switch?
            if (m_drawingWalls)
            {
                if (Grid.current.grid[coord.Item1, coord.Item2].isWall)
                    Grid.current.SetWall(coord.Item1, coord.Item2, false);
                else if (Grid.current.goal != Grid.current.grid[coord.Item1, coord.Item2] &&
                    Grid.current.start != Grid.current.grid[coord.Item1, coord.Item2])
                    Grid.current.SetWall(coord.Item1, coord.Item2);
            }
            else if (m_drawingWeights)
            {
                if (Grid.current.grid[coord.Item1, coord.Item2].weight > 1)
                    Grid.current.SetWeight(coord.Item1, coord.Item2, 1);
                else if (Grid.current.goal != Grid.current.grid[coord.Item1, coord.Item2] &&
                    Grid.current.start != Grid.current.grid[coord.Item1, coord.Item2] &&
                    !Grid.current.grid[coord.Item1, coord.Item2].isWall)
                    Grid.current.SetWeight(coord.Item1, coord.Item2);
            }
            else if (m_choosingStart)
            {
                Grid.current.SetStart(coord.Item1, coord.Item2);
                m_choosingStart = false;
                m_choosingGoal = true;
            }
            else if (m_choosingGoal)
            {
                Grid.current.SetGoal(coord.Item1, coord.Item2);
                m_choosingGoal = false;
                m_choosingStart = true;
            }
        }
        private void SquareOnMouseMove(object sender, MouseEventArgs e)
        {
            Rectangle uiSquare = sender as Rectangle;
            var coord = Grid.GetCoordinates(uiSquare.Name);

            if (e.LeftButton == MouseButtonState.Pressed)
                if (Grid.current.goal != Grid.current.grid[coord.Item1, coord.Item2] &&
                    Grid.current.start != Grid.current.grid[coord.Item1, coord.Item2])
                {
                    if (m_drawingWalls)
                        DragDrop.DoDragDrop(uiSquare, "", DragDropEffects.Copy);
                    else if (m_drawingWeights && !Grid.current.grid[coord.Item1, coord.Item2].isWall)
                        DragDrop.DoDragDrop(uiSquare, "", DragDropEffects.Copy);
                }
        }
        private void SquareDragEnter(object sender, DragEventArgs e)
        {
            Rectangle uiSquare = sender as Rectangle;
            var coord = Grid.GetCoordinates(uiSquare.Name);

            if (Grid.current.goal != Grid.current.grid[coord.Item1, coord.Item2] &&
                Grid.current.start != Grid.current.grid[coord.Item1, coord.Item2])
            {
                if (m_drawingWalls)
                {
                    if (!Grid.current.grid[coord.Item1, coord.Item2].isWall)
                        Grid.current.SetWall(coord.Item1, coord.Item2);
                    else
                        Grid.current.SetWall(coord.Item1, coord.Item2, false);
                }
                else if (!Grid.current.grid[coord.Item1, coord.Item2].isWall && m_drawingWeights)
                {
                    if (Grid.current.grid[coord.Item1, coord.Item2].weight == 1)
                        Grid.current.SetWeight(coord.Item1, coord.Item2);
                    else
                        Grid.current.SetWeight(coord.Item1, coord.Item2, 1);
                }
            }
        }
        private void PathGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            PopulateGrid();
        }
        private async void FindPathBtnClick(object sender, RoutedEventArgs e)
        {
            if (Grid.current.start == null || Grid.current.goal == null)
            {
                MessageBox.Show("Select start and goal nodes!");
                return;
            }

            ClearPathBtnClick(null, null);
            List<Square> path;

            try
            {
                switch (AlgorithmSelect.SelectedIndex)
                {
                    case 0:
                        path = await AStar.GetPath(Grid.current);
                        break;
                    case 1:
                        path = await Dijkstra.GetPath(Grid.current);
                        break;
                    case 2:
                        path = await GreedyBestFirstSearch.GetPath(Grid.current);
                        break;
                    case 3:
                        path = await BreadthFirstSearch.GetPath(Grid.current);
                        break;
                    default:
                        MessageBox.Show("Error selecting algorithm.");
                        return;
                }
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Path could not be found.");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error finding path: " + ex.Message);
                return;
            }

            for (int i = 0; i < path.Count - 1; i++)
            {
                if (i == 0)
                    path[i].ColorStart();
                else
                    path[i].ColorPath();
                UpdateUI();
                int.TryParse(App.Current.Properties["Speed"].ToString(), out int Speed);
                Thread.Sleep(50 * Speed + 20);
            }
        }
        private void ClearBoardBtnClick(object sender, RoutedEventArgs e)
        {
            Grid.current.start = null;
            Grid.current.goal = null;
            Grid.current.ClearWalls();
            foreach (object child in PathGrid.Children)
            {
                Rectangle rect = (Rectangle)child;
                rect.Fill = App.SDefault;
            }
        }
        private void ClearPathBtnClick(object sender, RoutedEventArgs e)
        {
            WallsCheckBox.IsChecked = false;
            foreach (object child in PathGrid.Children)
            {
                Rectangle rect = (Rectangle)child;
                Square current = Grid.current.grid[Grid.GetCoordinates(rect.Name).Item1, Grid.GetCoordinates(rect.Name).Item2];

                //Issue where this re-adds weighted nodes after clearing the path
                if (current != Grid.current.start && current != Grid.current.goal && !current.isWall && current.weight == 1)
                    current.ResetColor();
                else if (current.weight > 1)
                    current.ColorWeight();
                else if (current.isWall)
                    current.ColorWall();
            }
        }
        private void WallsCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            if (!m_drawingWalls)
                WeightsCheckBox.IsChecked = false;
            m_drawingWalls = !m_drawingWalls;
        }
        private void WeightsCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            if (!m_drawingWeights)
                WallsCheckBox.IsChecked = false;
            m_drawingWeights = !m_drawingWeights;
        }
        private void SpeedSelectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.Current.Properties["Speed"] = SpeedSelect.SelectedIndex;
        }
    }
}
