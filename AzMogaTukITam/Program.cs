// See https://aka.ms/new-console-template for more information

using AzMogaTukITam.Model;

Game mainGame = new Game(5, 5, new GameContext());
mainGame.Grid.Layers.Add(new BaseLayer(mainGame.Grid));
mainGame.Start();