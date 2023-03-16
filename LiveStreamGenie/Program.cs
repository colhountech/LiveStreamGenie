namespace LiveStreamGenie
{
    internal static class Program
    {
        private static NotifyIcon notifyIcon;
        private static ContextMenuStrip cms;
        private static ApplicationContext context;

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

            // Create an ApplicationContext and run a message loop
            // on the context.
            context = new ApplicationContext();
            Application.Run(context);

            // Hide notify icon on quit
            notifyIcon.Visible = false;
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
