namespace AzMogaTukITam.Model
{
    public sealed class PlayerLayer : LayerBase
    {
        public static HashSet<int> _attackedRows = new HashSet<int>();
        public static HashSet<int> _attackedColumns = new HashSet<int>();
        public static HashSet<int> _attackedLeftDiagonals = new HashSet<int>();
        public static HashSet<int> _attackedRightDiagonals = new HashSet<int>();

        private PlayerLayer(Grid grid) : base(grid)
        {
        }

        public PlayerLayer(Grid grid, DisplayValue dv, string pn, Coordinates startPosition) : this(grid)
        {
            DisplayValue = dv;
            PlayerName = pn;
            Data[startPosition.Y, startPosition.X] = true;
        }

        public void PlaceQueen(int row, int col, Grid grid)
        {
            if (CanPlaceQueen(row, col, grid))
            {
                MarkPositions(row, col);
            }
        }

        private bool CanPlaceQueen(int row, int col, Grid grid)
        {
            if (grid.Layers.Where(x => x is PlayerLayer && x.LayerID != LayerID) is IEnumerable<PlayerLayer>
                playerLayers)
                foreach (var unused in playerLayers)
                {
                    var positionOccupied =
                        _attackedRows.Contains(row) ||
                        _attackedColumns.Contains(col) ||
                        _attackedLeftDiagonals.Contains(col - row) ||
                        _attackedRightDiagonals.Contains(col + row);

                    if (positionOccupied)
                        return false;
                }

            return true;
        }

        private void MarkPositions(int row, int col)
        {
            _attackedRows.Add(row);
            _attackedColumns.Add(col);
            _attackedLeftDiagonals.Add(col - row);
            _attackedRightDiagonals.Add(col + row);
            Data[row, col] = true;
        }

        public override int ZIndex { get; protected set; } = 0;
        public override DisplayValue DisplayValue { get; protected set; } = new DisplayValue() {Value = 'X'};
        public override bool[,] Data { get; protected set; }
        public override int ConsolePriority { get; protected set; } = 0;
        public override int RequiredTurns { get; protected set; } = 0;
        public override Action<Game, ConsoleKeyInfo> ConsoleAction { get; protected set; }
        public override Action<Game> UpdateAction { get; protected set; }

        public string PlayerName { get; private set; }

        // Override props
    }
}