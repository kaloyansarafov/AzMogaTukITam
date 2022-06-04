
using System;
using System.Linq;

namespace AzMogaTukITam.Model
{

    public class Game
    {

        public const int FRAME_TIME = 100;

        private bool gameEnded = false;
        private Action gameEndedAction = () => { };

        public Game(int gridHeight, int gridWidth, GameContext cont)
        {
            this.Context = cont;
            this.Grid = new Grid(gridHeight, gridWidth);
        }

        public GameContext Context { get; set; }

        public Grid Grid { get; set; }

        public void Update()
        {
            LayerBase[] consoleLayers = this.Grid.Layers.Where(x => x.RequiredTurns > 0).OrderBy(x => x.ConsolePriority).ThenBy(x => x.ZIndex).ThenBy(x => x.LayerID).ToArray();
            if (consoleLayers is not null)
            {
                foreach (LayerBase consoleLayer in consoleLayers)
                {
                    var currTurns = 0;
                    consoleLayer.TurnDone += (object temp, EventArgs args) => currTurns++;
                    while(currTurns != consoleLayer.RequiredTurns)
                    {
                        var input = Console.ReadKey();
                        consoleLayer.ConsoleAction?.Invoke(this, input);

                        // TODO: FIX

                        Console.SetCursorPosition(Console.CursorLeft - this.Grid.Width, Console.CursorTop - this.Grid.Height);
                        this.DrawGrid();
                        if (this.gameEnded) return;
                    }
                }
            }
            else
            {
                foreach (LayerBase layer in this.Grid.Layers) layer.UpdateAction?.Invoke(this);
            }
        }

        public void Start()
        {
            while (!this.gameEnded)
            {
                this.DrawGrid();
                this.Update();
                Console.SetCursorPosition(Console.CursorLeft - this.Grid.Width, Console.CursorTop - this.Grid.Height);
                Thread.Sleep(FRAME_TIME);
            }
            gameEndedAction?.Invoke();
        }

        public void EndGame(Action endAction)
        {
            this.gameEnded = true;
            this.gameEndedAction = endAction;
        }

        private void DrawGrid()
        {
            var tempGrid = this.Grid.ConstructGrid();
            for (int y = 0; y < tempGrid.GetLength(0); y++)
            {
                for (int x = 0; x < tempGrid.GetLength(1); x++)
                {
                    var dv = tempGrid[y, x];
                    Console.BackgroundColor = dv.DisplayBackground;
                    Console.ForegroundColor = dv.DisplayForeground;
                    Console.Write(dv.Value);
                }
                Console.WriteLine();
            }
               
        }

    }

}
