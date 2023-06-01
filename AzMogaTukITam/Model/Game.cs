﻿namespace AzMogaTukITam.Model
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
            // The game loop takes all layers which have required turns (players) and iterates over them
            // When its a player's turn, the consoleAction event for that player is invoked when a key is pressed.
            // When the player's movement logic is over, it raises the event TurnDone.
            // A method called TurnHandler is subscribed to the TurnDone event, and is responsible for incrementing the currentTurn var
            // (it also raises the currently unused UpdateAction event on each layer when called)
            // When the required amount of turns get accomplished, it moves onto the next layer with required turns

            LayerBase[] consoleLayers = this.Grid.Layers.Where(x => x.RequiredTurns > 0).OrderBy(x => x.ConsolePriority)
                .ThenBy(x => x.ZIndex).ThenBy(x => x.LayerID).ToArray();
            if (consoleLayers is not null)
            {
                foreach (LayerBase consoleLayer in consoleLayers)
                {
                    this._currentTurn = 0;
                    consoleLayer.TurnDone += TurnHandler;
                    while (this._currentTurn < consoleLayer.RequiredTurns)
                    {
                        var input = Console.ReadKey();
                        consoleLayer.ConsoleAction?.Invoke(this, input);

                        this.DrawGrid();
                        if (this._gameEnded) return;
                    }

                    consoleLayer.TurnDone -= TurnHandler;
                }
            }
        }

        private void TurnHandler(object temp, EventArgs args)
        {
            _currentTurn++;
            foreach (LayerBase layer in this.Grid.Layers) layer.UpdateAction?.Invoke(this);
        }

        public void Start()
        {
            Console.Clear();
            Console.CursorVisible = false;
            DrawMessage("Use Arrow Keys to move and Enter to place a queen!", 3000);
            while (!this._gameEnded)
            {
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
            var previousGrid = this.Grid.PreviousState;
            var tempGrid = this.Grid.ConstructGrid();
            int rows = tempGrid.GetLength(0);
            int cols = tempGrid.GetLength(1);


            if (previousGrid is null || previousGrid[0, 0] is null)
            {
                Console.WriteLine($".-{new string('-', (cols * 3) - 2)}-.");
                for (int y = 0; y < rows; y++)
                {
                    Console.Write("|");
                    for (int x = 0; x < cols; x++)
                    {
                        var currentValue = tempGrid[y, x];
                        Console.BackgroundColor = currentValue.DisplayBackground;
                        Console.ForegroundColor = currentValue.DisplayForeground;
                        Console.Write($" {currentValue.Value} ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine("|");
                }
                Console.WriteLine($".-{new string('-', (cols * 3) - 2)}-.");
            }
            else
            {
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < cols; x++)
                    {
                        var previousValue = previousGrid[y, x];
                        var currentValue = tempGrid[y, x];

                        if (previousValue.Value != currentValue.Value 
                            || previousValue.DisplayBackground != currentValue.DisplayBackground 
                            || previousValue.DisplayForeground != currentValue.DisplayForeground)
                        {
                            Console.SetCursorPosition((x * 3) + 1, y + 4);
                            Console.BackgroundColor = currentValue.DisplayBackground;
                            Console.ForegroundColor = currentValue.DisplayForeground;
                            Console.Write($" {currentValue.Value} ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
            }


            this.Grid.PreviousState = this.Grid.ConstructGrid();
        }

        public void DrawMessage(string message, int duration)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($".-{new string('-', message.Length)}-.");
            Console.WriteLine($"| {message} |");
            Console.WriteLine($".-{new string('-', message.Length)}-.");
            this.DrawGrid();
            Thread.Sleep(duration);

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.WriteLine(new string(' ', Console.WindowWidth));
        }
    }
}