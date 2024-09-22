using Microsoft.Extensions.Configuration;

namespace FountainOfObjects.Settings {
    internal static class DifficultySettings {
        public static int GetCount(IConfiguration configuration, string settingType, string worldSize) {
            string path = $"DifficultySettings:{settingType}:{worldSize}";
            var value = configuration[path];
            if (int.TryParse(value, out int result)) {
                return result;
            }
            throw new ArgumentException($"Invalid setting type or world size. Path: {path}");
        }
    }
}
