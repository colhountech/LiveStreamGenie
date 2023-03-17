using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication;

namespace LiveStreamGenie
{
    public class MyApplicationContext : ApplicationContext
    {
        // Inverted Dependencies
        private readonly NotifyIcon notifyIcon;

        // OBS Settings
        protected static OBSWebsocket obs = new OBSWebsocket();

        #region Forms Settings

        private SettingsForm _settingsForm;
        private AboutForm _aboutForm;

        private readonly Settings settings = new();

        #endregion

        public MyApplicationContext(NotifyIcon notifyIcon)
        {
            this.notifyIcon = notifyIcon;
            Application.ApplicationExit += OnApplicationExit;

            _settingsForm = new SettingsForm();
            _aboutForm = new AboutForm();

            if (!settings.StartMinimized)
            {
                _settingsForm?.Show();
                _aboutForm?.Show();
            }
            InitObs();
        }
        private void InitObs()
        {
            /// setup obs
            obs.Connected += onConnect;
            obs.Disconnected += onDisconnect;
            obs.CurrentProgramSceneChanged += onSceneChanged;

            var url = "ws://raptor:4455";
            var password = "";

            obs.ConnectAsync(url, password);
        }
        public void Reconnect()
        {
            InitObs();
        }
        public void Settings_Click()
        {
            _settingsForm = new SettingsForm();
            _settingsForm.Show();
        }

        public void About_CLick()
        {
            _aboutForm = new AboutForm();
            _aboutForm.Show();

        }
        internal void ChangeScene(string scene)
        {
            try
            {
                obs.SetCurrentProgramScene(scene);
            }
            catch (Exception ex)
            {
                notifyIcon.ShowBalloonTip(3000, "OBS Disconnect", ex.Message, ToolTipIcon.Error);
            }
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
    }
}