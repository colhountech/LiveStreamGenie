using System.Runtime.InteropServices;

namespace LiveStreamGenie
{
    internal static class Program
    {
        private static NotifyIcon notifyIcon;
        private static ContextMenuStrip cms;
        private static ApplicationContext context;
        private static System.Timers.Timer timer;   

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon("favicon.ico");
            notifyIcon.Text = "Genie";

            cms = new ContextMenuStrip();

            cms.Items.Add(new ToolStripMenuItem("Reconnect", null, new EventHandler(Reconnect_Click)));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add(new ToolStripMenuItem("Quit", null, new EventHandler(Quit_Click), "Quit"));

            notifyIcon.ContextMenuStrip = cms;
            notifyIcon.Visible = true;
            


            // Set up the timer
            timer = new System.Timers.Timer(10 * 1000); // 60 seconds
            timer.Elapsed += Timer_Elapsed;
            timer.Start();



            // Create an ApplicationContext and run a message loop
            // on the context.
            context = new ApplicationContext();
            Application.Run(context);

            // Hide notify icon on quit
            notifyIcon.Visible = false;
        }
        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
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
