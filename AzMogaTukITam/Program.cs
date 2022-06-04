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
Console.WriteLine("Please enter grid size in the format 'height,width'");
var gridSize = Console.ReadLine();
var gridSizeArray = gridSize.Split(',');

switch (selection)
{
 case "1":
  Game mainGame = new Game(int.Parse(gridSizeArray[0]), int.Parse(gridSizeArray[1]), new GameContext());
  mainGame.Grid.Layers.Add(new BaseLayer(mainGame.Grid));
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