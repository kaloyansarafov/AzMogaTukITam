// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using AzMogaTukITam.Model;

Console.WriteLine(@"
         _______                   _____                    _____                    _____                    _____                    _____          
        /::\    \                 /\    \                  /\    \                  /\    \                  /\    \                  /\    \         
       /::::\    \               /::\____\                /::\    \                /::\    \                /::\____\                /::\    \        
      /::::::\    \             /:::/    /               /::::\    \              /::::\    \              /::::|   |               /::::\    \       
     /::::::::\    \           /:::/    /               /::::::\    \            /::::::\    \            /:::::|   |              /::::::\    \      
    /:::/~~\:::\    \         /:::/    /               /:::/\:::\    \          /:::/\:::\    \          /::::::|   |             /:::/\:::\    \     
   /:::/    \:::\    \       /:::/    /               /:::/__\:::\    \        /:::/__\:::\    \        /:::/|::|   |            /:::/__\:::\    \    
  /:::/    / \:::\    \     /:::/    /               /::::\   \:::\    \      /::::\   \:::\    \      /:::/ |::|   |            \:::\   \:::\    \   
 /:::/____/   \:::\____\   /:::/    /      _____    /::::::\   \:::\    \    /::::::\   \:::\    \    /:::/  |::|   | _____    ___\:::\   \:::\    \  
|:::|    |     |:::|    | /:::/____/      /\    \  /:::/\:::\   \:::\    \  /:::/\:::\   \:::\    \  /:::/   |::|   |/\    \  /\   \:::\   \:::\    \ 
|:::|____|     |:::|____||:::|    /      /::\____\/:::/__\:::\   \:::\____\/:::/__\:::\   \:::\____\/:: /    |::|   /::\____\/::\   \:::\   \:::\____\
 \:::\   _\___/:::/    / |:::|____\     /:::/    /\:::\   \:::\   \::/    /\:::\   \:::\   \::/    /\::/    /|::|  /:::/    /\:::\   \:::\   \::/    /
  \:::\ |::| /:::/    /   \:::\    \   /:::/    /  \:::\   \:::\   \/____/  \:::\   \:::\   \/____/  \/____/ |::| /:::/    /  \:::\   \:::\   \/____/ 
   \:::\|::|/:::/    /     \:::\    \ /:::/    /    \:::\   \:::\    \       \:::\   \:::\    \              |::|/:::/    /    \:::\   \:::\    \     
    \::::::::::/    /       \:::\    /:::/    /      \:::\   \:::\____\       \:::\   \:::\____\             |::::::/    /      \:::\   \:::\____\    
     \::::::::/    /         \:::\__/:::/    /        \:::\   \::/    /        \:::\   \::/    /             |:::::/    /        \:::\  /:::/    /    
      \::::::/    /           \::::::::/    /          \:::\   \/____/          \:::\   \/____/              |::::/    /          \:::\/:::/    /     
       \::::/____/             \::::::/    /            \:::\    \               \:::\    \                  /:::/    /            \::::::/    /      
        |::|    |               \::::/    /              \:::\____\               \:::\____\                /:::/    /              \::::/    /       
        |::|____|                \::/____/                \::/    /                \::/    /                \::/    /                \::/    /        
         ~~                       ~~                       \/____/                  \/____/                  \/____/                  \/____/         
                                                                                                                                                      
");

Console.WriteLine("Welcome to Queens, please enter a number corresponding to one of the available options below");
Console.WriteLine("1. Single Player");
Console.WriteLine("2. Multi Player");
Console.WriteLine("3. Exit");
var selection = Console.ReadLine();
int[] gridSize = ReadGridSize();

switch (selection)
{
 case "1":
  Game mainGame = new Game(gridSize[0], gridSize[1], new GameContext());
  mainGame.Grid.Layers.Add(new BaseLayer(mainGame.Grid));
  mainGame.Grid.Layers.Add(new PlayerLayer(mainGame.Grid, new DisplayValue(){ DisplayBackground = ConsoleColor.DarkRed }, "Player 1"));
  mainGame.Grid.Layers.Add(new SelectedLayer(mainGame.Grid));
  mainGame.Start();
  break;
 case "2":
  break;
 case "3":
  break;
 default:
  Console.WriteLine("Invalid selection");
  break;
}


static int[] ReadGridSize()
{
  Console.WriteLine("Please enter grid size in the format 'height,width'");
  var gridSize = Console.ReadLine();
  var gridSizeArray = gridSize.Split(',');
  int height = Int32.Parse(gridSizeArray[0]);
  int width = Int32.Parse(gridSizeArray[1]);
  
  if (height < 3 || width < 3)
  {
   Console.WriteLine("Grid size must be at least 3x3");
   ReadGridSize();
  }

  return new[] {height, width};
}