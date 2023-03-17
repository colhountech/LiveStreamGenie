using LiveStreamGenie.Properties;

namespace LiveStreamGenie
{
    public class Program
    {
        // System Tray Settings
        private static readonly NotifyIcon notifyIcon = new()
        {
            Text = "Live Stream Genie",
            Icon = Resources.favicon,
            ContextMenuStrip = new ContextMenuStrip {
                Items =
                {
                    new ToolStripMenuItem("Reconnect", null, new EventHandler(Reconnect_Click)),
                    new ToolStripMenuItem("Settings", null, new EventHandler(Settings_Click),"Setting"),
                    new ToolStripSeparator(),
                    new ToolStripMenuItem("About", null, new EventHandler(About_Click),"About"),
                    new ToolStripMenuItem("Quit", null, new EventHandler(Quit_Click), "Quit")
                }
            },
            Visible = true,
        };


        // Application Settings
        private static readonly System.Timers.Timer timer = new(60 * 1000); // 600 seconds - 10 Minutes
        private static readonly ApplicationContext context = new MyApplicationContext(notifyIcon);

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            InitialiseDemoTimer();

            // run a message loop on the context.
            Application.Run(context);

                // Wait until Application Quit

            // Hide notify icon on quit
            notifyIcon.Visible = false;
        }

        private static void InitialiseDemoTimer()
        {
            // Set up the timer
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }  

        private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            // Show the message
            notifyIcon.ShowBalloonTip(3000, "HeartBeat", "Still Alive", ToolTipIcon.Info);
        }

        static void Reconnect_Click(object? sender, System.EventArgs e)
        {
            if (context is MyApplicationContext myApplication)
            {
                myApplication.Reconnect();
            }
        }


        // TODO: Query PPT and look for Scene Name
        static void Scene_Click(string sceneName)
        {
            if (context is MyApplicationContext myApplication)
            {
                myApplication.ChangeScene(sceneName);
            }
        }

        static void Quit_Click(object? sender, System.EventArgs e)
        {
            // End application though ApplicationContext
            context.ExitThread();
        }
        static void Settings_Click(object? sender, EventArgs e)
        {
            if (context is MyApplicationContext myApplication)
            {
                myApplication.Settings_Click();
            }
        }
        static void About_Click(object? sender, EventArgs e)
        {
            if (context is MyApplicationContext myApplication)
            {
                myApplication.About_CLick();
            }
        }

    }
}