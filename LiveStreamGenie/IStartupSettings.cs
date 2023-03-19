using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveStreamGenie
{
    public interface IStartupSettings
    {
        Task<Settings?> LoadSettingsAsync();
        Task SaveSettingsAsync(Settings settings);
        NotifyIcon? NotifyIcon { get; set; }
    }
}


