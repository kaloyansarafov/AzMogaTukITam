using System;

namespace AzMogaTukITam.Model
{
    public class PlayerLayer : LayerBase
    {
        static HashSet<int> attackedRows = new HashSet<int>();
        static HashSet<int> attackedColumns = new HashSet<int>();
        static HashSet<int> attackedLeftDiagonals = new HashSet<int>();
        static HashSet<int> attackedRightDiagonals = new HashSet<int>();

        public PlayerLayer(Grid grid) : base(grid)
        {
            for (int y = 0; y < this.Data.GetLength(0); y++)
                for (int x = 0; x < this.Data.GetLength(1); x++)
                    this.Data[y, x] = true;
        }

        public PlayerLayer(Grid grid, DisplayValue dv, string pn) : this(grid)
        {
            this.DisplayValue = dv;
            this.PlayerName = pn;
        }

        private bool CanPlaceQueen(int row, int col, Grid grid)
        {
            var playerLayers = grid.Layers
                .Where(x => x is PlayerLayer && x.LayerID != this.LayerID) 
                as IEnumerable<PlayerLayer>;


            foreach (var layer in playerLayers)
            {
                var positionOccupied =
                attackedRows.Contains(row) ||
                attackedColumns.Contains(col) ||
                attackedLeftDiagonals.Contains(col - row) ||
                attackedRightDiagonals.Contains(col + row);

                if (positionOccupied)
                    return false;
            }

            return true;
        }

        private void MarkPositions(int row, int col)
        {
            attackedRows.Add(row);
            attackedColumns.Add(col);
            attackedLeftDiagonals.Add(col - row);
            attackedRightDiagonals.Add(col + row);
            this.Data[row, col] = true;
        }

        public override int ZIndex { get; protected set; } = 0;
        public override DisplayValue DisplayValue { get; protected set; } = new DisplayValue() { Value = 'X' };
        public override bool[,] Data { get; protected set; }
        public override int ConsolePriority { get; protected set; } = 0;
        public override int RequiredTurns { get; protected set; } = 0;
        public override Action<Game, ConsoleKeyInfo> ConsoleAction { get; protected set; }
        public override Action<Game> UpdateAction { get; protected set; }

        public string PlayerName { get; protected set; }

        // Override props

    }
}
