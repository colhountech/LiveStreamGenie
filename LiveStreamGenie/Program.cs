using Microsoft.Office.Interop.PowerPoint;
using LiveStreamGenie.Properties;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

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
        private static string _DefaultScene;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            InitialiseDemoTimer();


            Application ppt = new Application();
            ppt.SlideShowNextSlide += App_SlideShowNextSlide;
            // Occurs immediately before the transition to the next slide.
            // For the first slide, occurs immediately after the SlideShowBegin event.


            // run a message loop on the context.
            System.Windows.Forms.Application.Run(context);



            // Wait until Application Quit

            // Hide notify icon on quit
            notifyIcon.Visible = false;
        }

        private static void App_SlideShowNextSlide(SlideShowWindow Wn)
        {

            if (Wn != null)
            {
                Console.WriteLine($"Moved to Slide Number {Wn.View.Slide.SlideNumber}");
                //Text starts at Index 2 ¯\_(ツ)_/¯
                var note = String.Empty;
                try { note = Wn.View.Slide.NotesPage.Shapes[2].TextFrame.TextRange.Text; }
                catch { /*no notes*/ }

                bool sceneHandled = false;


                var notereader = new StringReader(note);
                string line;
                while ((line = notereader.ReadLine()) != null)
                {
                    if (line.StartsWith("OBS:"))
                    {
                        line = line.Substring(4).Trim();

                        if (!sceneHandled)
                        {
                            Console.WriteLine($"  Switching to OBS Scene named \"{line}\"");
                            try
                            {
                                sceneHandled = ChangeScene(line);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"  ERROR: {ex.Message.ToString()}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"  WARNING: Multiple scene definitions found.  I used the first and have ignored \"{line}\"");
                        }
                    }

                    if (line.StartsWith("OBSDEF:"))
                    {
                        _DefaultScene = line.Substring(7).Trim();
                        Console.WriteLine($"  Setting the default OBS Scene to \"{_DefaultScene}\"");
                    }

                    //if (line.StartsWith("**START"))
                    //{
                    //    OBS.StartRecording();
                    //}

                    //if (line.StartsWith("**STOP"))
                    //{
                    //    OBS.StopRecording();
                    //}

                    if (!sceneHandled)
                    {
                        ChangeScene();
                        Console.WriteLine($"  Switching to OBS Default Scene named \"{_DefaultScene}\"");
                    }
                }
            }
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
        static bool ChangeScene(string sceneName = "")
        {
            if (sceneName ==  "")
            {
                sceneName = _DefaultScene;
            }

            if (context is MyApplicationContext myApplication)
            {
                myApplication.ChangeScene(sceneName);
                return true;
            }
            return false;
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