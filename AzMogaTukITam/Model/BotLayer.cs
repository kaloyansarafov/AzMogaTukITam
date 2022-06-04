namespace AzMogaTukITam.Model
{
    public sealed class BotLayer : LayerBase
    {
        public HashSet<int> AttackedRows = new();
        public HashSet<int> AttackedColumns = new();
        public HashSet<int> AttackedLeftDiagonals = new();
        public HashSet<int> AttackedRightDiagonals = new();

        int _currentTurn = 0;

        private BotLayer(Grid grid) : base(grid)
        {

            this.ConsoleAction = HandleConsole;

        }

        public BotLayer(Grid grid, DisplayValue dv, string pn) : this(grid)
        {
            DisplayValue = dv;
            PlayerName = pn;
        }
        
        //algorithm to find the best place to put a queen, where it blocks the most open spaces in the 4 directions and diagonals
        public Coordinates FindBestPlace(Grid grid)
        {
            Coordinates bestPlace = new Coordinates(0, 0);
            int bestScore = 0;
            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {
                    if (CanPlaceQueen(i, j, grid))
                    {
                        int score = 0;
                        score += GetScore(i, j, grid, AttackedRows);
                        score += GetScore(i, j, grid, AttackedColumns);
                        score += GetScore(i, j, grid, AttackedLeftDiagonals);
                        score += GetScore(i, j, grid, AttackedRightDiagonals);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestPlace = new Coordinates(i, j);
                        }
                    }
                }
            }
            return bestPlace;
        }

        //returns the score of a place, where it blocks the most open spaces in the 4 directions and diagonals
        public int GetScore(int row, int column, Grid grid, HashSet<int> set)
        {
            int score = 0;
            for (int i = 0; i < grid.Height; i++)
            {
                if (set.Contains(i))
                {
                    score++;
                }
            }
            return score;
        }


        private bool CanPlaceQueen(int row, int col, Grid grid)
        {
            if (grid.Layers.Where(x => x is BotLayer).Cast<BotLayer>().ToArray() is BotLayer[]
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

        public bool IsPlaceOccupied(int row, int col)
        {
            return (AttackedRows.Contains(row) ||
                        AttackedColumns.Contains(col) ||
                        AttackedLeftDiagonals.Contains(col - row) ||
                        AttackedRightDiagonals.Contains(col + row));
        }

        private void MarkPositions(int row, int col)
        {
            AttackedRows.Add(row);
            AttackedColumns.Add(col);
            AttackedLeftDiagonals.Add(col - row);
            AttackedRightDiagonals.Add(col + row);
        }

        private IEnumerable<Coordinates> GetAttackedCoords(Grid grid)
        {
            for (int i = 0; i < grid.Height; i++)
                for (int o = 0; o < grid.Width; o++)
                    if (IsPlaceOccupied(i, o)) yield return new Coordinates(i, o);
        }

        public override int ZIndex { get; protected set; } = 150;
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
                selectedLayer.SetCurrentPointer(new Coordinates() { Y = game.Grid.Height / 2, X = game.Grid.Width / 2 });
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

                    if (!CanPlaceQueen(selectedLayer.CurrentPointer.Y, selectedLayer.CurrentPointer.X, game.Grid))
                    {
                        game.DrawMessage($"{PlayerName} shall not be placing here, it's either occupied by the opponent or the player himself!", 6000);
                        break;
                    }

                    MarkPositions(selectedLayer.CurrentPointer.Y, selectedLayer.CurrentPointer.X);
                    this.Data[selectedLayer.CurrentPointer.Y, selectedLayer.CurrentPointer.X] = true;

                    BotLayer[] playerLayers = game.Grid.Layers.Where(x => x is BotLayer).Cast<BotLayer>().ToArray();
                    var oponentCanMove = false;
                    for (int i = 0; i < game.Grid.Height; i++)
                        for (int o = 0; o < game.Grid.Width; o++)
                            if (playerLayers.All(pl => !pl.IsPlaceOccupied(i, o))) oponentCanMove = true;
                    if (!oponentCanMove)
                        game.EndGame(() => game.DrawMessage($"{PlayerName} Won!", 5000));

                    var blockLayer = (BlockLayer)game.Grid.Layers.First(l => l is BlockLayer);
                    foreach (var Coord in this.GetAttackedCoords(game.Grid))
                        blockLayer.Block(Coord);

                    this.OnTurnDone();

                    if (_currentTurn <= this.RequiredTurns + 1)
                        selectedLayer.SetCurrentPointer(new Coordinates() { Y = game.Grid.Height / 2, X = game.Grid.Width / 2 });
                    break;
                default:
                    break;
            }
        }
        
        
    }
}