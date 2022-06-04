namespace AzMogaTukITam.Model
{
    public sealed class PlayerLayer : LayerBase
    {
        public HashSet<int> AttackedRows = new();
        public HashSet<int> AttackedColumns = new();
        public HashSet<int> AttackedLeftDiagonals = new();
        public HashSet<int> AttackedRightDiagonals = new();

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
                foreach (var layer in playerLayers)
                {
                    var positionOccupied =
                        layer.AttackedRows.Contains(row) ||
                        layer.AttackedColumns.Contains(col) ||
                        layer.AttackedLeftDiagonals.Contains(col - row) ||
                        layer.AttackedRightDiagonals.Contains(col + row);

                    if (positionOccupied)
                        return false;
                }

            return true;
        }

        public bool IsPlaceSafe(int row, int col)
        {
            var positionOccupied =
                AttackedRows.Contains(row) ||
                AttackedColumns.Contains(col) ||
                AttackedLeftDiagonals.Contains(col - row) ||
                AttackedRightDiagonals.Contains(col + row);

            return !positionOccupied;
        }

        private void MarkPositions(int row, int col)
        {
            AttackedRows.Add(row);
            AttackedColumns.Add(col);
            AttackedLeftDiagonals.Add(col - row);
            AttackedRightDiagonals.Add(col + row);
            Data[row, col] = true;
        }

        private void ReturnAttackedCoordinates(Grid grid)
        {
            List<Coordinates> attackedCoordinates = new();

            foreach (int row in AttackedRows)
            {
                for (int i = 0; i < grid.Width; i++)
                {
                    attackedCoordinates.Add(new Coordinates(row, i));
                }
            }

            foreach (int col in AttackedColumns)
            {
                for (int i = 0; i < grid.Height; i++)
                {
                    attackedCoordinates.Add(new Coordinates(i, col));
                }
            }
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