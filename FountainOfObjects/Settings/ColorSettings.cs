using Microsoft.Extensions.Configuration;


namespace FountainOfObjects.Settings {
    internal static class ColorSettings {

        private static readonly Dictionary<string, ConsoleColor> _colors = new Dictionary<string, ConsoleColor>();
        public static void LoadColors(IConfiguration configuration) {
            foreach (var keyValue in configuration.GetSection("ConsoleColors").AsEnumerable()) {
                if (Enum.TryParse<ConsoleColor>(keyValue.Value, true, out var consoleColor)) {
                    _colors.Add(keyValue.Key, consoleColor);
                }
                else {
                    _colors.Add(keyValue.Key, ConsoleColor.Gray);
                }
            }
        }

        public static ConsoleColor MenuColor => GetColor("MenuColor");
        public static ConsoleColor ChoiceColor => GetColor("ChoiceColor");
        public static ConsoleColor WarningColor => GetColor("WarningColor");
        public static ConsoleColor SenseColor => GetColor("SenseColor");

        private static ConsoleColor GetColor(string colorName) {
            return _colors.TryGetValue(colorName, out var color) ? color : ConsoleColor.Gray;
        }
    }
}
