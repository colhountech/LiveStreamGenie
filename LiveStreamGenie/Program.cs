using Microsoft.Office.Interop.PowerPoint;
using LiveStreamGenie.Properties;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

namespace LiveStreamGenie
{
    public class Program
    {

        // Application Settings
        private static readonly System.Timers.Timer heartbeat = new(6 * 1000); // 600 seconds - 10 Minutes
        private static string _defaultScene = String.Empty;

        // Setup Dependencies for MyApp
        private static IStartupSettings StartupSettings = new StartupSettings();
        private static MyApplicationContext myApp = new MyApplicationContext(StartupSettings);

        [STAThread]
        static void Main()
        {
            // Call the `SetUnhandledExceptionMode` from Main, because it had to happen before the framework starts running.
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            System.Windows.Forms.Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ApplicationConfiguration.Initialize();

            StartupSettings.NotifyIcon = InitNotifyIcon();
            InitialiseHeartbeatTimer();
            InitPowerpoint();

            // run a message loop on the context.
            System.Windows.Forms.Application.Run(myApp);

            // Wait until Application Quit
            ;
        }

        #region Initializers

        private static void InitialiseHeartbeatTimer()
        {
            // Set up the heartbeat timer
            heartbeat.Elapsed += Timer_Elapsed;
            heartbeat.Start();
        }

        static NotifyIcon InitNotifyIcon()
        {
            var notifyIcon = new NotifyIcon
            {
                Text = "Live Stream Genie",
                Icon = Resources.favicon,
                ContextMenuStrip = new ContextMenuStrip
                {
                    Items =
                    {
                        new ToolStripMenuItem("Reconnect", null,  Reconnect_Click, "reconnect"),
                        new ToolStripMenuItem("Settings", null, Settings_Click, "settings"),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("About", null, About_Click, "about"),
                        new ToolStripMenuItem("Quit", null, Quit_Click, "quit")
                    }
                },
                Visible = true
            };
            return notifyIcon;
        }
        private static void InitPowerpoint()
        {
            Application ppt = new Application();
            ppt.SlideShowNextSlide += App_SlideShowNextSlide;
            // Occurs immediately before the transition to the next slide.
            // For the first slide, occurs immediately after the SlideShowBegin event.
        }

        #endregion

        #region PowerPoint Hander - This is where all the Magic Starts

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
                    if (Wn.View.Slide.SlideNumber is int slideNumber)
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
                                _defaultScene = line.Substring(7).Trim();
                                LogActivity(ActivityType.INFO, $"Setting the default OBS Scene to \"{_defaultScene}\"");
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
                                LogActivity(ActivityType.INFO, $"Switching to OBS Default Scene named \"{_defaultScene}\"");
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                LogActivity(ActivityType.ERROR, $"{ex}");
            }

        }
        #endregion 

        #region OBS Commands

        static bool ChangeScene(string sceneName = "")
        {

            if (sceneName == "")
            {
                sceneName = _defaultScene;
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

        #endregion

        #region Event Handlers

        private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (myApp.Settings is Settings settings &&  settings.DisableHeartBeat)
            {
                return;
            }
                // Tell the User we are still here
                StartupSettings.NotifyIcon?.ShowBalloonTip(3000, "HeartBeat", "Still Alive", ToolTipIcon.Info);
        }


        static void Reconnect_Click(object? sender, System.EventArgs e)
        {
            myApp.Reconnect();
        }


        static void Quit_Click(object? sender, System.EventArgs e)
        {
            if (StartupSettings.NotifyIcon is NotifyIcon i)
            {
                i.Visible = false;
            }
            // End application though ApplicationContext
            myApp.ExitThread();
        }
        static void Settings_Click(object? sender, EventArgs e)
        {
            myApp.Settings_Click();
        }
        static void About_Click(object? sender, EventArgs e)
        {
            myApp.About_Click();
        }
        #endregion

        #region Exception Handling and Logging
        private static void LogActivity(ActivityType severity, string message)
        {
            myApp.LogActivity(severity, message);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogActivity(ActivityType.ERROR, e.ExceptionObject?.ToString() ?? "Current Domain UnhandledException");
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogActivity(ActivityType.ERROR, e.Exception.Message);
        }
        #endregion
    }
}
