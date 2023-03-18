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

    }
}
