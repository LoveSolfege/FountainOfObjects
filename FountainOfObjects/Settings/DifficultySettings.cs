using FountainOfObjects.Enums;
using Microsoft.Extensions.Configuration;

namespace FountainOfObjects.Settings
{
    internal static class DifficultySettings {

        public static int WorldSize { get; private set; }
        public static int PitCount { get; private set; }
        public static int MaelstromCount { get; private set; }
        public static int AmarokCount { get; private set; }


        public static void LoadDifficultySettings(IConfiguration configuration, Difficulty diff) {
            string worldSize = DifficultyToString[diff];

            WorldSize = GetCount(configuration, nameof(WorldSize), worldSize);
            PitCount = GetCount(configuration, nameof(PitCount), worldSize);
            MaelstromCount = GetCount(configuration, nameof(MaelstromCount), worldSize);
            AmarokCount = GetCount(configuration, nameof(AmarokCount), worldSize);
        }

        private static int GetCount(IConfiguration configuration, string settingType, string worldSize) {
            string path = $"DifficultySettings:{settingType}:{worldSize}";
            var value = configuration[path];
            if (int.TryParse(value, out int result)) {
                return result;
            }
            throw new ArgumentException($"Invalid setting type or world size. Path: {path}");
        }

        private static Dictionary<Difficulty, String> DifficultyToString = new() {
            { Difficulty.Easy, "SmallWorld"},
            { Difficulty.Medium, "MediumWorld" },
            { Difficulty.Hard, "LargeWorld" }
        };
    }
}
