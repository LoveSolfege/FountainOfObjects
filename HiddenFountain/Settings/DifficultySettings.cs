using HiddenFountain.Constants;
using HiddenFountain.Enums;
using HiddenFountain.Utilities;
using HiddenFountain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace HiddenFountain.Settings
{
    internal static class DifficultySettings {

        public static int WorldSize { get; private set; }
        public static int PitCount { get; private set; }
        public static int MaelstromCount { get; private set; }
        public static int AmarokCount { get; private set; }


        public static void LoadDifficultySettings(IConfiguration configuration, Difficulty diff) {
            string worldSize = DifficultyToString[diff];
            try {
                WorldSize = GetCount(configuration, nameof(WorldSize), worldSize);
                PitCount = GetCount(configuration, nameof(PitCount), worldSize);
                MaelstromCount = GetCount(configuration, nameof(MaelstromCount), worldSize);
                AmarokCount = GetCount(configuration, nameof(AmarokCount), worldSize);
            }
            catch (ArgumentException e) {
                Utils.PrintColoredText(e.Message, ColorSettings.FailureColor);
                Console.ReadKey();
            }

        }

        private static int GetCount(IConfiguration configuration, string settingType, string worldSize) {
            string path = $"DifficultySettings:{settingType}:{worldSize}";
            var value = configuration[path];
            if (int.TryParse(value, out int result)) {
                return result;
            }
            throw new SettingsSectionCorruptedException(GameStrings.JsonCorrupted);
        }

        private static Dictionary<Difficulty, String> DifficultyToString = new() {
            { Difficulty.Easy, "SmallWorld"},
            { Difficulty.Medium, "MediumWorld" },
            { Difficulty.Hard, "LargeWorld" }
        };
    }
}
