using HiddenFountain.Commands;
using HiddenFountain.Constants;
using HiddenFountain.Exceptions;
using HiddenFountain.Utilities;
using Microsoft.Extensions.Configuration;

namespace HiddenFountain.Settings {
    internal static class CommandSettings {
        public static void LoadCommands(IConfiguration config) {
            try {
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
            catch(NullReferenceException) {
                Utils.PrintColoredText(GameStrings.JsonCorrupted, ColorSettings.FailureColor);
                Console.ReadKey();
                CommandStorage.Commands = new Dictionary<string, Command> {
                    { "MoveUp", new Command { Aliases = ["up", "w"] } },
                    { "MoveDown", new Command { Aliases = ["down", "s"] } },
                    { "MoveLeft", new Command { Aliases = ["left", "a"] } },
                    { "MoveRight", new Command { Aliases = ["right", "d"] } },
                    { "EnableFountain", new Command { Aliases = ["enable"] } },
                    { "DisableFountain", new Command { Aliases = ["disable"] } },
                    { "Help", new Command { Aliases = ["help"] } }
                };

            }
            catch (DuplicateAliasException e) {
                Utils.PrintColoredText($"Error loading commands: {e.Message}\n{GameStrings.JsonEditOrDelete}", ColorSettings.FailureColor);
                Console.ReadKey(true);
                Environment.Exit(1);
            }
        }
    }
}
