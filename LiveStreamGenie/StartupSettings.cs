using LiveStreamGenie.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LiveStreamGenie
{
    internal class StartupSettings : IStartupSettings
    {
        async Task<Settings?> IStartupSettings.LoadSettingsAsync()
        {
            var _path = GetSettingsPath();
            using (var fs = File.OpenRead(_path))
            {
                var options = new JsonSerializerOptions();
                // Not yet ready for NodaTime
                //.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

                return await JsonSerializer.DeserializeAsync<Settings>(fs, options);
            }
        }

        async Task IStartupSettings.SaveSettingsAsync(Settings settings)
        {
            var _path = GetSettingsPath();
            using (var fs = File.Open(_path, FileMode.Create))
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    // Keep settings files small. only write non-defaults
                    // DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                };
                // .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                await JsonSerializer.SerializeAsync<Settings>(fs, settings, options);
            }
        }

        private static string GetSettingsPath()
        {
            string jsonFile = "settings.json";
            string fullPath = $"{cwd}{_}{jsonFile}"; // Fallback (Debug) Location

            var di = new DirectoryInfo($"{appData}{_}{appName}");
            if (di.Exists)
            {
                fullPath = $"{appData}{_}{appName}{_}{jsonFile}"; // Deploy Location
            }
            return fullPath;
        }

        NotifyIcon? IStartupSettings.NotifyIcon { get; set;  }


        #region constants
        // short names to stops me going crazy
        static readonly string cwd = Environment.CurrentDirectory;
        static readonly char _ = Path.AltDirectorySeparatorChar;
        static readonly string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static readonly string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        #endregion
    }

}
