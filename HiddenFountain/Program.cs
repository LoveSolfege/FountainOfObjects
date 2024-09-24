using HiddenFountain.Constants;
using HiddenFountain.Controllers;
using HiddenFountain.Exceptions;
using HiddenFountain.Settings;
using HiddenFountain.Utilities;
using Microsoft.Extensions.Configuration;

namespace HiddenFountain {
    internal class Program {

        private static readonly IConfiguration config = Settings.ConfigurationManager.Configuration;

        static void Main(string[] args) {
            TryLoadCommands(config);
            ColorSettings.LoadColors(config);
            GameController game;
            while (true) {
                PrintMenuAndWait(ColorSettings.MenuColor);
                game = GameController.SelectGameDifficulty(config, ColorSettings.MenuColor);
                game.Run();
            }
        }

        static void PrintMenuAndWait(ConsoleColor color = ConsoleColor.Gray) {
            Utils.ClearConsolePlaceHeader(GameStrings.WholeMenu, color);
            Console.ReadKey();
        }

        static void TryLoadCommands(IConfiguration config) {
            try {
                CommandSettings.LoadCommands(config);
            }
            catch (DuplicateAliasException e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
