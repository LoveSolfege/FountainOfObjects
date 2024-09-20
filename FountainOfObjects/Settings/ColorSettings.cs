namespace FountainOfObjects.Settings {
    internal static class ColorSettings {
        public static ConsoleColor MenuColor {  get; private set; }
        public static ConsoleColor ChoiceColor { get; private set; }
        public static ConsoleColor WarningColor { get; private set; }

        public static void LoadColors() {
            MenuColor = ParseColor(Properties.Settings.Default.MenuColor);
            ChoiceColor = ParseColor(Properties.Settings.Default.ChoiceColor);
            WarningColor = ParseColor(Properties.Settings.Default.WarningColor);
        }

        private static ConsoleColor ParseColor(string colorName) {
            return Enum.TryParse<ConsoleColor>(colorName, true, out var consoleColor)
                ? consoleColor
                : ConsoleColor.Gray;
        }

    }
}
