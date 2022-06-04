using System;

namespace AzMogaTukITam.Model
{
    public class PlayerInitLayer : LayerBase
    {
        private int currentTurn = 0;
        private bool inited = false;

        public PlayerInitLayer(Grid grid) : base(grid)
        {
        }

            this.ConsoleAction = SelectPosition;

        }

        public override int ZIndex { get; protected set; } = 1;
        public override DisplayValue DisplayValue { get; protected set; } = new DisplayValue() {Value = ' '};
        public override bool[,] Data { get; protected set; }
        public override int ConsolePriority { get; protected set; } = 0;
        public override int RequiredTurns { get; protected set; } = 2;
        public override Action<Game, ConsoleKeyInfo> ConsoleAction { get; protected set; }
        public override Action<Game> UpdateAction { get; protected set; }

        // Override props

        public void SelectPosition(Game game, ConsoleKeyInfo keyInfo)
        {
            var selectedLayer = (SelectedLayer)game.Grid.Layers.First(l => l is SelectedLayer);
            if (!inited)
            {
                game.DrawMessage($"Player {this.currentTurn + 1} is choosing a start position", 3000);
                selectedLayer.SetCurrentPointer(new Coordinates() { Y = game.Grid.Height / 2, X = game.Grid.Width / 2 });
                this.inited = true;
            }
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedLayer.MoveCurrentPointer(new Coordinates() {Y = -1});
                    break;
                case ConsoleKey.DownArrow:
                    selectedLayer.MoveCurrentPointer(new Coordinates() {Y = 1});
                    break;
                case ConsoleKey.RightArrow:
                    selectedLayer.MoveCurrentPointer(new Coordinates() {X = 1});
                    break;
                case ConsoleKey.LeftArrow:
                    selectedLayer.MoveCurrentPointer(new Coordinates() {X = -1});
                    break;
                case ConsoleKey.Enter:
                    //TODO: check if value is valid
                    currentTurn++;
                    game.Grid.Layers.Add(new PlayerLayer(game.Grid, new DisplayValue(){ Value = 'X' }, $"Player {this.currentTurn + 1}", selectedLayer.CurrentPointer));
                    //TODO: Block not valid values
                    this.OnTurnDone();
                    selectedLayer.SetCurrentPointer(new Coordinates(){ Y = game.Grid.Height / 2, X = game.Grid.Width / 2 });
                    if (currentTurn >= RequiredTurns)
                    {
                        this.RequiredTurns = 0;
                        selectedLayer.ClearCurrentPointer();
                    } else
                        game.DrawMessage($"Player {this.currentTurn + 1} is choosing a start position", 3000);
                    break;
                default:
                    break;
            }
        }
    }
}