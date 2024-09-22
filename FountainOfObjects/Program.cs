using FountainOfObjects.Enums;
using FountainOfObjects.Settings;
using FountainOfObjects.Utilities;
using Microsoft.Extensions.Configuration;

namespace FountainOfObjects
{
    internal class Program {

        private static IConfiguration config = Settings.ConfigurationManager.Configuration;
        static void Main(string[] args) {
            
            ColorSettings.LoadColors(config);
            GameController game;
            while (true) {
                PrintMenuAndWait(ColorSettings.MenuColor);
                game = SelectGameDifficulty(ColorSettings.MenuColor);
                game.Run();
            }
        }

        static GameController SelectGameDifficulty(ConsoleColor color = ConsoleColor.Gray) {
            Utils.ClearConsolePlaceHeader("In order to start the game, please select game difficulty", color);
            Utils.PrintColoredText("[1] Easy, [2] Medium, [3] Hard", color);
            Difficulty diff = Difficulty.None;
            while (diff == Difficulty.None) {
                string choice = Utils.GetInput("Cave size: ", ColorSettings.ChoiceColor);
                switch (choice) {
                    case "1":
                        diff = Difficulty.Easy;
                        break;
                    case "2":
                        diff = Difficulty.Medium;
                        break;
                    case "3":
                        diff = Difficulty.Hard;
                        break;
                    default:
                        Utils.PrintColoredText("please select something between 1-2-3", ColorSettings.WarningColor);
                        continue;
                }
            }
            DifficultySettings.LoadDifficultySettings(config, diff);
            return new GameController(diff);
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
