using HiddenFountain.Commands;
using HiddenFountain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace HiddenFountain.Settings {
    internal static class CommandSettings {
        public static void LoadCommands(IConfiguration config) {

            CommandStorage.Commands = config.GetSection("CommandSettings").Get<Dictionary<string, Command>>();

            var allAliases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var command in CommandStorage.Commands) {
                foreach (var alias in command.Value.Aliases) {
                    if (!allAliases.Add(alias)) {
                        throw new DuplicateAliasException($"Duplicate alias found: '{alias}'");
                    }
                }
            }
        }
    }
}
