namespace HiddenFountain.Settings {
    internal static class JsonBlueprint {
        public static object CreateJsonBlueprint() {
            return new {
                ColorSettings = new {
                    MenuColor = "DarkYellow",
                    ChoiceColor = "DarkCyan",
                    WarningColor = "DarkRed",
                    SenseColor = "DarkGreen",
                    EnteringColor = "DarkMagenta",
                    HelpColor = "Blue",
                    SuccessColor = "Green",
                    FailureColor = "Red"
                },

                DifficultySettings = new {
                    WorldSize = new Dictionary<string, int> {
                    { "SmallWorld", 4 },
                    { "MediumWorld", 6 },
                    { "LargeWorld", 8 }
                },
                    PitCount = new Dictionary<string, int> {
                    { "SmallWorld", 3 },
                    { "MediumWorld", 4 },
                    { "LargeWorld", 6 }
                },
                    MaelstromCount = new Dictionary<string, int> {
                    { "SmallWorld", 0 },
                    { "MediumWorld", 1 },
                    { "LargeWorld", 2 }
                },
                    AmarokCount = new Dictionary<string, int> {
                    { "SmallWorld", 1 },
                    { "MediumWorld", 2 },
                    { "LargeWorld", 3 }
                }
                },

                CommandSettings = new Dictionary<string, object> {
                { "MoveUp", new { Aliases = new[] { "move up", "up", "u" } } },
                { "MoveDown", new { Aliases = new[] { "move down", "down", "d" } } },
                { "MoveLeft", new { Aliases = new[] { "move left", "left", "l" } } },
                { "MoveRight", new { Aliases = new[] { "move right", "right", "r" } } },
                { "ActivateFountain", new { Aliases = new[] { "activate fountain", "activate" } } },
                { "DeactivateFountain", new { Aliases = new[] { "deactivate fountain", "deactivate" } } },
                { "Help", new { Aliases = new[] { "help", "h" } } }
                }
            };
        }
    }
}
