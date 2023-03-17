using LiveStreamGenie.Properties;

namespace LiveStreamGenie
{
    public class Program
    {
        private static readonly NotifyIcon notifyIcon = new()
        {
            
            Text = "Live Stream Genie",
            Icon = Resources.favicon,
            ContextMenuStrip = new ContextMenuStrip {
                Items =
                {
                    new ToolStripMenuItem("Reconnect", null, new EventHandler(Reconnect_Click)),
                    new ToolStripSeparator(),
                    new ToolStripMenuItem("Quit", null, new EventHandler(Quit_Click), "Quit")
                }
            },
            Visible = true,
        };

        private static readonly System.Timers.Timer timer = new(15 * 1000); // 60 seconds
        private static readonly ApplicationContext context = new MyApplicationContext();

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Initialise();

            // run a message loopon the context.
            Application.Run(context);

            // Hide notify icon on quit
            notifyIcon.Visible = false;
        }

        private static void Initialise()
        {
            // Set up the timer
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            // Show the message
            notifyIcon.ShowBalloonTip(3000, "Title", "Message", ToolTipIcon.Info);
        }

        static void Reconnect_Click(object? sender, System.EventArgs e)
        {
            MessageBox.Show("Hello World!");
        }

        static void Quit_Click(object? sender, System.EventArgs e)
        {
            // End application though ApplicationContext
            context.ExitThread();
        }
    }
}
