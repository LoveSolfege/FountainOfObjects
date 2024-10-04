using HiddenFountain.Constants;
using HiddenFountain.Exceptions;
using HiddenFountain.Utilities;
using Microsoft.Extensions.Configuration;


namespace HiddenFountain.Settings {
    internal static class ColorSettings {
        public static ConsoleColor MenuColor { get; private set; } = ConsoleColor.DarkYellow;
        public static ConsoleColor ChoiceColor { get; private set; } = ConsoleColor.DarkCyan;
        public static ConsoleColor WarningColor { get; private set; } = ConsoleColor.DarkRed;
        public static ConsoleColor SenseColor { get; private set; } = ConsoleColor.DarkGreen;
        public static ConsoleColor EnteringColor { get; private set; } = ConsoleColor.DarkMagenta;
        public static ConsoleColor HelpColor { get; private set; } = ConsoleColor.Blue;
        public static ConsoleColor SuccessColor { get; private set; } = ConsoleColor.Green;
        public static ConsoleColor FailureColor { get; private set; } = ConsoleColor.Red;

        public static void LoadColors(IConfiguration configuration) {
            try {
                MenuColor = ParseColor(configuration["ColorSettings:MenuColor"]);
                ChoiceColor = ParseColor(configuration["ColorSettings:ChoiceColor"]);
                WarningColor = ParseColor(configuration["ColorSettings:WarningColor"]);
                SenseColor = ParseColor(configuration["ColorSettings:SenseColor"]);
                EnteringColor = ParseColor(configuration["ColorSettings:EnteringColor"]);
                HelpColor = ParseColor(configuration["ColorSettings:HelpColor"]);
                SuccessColor = ParseColor(configuration["ColorSettings:SuccessColor"]);
                FailureColor = ParseColor(configuration["ColorSettings:FailureColor"]);
            }
            catch (SettingsSectionCorruptedException e) {
                Utils.PrintColoredText($"Error loading colors: {e.Message}", ConsoleColor.Red);
                Console.ReadKey();
            }

        }

        private static ConsoleColor ParseColor(string colorName) {
            if (Enum.TryParse<ConsoleColor>(colorName, true, out var consoleColor)) {
                Console.WriteLine(consoleColor);
                return consoleColor;
            }
            else {
                throw new SettingsSectionCorruptedException(GameStrings.JsonBadColors);
            }
        }
    }
}
