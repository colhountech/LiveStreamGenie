using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Websocket.Client.Logging;

namespace LiveStreamGenie
{
    public class Settings
    {
        public bool StartMinimized { get; set; } = false;

        public string ObsServer { get; set; } = default!;

        public string ObsPort { get; set; } = default!;

        public string ObsPass { get; set; } = default!;

    }

}
