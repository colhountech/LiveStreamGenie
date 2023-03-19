using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveStreamGenie
{
    public class Settings
    {
        public bool QuitOnAllFormsClosed { get; set; }  = false;
        public bool StartMinimized { get; set;  } = false;

        public string ObsServer { get; set; } = default!;

        public string ObsPort { get; set; } = default!;

        public string ObsPass {  get; set; } = default!;

        #region constants
        // short names to stops me going crazy
        static readonly string cwd = Environment.CurrentDirectory;
        static readonly char _ = Path.AltDirectorySeparatorChar;
        static readonly string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static readonly string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        #endregion

    
        public static string GetSettingsPath()
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
    }
}
