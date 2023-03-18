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
        private static readonly System.Timers.Timer heartbeat = new(60 * 1000); // 600 seconds - 10 Minutes
        private static readonly MyApplicationContext myApp = new MyApplicationContext(notifyIcon);
        private static string _DefaultScene = String.Empty;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            InitialiseHeartbeatTimer();

            Application ppt = new Application();
            ppt.SlideShowNextSlide += App_SlideShowNextSlide;
            // Occurs immediately before the transition to the next slide.
            // For the first slide, occurs immediately after the SlideShowBegin event.

            // run a message loop on the context.
            System.Windows.Forms.Application.Run(myApp);

            // Wait until Application Quit

            // Hide notify icon on quit
            notifyIcon.Visible = false;
        }

        private static void App_SlideShowNextSlide(SlideShowWindow Wn)
        {
            /*
             * System.Runtime.InteropServices.COMException
  HResult=0x80048240
  Message=SlideShowView (unknown member) : Invalid request.  No slide is currently in view.
  Source=<Cannot evaluate the exception source>
  StackTrace:
<Cannot evaluate the exception stack trace>

             */
            try
            {
                if (Wn != null)
                {
                    LogActivity(ActivityType.INFO, $"Moved to Slide Number {Wn.View.Slide.SlideNumber}");
                    //Text starts at Index 2 ¯\_(ツ)_/¯
                    var note = String.Empty;
                    try { note = Wn.View.Slide.NotesPage.Shapes[2].TextFrame.TextRange.Text; }
                    catch { /*no notes*/ }

                    bool sceneHandled = false;

                    var notereader = new StringReader(note);
                    while (notereader.ReadLine() is string line)
                    {
                        if (line.StartsWith("OBS:"))
                        {
                            line = line.Substring(4).Trim();

                            if (!sceneHandled)
                            {
                                LogActivity(ActivityType.INFO, $"Switching to OBS Scene named \"{line}\"");
                                try
                                {
                                    sceneHandled = ChangeScene(line);
                                }
                                catch (Exception ex)
                                {
                                    LogActivity(ActivityType.ERROR, $"{ex.Message}");
                                }
                            }
                            else
                            {
                                LogActivity(ActivityType.WARNING, $"Multiple scene definitions found.  I used the first and have ignored \"{line}\"");
                            }
                        }

                        if (line.StartsWith("OBSDEF:"))
                        {
                            _DefaultScene = line.Substring(7).Trim();
                            LogActivity(ActivityType.INFO, $"Setting the default OBS Scene to \"{_DefaultScene}\"");
                        }

                        if (line.StartsWith("**START"))
                        {
                            StartRecording();
                        }

                        if (line.StartsWith("**STOP"))
                        {
                            StopRecording();
                        }

                        if (!sceneHandled)
                        {
                            ChangeScene();
                            LogActivity(ActivityType.INFO, $"Switching to OBS Default Scene named \"{_DefaultScene}\"");
                        }
                    }
                }
            } catch (Exception ex)
            {
                LogActivity(ActivityType.ERROR, $"{ex}");
            }
            
        }
        static bool ChangeScene(string sceneName = "")
        {
            if (sceneName == "")
            {
                sceneName = _DefaultScene;
            }

            myApp.ChangeScene(sceneName);
            return true; // keep the same workflow as before

        }
        private static void StopRecording()
        {
            myApp.StopRecording();
        }
        private static void StartRecording()
        {
            myApp.StartRecording();
        }
        private static void InitialiseHeartbeatTimer()
        {
            // Set up the timer
            heartbeat.Elapsed += Timer_Elapsed;
            heartbeat.Start();
        }  

        private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            // Tell the User we are still here
            notifyIcon.ShowBalloonTip(3000, "HeartBeat", "Still Alive", ToolTipIcon.Info);
        }

        static void Reconnect_Click(object? sender, System.EventArgs e)
        {
            myApp.Reconnect();
        }

        private static void LogActivity(ActivityType severity, string message)
        {
            myApp.LogActivity(severity, message);
        }

        static void Quit_Click(object? sender, System.EventArgs e)
        {
            // End application though ApplicationContext
            myApp.ExitThread();
        }
        static void Settings_Click(object? sender, EventArgs e)
        {
            myApp.Settings_Click();
        }
        static void About_Click(object? sender, EventArgs e)
        {
            myApp.About_CLick();
        }
    }
}