using System;
using System.Collections;
using PathfinderVisualizer.WPF.AStarVisualizer;

namespace PathfinderVisualizer.WPF.AStarAlgorithm
{
    public static class AStarValues
    {
        public static void InitAStarTiles()
        {
            AStarTiles = new AStarTile[_NumGridRows, _NumGridColumns];

            for (int i = 0; i < _NumGridRows; i++)
                for (int j = 0; j < _NumGridColumns; j++)
                    AStarTiles[i, j] = new AStarTile(i, j, Tile.Empty);
        }

        #region Grid Dimensions
        private static readonly object lockGridValuesObject = new object();
        private static int _NumGridRows = StartupValues.NumGridColumns;
        private static int _NumGridColumns = StartupValues.NumGridColumns;

        public static int NumGridRows
        {
            get { lock (lockGridValuesObject) return _NumGridRows; }
            set
            {
                lock (lockGridValuesObject)
                {
                    if (_NumGridRows == value)
                        return;

                    _NumGridRows = value;
                }
            }
        }
        public static int NumGridColumns
        {
            get { lock (lockGridValuesObject) return _NumGridColumns; }
            set
            {
                lock (lockGridValuesObject)
                {
                    if (_NumGridColumns == value)
                        return;

                    _NumGridColumns = value;
                }
                OnGridDimensionChanged(EventArgs.Empty);
            }
        }

        public static event EventHandler GridDimensionChanged;
        private static void OnGridDimensionChanged(EventArgs args)
        {
            EventHandler handler = GridDimensionChanged;

            AStarTiles = new AStarTile[NumGridRows, NumGridColumns];

            if (handler != null)
                GridDimensionChanged(null, args);
        }
        #endregion

        #region Algorithm Controls
        private static readonly object lockAStarControlsObject = new object();
        private static State _AStarState = State.HasNotStarted;
        public static State AStarState
        {
            get { lock (lockAStarControlsObject) return _AStarState; }
            set
            {
                lock (lockAStarControlsObject)
                {
                    _AStarState = value;
                    AlgorithmStateChanged(null, EventArgs.Empty);
                }
            }
        }

        public static EventHandler AlgorithmStateChanged;
        #endregion

        #region Delay Control
        private static readonly object lockDelayControlObject = new object();
        private static uint _Delay = StartupValues.Currentelay;
        public static uint Delay
        {
            get { lock (lockDelayControlObject) return _Delay; }
            set
            {
                lock (lockDelayControlObject)
                {
                    _Delay = value;
                    OnDelayChanged();
                }
            }
        }

        private static void OnDelayChanged()
        {
            EventHandler handler = DelayChanged;
            if (handler != null)
                DelayChanged(null, null);
        }
        public static event EventHandler DelayChanged;
        #endregion

        #region Diagonal Paths Enabled
        private static object lockDiagonalPathsEnabledObject = new object();
        private static bool _DiagonalPathsEnabled = false;
        public static bool DiagonalPathsEnabled
        {
            get { lock (lockDiagonalPathsEnabledObject) return _DiagonalPathsEnabled; }
            set { lock (lockDiagonalPathsEnabledObject) _DiagonalPathsEnabled = value; }
        }
        #endregion

        #region AStar Tiles
        public static readonly object lockAStarTilesObject = new object();
        private static AStarTile _StartTile = null;
        private static AStarTile _GoalTile = null;
        private static AStarTile[,] _AStarTiles = new AStarTile[NumGridRows, NumGridColumns];

        public static AStarTile StartTile
        {
            get { lock (lockAStarTilesObject) return _StartTile; }
            set { lock (lockAStarTilesObject) _StartTile = value; }
        }
        public static AStarTile GoalTile
        {
            get { lock (lockAStarTilesObject) return _GoalTile; }
            set { lock (lockAStarTilesObject) _GoalTile = value; }
        }
        public static AStarTile[,] AStarTiles
        {
            get { lock (lockAStarTilesObject) return _AStarTiles; }
            set { lock (lockAStarTilesObject) _AStarTiles = value; }
        }

        public static void SetAStarTile(AStarTile newTile)
        {
            lock (lockAStarTilesObject)
            {
                Tile tileType = newTile.TileType;
                if (tileType == Tile.Goal)
                {
                    if (GoalTile != null)
                    {
                        var previousGoalTile = GoalTile;
                        int rowIndex = previousGoalTile.RowIndex;
                        int columnIndex = previousGoalTile.ColumnIndex;

                        var tile = new AStarTile(rowIndex, columnIndex, Tile.Empty);
                        SetAStarTile(tile);
                    }

                    GoalTile = newTile;

                }
                if (tileType == Tile.Start)
                {
                    if (StartTile != null)
                    {
                        var previousStartTile = StartTile;
                        int rowIndex = previousStartTile.RowIndex;
                        int columnIndex = previousStartTile.ColumnIndex;

                        var tile = new AStarTile(rowIndex, columnIndex, Tile.Empty);
                        SetAStarTile(tile);
                    }

                    StartTile = newTile;

                }

                AStarTiles[newTile.RowIndex, newTile.ColumnIndex] = newTile;
                OnTileChanged(newTile);
            }
        }

        private static void OnTileChanged(AStarTile newTile)
        {
            TileChangedHandler handler = TileHasChanged;

            if (handler != null)
                TileHasChanged(null, newTile.RowIndex, newTile.ColumnIndex);
        }
        public delegate void TileChangedHandler(object sender, int RowIndex, int ColumnInex);
        public static event TileChangedHandler TileHasChanged;
        #endregion

        #region Solution Path
        public static readonly object lockPathObject = new object();
        private static ArrayList _Path = null;

        public static ArrayList Path
        {
            get { lock (lockPathObject) return _Path; }
            set
            {
                lock (lockPathObject)
                {
                    _Path = value;
                    OnPathFound();
                }
            }
        }

        private static void OnPathFound()
        {
            EventHandler handler = PathChanged;
            if (handler != null)
                PathChanged(null, null);
        }
        public static event EventHandler PathChanged;
        #endregion
    }
}