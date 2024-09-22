using FountainOfObjects.Settings;
using FountainOfObjects.Utilities;

namespace FountainOfObjects
{
    internal class Program {
        static void Main(string[] args) {
            ColorSettings.LoadColors(ConfigurationManager.Configuration);
            GameController game;
            while (true) {
                PrintMenuAndWait(ColorSettings.MenuColor);
                game = GameSizeSelection(ColorSettings.MenuColor);
                game.Run();
            }
        }

        static GameController GameSizeSelection(ConsoleColor color = ConsoleColor.Gray) {
            Utils.ClearConsolePlaceHeader("In order to start the game, please select game difficulty", color);
            Utils.PrintColoredText("[1] Easy, [2] Medium, [3] Hard", color);
            
            while (true) {
                string choice = Utils.GetInput("Cave size: ", ColorSettings.ChoiceColor);
                switch (choice) {
                    case "1":
                        return new GameController(Difficulty.Easy);
                    case "2":
                        return new GameController(Difficulty.Medium);
                    case "3":
                        return new GameController(Difficulty.Hard);
                    default:
                        Utils.PrintColoredText("please select something between 1-2-3", ColorSettings.WarningColor);
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
