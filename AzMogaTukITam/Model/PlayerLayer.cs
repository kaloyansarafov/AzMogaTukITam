using System;

namespace AzMogaTukITam.Model
{
    public class PlayerLayer : LayerBase
    {

        public PlayerLayer(Grid grid) : base(grid) {}

        public PlayerLayer(Grid grid, DisplayValue dv, string pn, Cordinates startPosition) : this(grid)
        {
            this.DisplayValue = dv;
            this.PlayerName = pn;
            this.Data[startPosition.Y, startPosition.X] = true;
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
