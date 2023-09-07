using LiveStreamGenie.Properties;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication;
using OBSWebsocketDotNet.Types.Events;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;

namespace LiveStreamGenie
{
    public class OBSContext : ApplicationContext
    {
        private IStartupSettings _startupSettings;
        internal Settings Settings { get; set;  }

        private bool _obsConnected = false;
        private bool ObsConnected {
            get
            {
                return _obsConnected;
            }
            set
            {
                _obsConnected = value;

                if (_startupSettings.NotifyIcon is NotifyIcon notifyIcon)
                {
                    notifyIcon.Icon = _obsConnected ?  Resources.favicon : Resources.not_connected;
                }
            }
        }

        // OBS Settings
        protected static OBSWebsocket obs = new OBSWebsocket();

        #region Forms Settings

        private SettingsForm _settingsForm;
        private AboutForm _aboutForm;
        private static StringBuilder activityLog = new();

        #endregion

        public OBSContext( IStartupSettings startupSettings)
        {
            _startupSettings = startupSettings;
            Settings = startupSettings.LoadSettingsAsync().GetAwaiter().GetResult();
            
            Application.ApplicationExit += OnApplicationExit;

            InitSettings();
            InitAbout();

            if (!Settings?.StartMinimized ?? true)
            {
                _settingsForm?.Show();
                _aboutForm?.Show();
            }
            InitObs();
            ReconnectObs();

        }
    private void InitAbout()
        {
            _aboutForm = new AboutForm();
            _aboutForm.ActivityLog = activityLog.ToString();
        }

        private void InitSettings()
        {
           
            _settingsForm = new SettingsForm(Settings);
            // Load the settings from the Form when settings form is clsoed
            _settingsForm.FormClosed += _settingsForm_FormClosed;
        }
        private async void _settingsForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            await _startupSettings.SaveSettingsAsync(Settings);
            ReconnectObs();
        }


        private void InitObs()
        {
            /// setup obs
            obs.Connected += onConnect;
            obs.Disconnected += onDisconnect;
            obs.CurrentProgramSceneChanged += onSceneChanged;
            obs.RecordStateChanged += onRecordEvent;
        }

        private void onRecordEvent(object? sender, RecordStateChangedEventArgs e)
        {
            var state = e.OutputState;
            _startupSettings.NotifyIcon?.ShowBalloonTip(30, "OBS Record", $"{state}", ToolTipIcon.Warning);
        }

        public void ReconnectObs()
        {
            try
            {
                var url = $"ws://{Settings.ObsServer}:{Settings.ObsPort}";
                var pass = Settings.ObsPass ?? string.Empty;
                obs.ConnectAsync(url, pass);
            }
            catch (Exception ex)
            {
                LogActivity(ActivityType.ERROR, ex.Message);
            }
        }
        public void Settings_Click()
        {
            if (_settingsForm.IsDisposed)
            {
                InitSettings();
            }
            _settingsForm.Show();
        }

      
        public void About_Click()
        {
            if (_aboutForm.IsDisposed)
            {
                InitAbout();
            }
            _aboutForm.Show();
        }
        internal bool ChangeScene(string scene)
        {
            try
            {
                if (ObsConnected)
                {
                    obs.SetCurrentProgramScene(scene);
                }
            }
            catch (Exception ex)
            {
                _startupSettings.NotifyIcon?.ShowBalloonTip(3000, "OBS Change Scene", ex.Message, ToolTipIcon.Error);
                return false;
            }
            return true;
        }

        #region Callbacks

        private void onSceneChanged(object? sender, OBSWebsocketDotNet.Types.Events.ProgramSceneChangedEventArgs e)
        {
            var scene = e.SceneName;
            _startupSettings.NotifyIcon?.ShowBalloonTip(30, "OBS Scene", $"{scene}", ToolTipIcon.Info);
        }

        private void onDisconnect(object? sender, ObsDisconnectionInfo e)
        {
            ObsConnected = false;

            string reason =   e.DisconnectReason ?? e.ObsCloseCode.ToString();

            if (e?.WebsocketDisconnectionInfo?.Exception?.InnerException?.Message is string message)
            {
                reason += $": {message}";
            }
            _startupSettings.NotifyIcon?.ShowBalloonTip(3000, "OBS Disconnect", reason, ToolTipIcon.Error);
        }

        private void onConnect(object? sender, EventArgs e)
        {
            if (sender is OBSWebsocket ws && ws.IsConnected)
            {
                ObsConnected = true;
                _startupSettings.NotifyIcon?.ShowBalloonTip(3000, "OBS Connected", "👍", ToolTipIcon.Info);
            }
        }

        #endregion
        private void OnApplicationExit(object? sender, EventArgs e)
        {

            // When the application is exiting, write the application data to the
            // user file and close it.
        
        }

        internal void StopRecording()
        {
            if (ObsConnected)
            {

                obs.StopRecord();
            }
        }

        internal void StartRecording()
        {
            if (ObsConnected)
            {

                obs.StartRecord();
            }
        }

        internal void LogActivity(ActivityType severity, string message)
        {
            _startupSettings.NotifyIcon?.ShowBalloonTip(3000, "Activity", message, ToolTipIcon.Info);

            // Hack: Make this better
            activityLog.AppendLine(message);
            _aboutForm.ActivityLog = activityLog.ToString();
        }
      
    }
}