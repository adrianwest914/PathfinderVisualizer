namespace PathfinderVisualizer.WPF.AStarAlgorithm
{
    public class AStarTile
    {
        public Tile TileType { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        public AStarTile(int RowIndex, int ColumnIndex, Tile AStarTileType = Tile.Empty)
        {
            this.RowIndex = RowIndex;
            this.ColumnIndex = ColumnIndex;
            TileType = AStarTileType;
        }

        public void RedrawTile()
        {
            AStarValues.SetAStarTile(this);
        }
    }
}
