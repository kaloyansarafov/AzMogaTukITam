
namespace AzMogaTukITam.Model
{

    public class Game
    {

        public const int FRAME_TIME = 100;

        private int currentTurn = 0;
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
                    this.currentTurn = 0;
                    consoleLayer.TurnDone += TurnHandler;
                    while(currentTurn != consoleLayer.RequiredTurns - 1)
                    {
                        var input = Console.ReadKey();
                        consoleLayer.ConsoleAction?.Invoke(this, input);

                        // TODO: FIX

                        Console.Clear();
                        this.DrawGrid();
                        if (this.gameEnded) return;
                    }
                    consoleLayer.TurnDone -= TurnHandler;
                }
            }
            else
            {
                foreach (LayerBase layer in this.Grid.Layers) layer.UpdateAction?.Invoke(this);
            }
        }

        private void TurnHandler(object temp, EventArgs args){
            currentTurn ++;
        }

        public void Start()
        {
            Console.Clear();
            while (!this.gameEnded)
            {
                Console.Clear();
                this.DrawGrid();
                this.Update();
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
            int rows = tempGrid.GetLength(0);
            int cols = tempGrid.GetLength(1);

            Console.WriteLine($".-{new string('-', (cols * 2) - 1)}-.");
            for (int y = 0; y < rows; y++)
            {
                Console.Write("| ");
                for (int x = 0; x < cols; x++)
                {
                    var dv = tempGrid[y, x];
                    Console.BackgroundColor = dv.DisplayBackground;
                    Console.ForegroundColor = dv.DisplayForeground;
                    Console.Write($"{dv.Value} ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine($".-{new string('-', (cols * 2) - 1)}-.");
        }

        private void DrawMessage(string message, int duration)
        {
            
            Console.Clear();
            System.Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{message}{Environment.NewLine}{Environment.NewLine}");
            this.DrawGrid();
            Thread.Sleep(duration);
               
        }

    }

}
