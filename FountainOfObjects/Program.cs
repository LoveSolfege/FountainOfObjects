using FountainOfObjects.Utilities;

namespace FountainOfObjects
{
    internal class Program {
        static void Main(string[] args) {
            GameController game;

            PrintMenuAndWait(menuColor);
            game = GameSizeSelection(menuColor);
            game.Run();
            
            
        }


        static GameController GameSizeSelection(ConsoleColor color = ConsoleColor.Gray) {
            Utils.ClearConsolePlaceHeader("In order to start the game, please select cave size", color);
            Utils.PrintColoredText("[1] Small (4x4), [2] Medium (6x6), [3] Large (8x8)", color);
            
            while (true) {
                string choice = Utils.GetInput("Cave size: ", choiceColor);
                switch (choice) {
                    case "1":
                        return new GameController(LevelSize.Small);
                    case "2":
                        return new GameController(LevelSize.Medium);
                    case "3":
                        return new GameController(LevelSize.Large);
                    default:
                        Console.WriteLine("perhaps you meant something else?");
                        continue;
                }
            }
        }

        static void PrintMenuAndWait(ConsoleColor color = ConsoleColor.Gray) {
            Utils.ClearConsolePlaceHeader("Welcome to the Fountain of objects game", color);
            Utils.PrintColoredText("You will have to find and enable Fountain of objects", color);
            Utils.PrintColoredText("Available controls are:", color);
            Utils.PrintColoredText("move [up, down, left, right], [enable, disable] fountain", color);
            Utils.PrintColoredText("Press any key to begin", color);
            Console.ReadKey();
        }
    }
}
