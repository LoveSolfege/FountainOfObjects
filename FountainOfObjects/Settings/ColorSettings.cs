using Microsoft.Extensions.Configuration;


namespace FountainOfObjects.Settings {
    internal static class ColorSettings {
        public static ConsoleColor MenuColor { get; private set; }
        public static ConsoleColor ChoiceColor { get; private set; }
        public static ConsoleColor WarningColor { get; private set; }
        public static ConsoleColor SenseColor { get; private set; }

        public static void LoadColors(IConfiguration configuration) {
            MenuColor = ParseColor(configuration["ColorSettings:MenuColor"]);
            ChoiceColor = ParseColor(configuration["ColorSettingss:ChoiceColor"]);
            WarningColor = ParseColor(configuration["ColorSettings:WarningColor"]);
            SenseColor = ParseColor(configuration["ColorSettings:SenseColor"]);
        }

        private static ConsoleColor ParseColor(string colorName) {
            return Enum.TryParse<ConsoleColor>(colorName, true, out var consoleColor)
                ? consoleColor
                : ConsoleColor.Gray;
        }
    }
}
