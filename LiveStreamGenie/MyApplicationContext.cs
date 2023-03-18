using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication;
using OBSWebsocketDotNet.Types.Events;
using System.Text;

namespace LiveStreamGenie
{
    public class MyApplicationContext : ApplicationContext
    {
        private static StringBuilder activityLog = new();
        // Inverted Dependencies
        private readonly NotifyIcon notifyIcon;

        // OBS Settings
        protected static OBSWebsocket obs = new OBSWebsocket();

        #region Forms Settings

        private SettingsForm _settingsForm;
        private AboutForm _aboutForm;

        private Settings settings = new();

        #endregion

        public MyApplicationContext(NotifyIcon notifyIcon)
        {
            this.notifyIcon = notifyIcon;
            Application.ApplicationExit += OnApplicationExit;

            InitSettings();
            InitAbout();

            if (!settings.StartMinimized)
            {
                _settingsForm?.Show();
                _aboutForm?.Show();
            }
            InitObs();
        }

        private void InitAbout()
        {
            _aboutForm = new AboutForm();
            _aboutForm.ActivityLog = activityLog.ToString();
        }

        private void InitSettings()
        {
            _settingsForm = new SettingsForm();
            // Load the settings into the Form
            _settingsForm.Settings = settings;
            _settingsForm.FormClosed += _settingsForm_FormClosed;
        }
        private void _settingsForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Reconnect();
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
            notifyIcon.ShowBalloonTip(30, "OBS Record", $"{state}", ToolTipIcon.Warning);
        }

        public void Reconnect()
        {
            try
            {
                var url = $"ws://{settings.ObsServer}:{settings.ObsPort}";
                var pass = settings.ObsPass ?? string.Empty;
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

      
        public void About_CLick()
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
                obs.SetCurrentProgramScene(scene);
            }
            catch (Exception ex)
            {
                notifyIcon.ShowBalloonTip(3000, "OBS Change Scene", ex.Message, ToolTipIcon.Error);
                return false;
            }
            return true;
        }

        #region Callbacks

        private void onSceneChanged(object? sender, OBSWebsocketDotNet.Types.Events.ProgramSceneChangedEventArgs e)
        {
            var scene = e.SceneName;
            notifyIcon.ShowBalloonTip(30, "OBS Scene", $"{scene}", ToolTipIcon.Info);
        }

        private void onDisconnect(object? sender, ObsDisconnectionInfo e)
        {

            string reason =   e.DisconnectReason ?? e.ObsCloseCode.ToString();

            if (e?.WebsocketDisconnectionInfo?.Exception?.InnerException?.Message is string message)
            {
                reason += message;
            }
            notifyIcon.ShowBalloonTip(3000, "OBS Disconnect", reason, ToolTipIcon.Error);
        }

        private void onConnect(object? sender, EventArgs e)
        {
            if (sender is OBSWebsocket ws && ws.IsConnected)
            {
                notifyIcon.ShowBalloonTip(3000, "OBS Connected", "👍", ToolTipIcon.Info);
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
            obs.StopRecord();
        }

        internal void StartRecording()
        {
            obs.StartRecord();
        }

        internal void LogActivity(ActivityType severity, string message)
        {
            activityLog.AppendLine(message);
            _aboutForm.ActivityLog = activityLog.ToString();
        }
    }
}