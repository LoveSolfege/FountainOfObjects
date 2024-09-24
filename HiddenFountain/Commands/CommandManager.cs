using HiddenFountain.Enums;

namespace HiddenFountain.Commands {
    internal static class CommandManager {

        public static PlayerAction ExecuteCommand(string input) {
            var normalizedInput = input.Trim().ToLower();

            // Find the command based on the aliases
            foreach (var command in CommandStorage.Commands) {
                if (command.Value.Aliases.Any(alias => alias.Equals(normalizedInput, StringComparison.OrdinalIgnoreCase))) {
                    GetCommand(command.Key);
                }
            }

            return PlayerAction.None;
        }


        private static PlayerAction GetCommand(string commandKey) {
            if (Enum.TryParse<PlayerAction>(commandKey, true, out var action)) {
                return action;
            }
            return PlayerAction.None;
        }
    }
}
