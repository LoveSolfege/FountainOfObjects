using HiddenFountain.Constants;
using HiddenFountain.Utilities;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace HiddenFountain.Settings {
    internal static class ConfigurationManager {
        public static IConfiguration Configuration { get; private set; }

        static ConfigurationManager() {
            try {
                LoadConfiguration();
            }
            catch(FileNotFoundException) {
                PrintJsonErrorAndCreateNew(GameStrings.JsonLoadError);
            }
            catch(JsonException) {
                PrintJsonErrorAndCreateNew(GameStrings.JsonLoadError);
            }
            catch (InvalidDataException) {
                PrintJsonErrorAndCreateNew(GameStrings.JsonLoadError);
            }
        }



        private static void LoadConfiguration() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        private static void CreateJsonIfNotFound() {
            var config = JsonBlueprint.CreateJsonBlueprint();

            string jsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("appsettings.json", jsonString);

            Utils.PrintColoredText(GameStrings.JsonCreated, ColorSettings.SuccessColor);

        }

        private static void PrintJsonErrorAndCreateNew(string message) {
            Utils.PrintColoredText(message, ColorSettings.FailureColor);
            Console.ReadKey();
            CreateJsonIfNotFound();
            Console.ReadKey();
            LoadConfiguration();
        }

    }
}
