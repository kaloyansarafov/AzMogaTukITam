namespace AzMogaTukITam.Model
{
    public sealed class PlayerLayer : LayerBase
    {
        private int _currentTurn = 0;

        public static HashSet<int> _attackedRows = new HashSet<int>();
        public static HashSet<int> _attackedColumns = new HashSet<int>();
        public static HashSet<int> _attackedLeftDiagonals = new HashSet<int>();
        public static HashSet<int> _attackedRightDiagonals = new HashSet<int>();
        public HashSet<int> AttackedRows = new HashSet<int>();
        public HashSet<int> AttackedColumns = new HashSet<int>();
        public HashSet<int> AttackedLeftDiagonals = new HashSet<int>();
        public HashSet<int> AttackedRightDiagonals = new HashSet<int>();

        private PlayerLayer(Grid grid) : base(grid)
        {
        }

        public PlayerLayer(Grid grid, DisplayValue dv, string pn) : this(grid)
        {
            DisplayValue = dv;
            PlayerName = pn;
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

        public override int ZIndex { get; protected set; } = 1;
        public override DisplayValue DisplayValue { get; protected set; } = new DisplayValue() { Value = 'X' };
        public override bool[,] Data { get; protected set; }
        public override int ConsolePriority { get; protected set; } = 0;
        public override int RequiredTurns { get; protected set; } = 1;
        public override Action<Game, ConsoleKeyInfo> ConsoleAction { get; protected set; }
        public override Action<Game> UpdateAction { get; protected set; }

        public string PlayerName { get; private set; }

        // Override props

        private void HandleConsole(Game game, ConsoleKeyInfo ki)
        {
            if (_currentTurn == 0)
                game.DrawMessage($"{PlayerName} shall choose next!", 2000);
            var selectedLayer = (SelectedLayer)game.Grid.Layers.First(l => l is SelectedLayer);
            if (_currentTurn == 0)
            {
                selectedLayer.SetCurrentPointer(new Coordinates() { Y = game.Grid.Height / 2, X = game.Grid.Width/2 });
                _currentTurn++;
            }
            switch (ki.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedLayer.MoveCurrentPointer(new Coordinates() { Y = -1 });
                    break;
                case ConsoleKey.DownArrow:
                    selectedLayer.MoveCurrentPointer(new Coordinates() { Y = 1 });
                    break;
                case ConsoleKey.RightArrow:
                    selectedLayer.MoveCurrentPointer(new Coordinates() { X = 1 });
                    break;
                case ConsoleKey.LeftArrow:
                    selectedLayer.MoveCurrentPointer(new Coordinates() { X = -1 });
                    break;
                case ConsoleKey.Enter:
                    // Check for safe 
                    this.Data[selectedLayer.CurrentPointer.Y, selectedLayer.CurrentPointer.X] = true;
                    // Block
                    this.OnTurnDone();
                    if (_currentTurn <= this.RequiredTurns +1)
                        selectedLayer.SetCurrentPointer(new Coordinates() { Y = game.Grid.Height / 2, X = game.Grid.Width/2 });
                    break;
                default:
                    break;
            }
        }

    }
}