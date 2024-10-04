using HiddenFountain.Constants;
using HiddenFountain.Controllers;
using HiddenFountain.Settings;
using HiddenFountain.Utilities;
using Microsoft.Extensions.Configuration;

namespace HiddenFountain {
    internal class Program {
        private static readonly IConfiguration config = Settings.ConfigurationManager.Configuration;

        static void Main(string[] args) {
            CommandSettings.LoadCommands(config);
            ColorSettings.LoadColors(config);
            GameController game;
            while (true) {
                PrintMenuAndWait();
                game = GameController.SelectGameDifficulty(config, ColorSettings.MenuColor);
                game.Run();
            }
        }

        static void PrintMenuAndWait() {
            Utils.ClearConsolePlaceHeader(GameStrings.WholeMenu, ColorSettings.MenuColor);
            Console.ReadKey();
        }
    }
}
