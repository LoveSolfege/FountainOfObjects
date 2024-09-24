using Microsoft.Extensions.Configuration;

namespace HiddenFountain.Settings {
    internal static class ConfigurationManager {
        public static IConfiguration Configuration { get; private set; }

        static ConfigurationManager() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }
    }
}
