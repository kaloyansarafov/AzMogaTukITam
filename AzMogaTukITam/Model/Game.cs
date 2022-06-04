namespace AzMogaTukITam.Model
{
    public class Game
    {

        public const int FRAME_TIME = 1000;

        private int _currentTurn;
        private bool _gameEnded;
        private Action _gameEndedAction = () => { };

        public Game(int gridHeight, int gridWidth, GameContext cont)
        {
            this.Context = cont;
            this.Grid = new Grid(gridHeight, gridWidth);
        }

        public GameContext Context { get; set; }

        public Grid Grid { get; set; }

        public void Update()
        {
            LayerBase[] consoleLayers = this.Grid.Layers.Where(x => x.RequiredTurns > 0).OrderBy(x => x.ConsolePriority)
                .ThenBy(x => x.ZIndex).ThenBy(x => x.LayerID).ToArray();
            foreach (LayerBase layer in this.Grid.Layers) layer.UpdateAction?.Invoke(this);
            if (consoleLayers is not null)
            {
                foreach (LayerBase consoleLayer in consoleLayers)
                {
                    this._currentTurn = 0;
                    consoleLayer.TurnDone += TurnHandler;
                    while(this._currentTurn < consoleLayer.RequiredTurns)
                    {
                        var input = Console.ReadKey();
                        consoleLayer.ConsoleAction?.Invoke(this, input);

                        // TODO: FIX

                        Console.Clear();
                        this.DrawGrid();
                        foreach (LayerBase layer in this.Grid.Layers) layer.UpdateAction?.Invoke(this);
                        if (this._gameEnded) return;
                    }

                    consoleLayer.TurnDone -= TurnHandler;
                }
            }
        }

        private void TurnHandler(object temp, EventArgs args)
        {
            _currentTurn++;
        }

        public void Start()
        {
            Console.Clear();
            while (!this._gameEnded)
            {
                Console.Clear();
                this.DrawGrid();
                this.Update();
                Thread.Sleep(FRAME_TIME);
            }

            _gameEndedAction?.Invoke();
        }

        public void EndGame(Action endAction)
        {
            this._gameEnded = true;
            this._gameEndedAction = endAction;
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

        public void DrawMessage(string message, int duration)
        {
            Console.Clear();
            //create border around the message
            Console.WriteLine($".-{new string('-', message.Length + 2)}-.");
            Console.WriteLine($"| {message} |");
            Console.WriteLine($".-{new string('-', message.Length + 2)}-.");
            this.DrawGrid();
            Thread.Sleep(duration);
        }
    }
}