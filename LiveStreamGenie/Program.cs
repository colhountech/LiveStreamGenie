using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace LiveStreamGenie
{
    internal static class Program
    {
        private static NotifyIcon notifyIcon;
        private static ContextMenuStrip cms;
        private static ApplicationContext context;
        private static System.Timers.Timer timer;
        private static readonly Settings settings = new();


        class MyApplicationContext : ApplicationContext
        {
            private int _formCount;
            private readonly SettingsForm _form1;
            private readonly AboutForm _form2;

            private Rectangle _form1Position;
            private Rectangle _form2Position;

            private readonly FileStream _userData;


            private MyApplicationContext()
            {
                _formCount = 0;

                // Handle the ApplicationExit event to know when the application is exiting.
                Application.ApplicationExit += OnApplicationExit;

                try
                {
                    // Create a file that the application will store user specific data in.
                    _userData = new FileStream(Application.UserAppDataPath + "\\appdata.txt", FileMode.OpenOrCreate);
                }
                catch (IOException e)
                {
                    // Inform the user that an error occurred.
                    MessageBox.Show("An error occurred while attempting to show the application." +
                                    "The error is:" + e.ToString());

                    // Exit the current thread instead of showing the windows.
                    ExitThread();
                }

                // Create both application forms and handle the Closed event
                // to know when both forms are closed.
                _form1 = new SettingsForm();
                _form1.Closed += OnFormClosed;
                _form1.Closing += OnFormClosing;
                _form1.Disposed += Form_Disposed;
                _formCount++;

                _form2 = new AboutForm();
                _form2.Closed += OnFormClosed;
                _form2.Closing += OnFormClosing;
                _form2.Disposed += Form_Disposed;
                _formCount++;

                // Get the form positions based upon the user specific data.
                if (ReadFormDataFromFile())
                {
                    // If the data was read from the file, set the form
                    // positions manually.
                    _form1.StartPosition = FormStartPosition.Manual;
                    _form2.StartPosition = FormStartPosition.Manual;

                    _form1.Bounds = _form1Position;
                    _form2.Bounds = _form2Position;
                }

                // Show both forms.
                if (!settings.StartMinimized)
                {
                    _form1.Show();
                    _form2.Show();
                }
            }

            private void Form_Disposed(object? sender, EventArgs e)
            {
                if (
                    _form1.Bounds != _form1Position)
                {
                    _form1Position = _form1.Bounds;
                }

                if (_form2.Bounds != _form2Position)
                {
                    _form2Position = _form2.Bounds;
                }
               
            }

            private void OnApplicationExit(object? sender, EventArgs e)
            {
               

                // When the application is exiting, write the application data to the
                // user file and close it.
                WriteFormDataToFile();

                try
                {
                    // Ignore any errors that might occur while closing the file handle.
                    _userData.Close();
                }
                catch { }
            }

            private void OnFormClosing(object? sender, CancelEventArgs e)
            {
                // When a form is closing, remember the form position so it
                // can be saved in the user data file.
                // also need to track this if systray Exit choosen so also check in applicaiton.exit
              
                // Moved logic to FormDisposed because system tray exit doesn't catch Form Closing
            }

            private void OnFormClosed(object? sender, EventArgs e)
            {
                // When a form is closed, decrement the count of open forms.

                // When the count gets to 0, exit the app by calling
                // ExitThread().
                _formCount--;
                if (_formCount == 0 && settings.QuitOnAllFormsClosed)
                {
                    ExitThread();
                }
            }

            private bool WriteFormDataToFile()
            {
                // Write the form positions to the file.
                UTF8Encoding encoding = new();

                RectangleConverter rectConv = new();
                if (rectConv.ConvertToString(_form1Position) is string form1pos &&
                    rectConv.ConvertToString(_form2Position) is string form2pos
                    )
                {
                    byte[] dataToWrite = encoding.GetBytes("~" + form1pos + "~" + form2pos);

                    try
                    {
                        // Set the write position to the start of the file and write
                        _userData.Seek(0, SeekOrigin.Begin);
                        _userData.Write(dataToWrite, 0, dataToWrite.Length);
                        _userData.Flush();

                        _userData.SetLength(dataToWrite.Length);
                        return true;
                    }
                    catch { }
                }
                // An error occurred while attempting to write, return false.
                return false;
            }

            private bool ReadFormDataFromFile()
            {
                // Read the form positions from the file.
                UTF8Encoding encoding = new();
                string data;

                if (_userData.Length != 0)
                {
                    byte[] dataToRead = new byte[_userData.Length];

                    try
                    {
                        // Set the read position to the start of the file and read.
                        _userData.Seek(0, SeekOrigin.Begin);
                        _userData.Read(dataToRead, 0, dataToRead.Length);
                    }
                    catch (IOException e)
                    {
                        // TODO: Log this error
                        string errorInfo = e.ToString();
                        // An error occurred while attempt to read, return false.
                        return false;
                    }

                    // Parse out the data to get the window rectangles
                    data = encoding.GetString(dataToRead);

                    try
                    {
                        // Convert the string data to rectangles
                        RectangleConverter rectConv = new();

                        string form1pos = data[1..data.IndexOf("~", 1)];

                        if (rectConv.ConvertFromString(form1pos) is Rectangle position1)
                        {
                            _form1Position = position1;
                        }
                        
                        string form2pos = data[(data.IndexOf("~", 1) + 1)..];

                        if (rectConv.ConvertFromString(form2pos) is Rectangle position2)
                        {
                            _form2Position = position2;
                        }                      

                        return true;
                    }
                    catch
                    {
                        // Error occurred while attempting to convert the rectangle data.
                        // Return false to use default values.
                        return false;
                    }
                }
                else
                {
                    // No data in the file, return false to use default values.
                    return false;
                }
            }
            [STAThread]
            static void Main()
            {

                ApplicationConfiguration.Initialize();

                notifyIcon = new NotifyIcon
                {
                    Icon = new Icon("favicon.ico"),
                    Text = "Genie"
                };


                cms = new ContextMenuStrip();

                cms.Items.Add(new ToolStripMenuItem("Reconnect", null, new EventHandler(Reconnect_Click)));
                cms.Items.Add(new ToolStripSeparator());
                cms.Items.Add(new ToolStripMenuItem("Quit", null, new EventHandler(Quit_Click), "Quit"));

                notifyIcon.ContextMenuStrip = cms;
                notifyIcon.Visible = true;



                // Set up the timer
                timer = new System.Timers.Timer(15 * 1000); // 60 seconds
                timer.Elapsed += Timer_Elapsed;
                timer.Start();



                // Create an ApplicationContext and run a message loop
                // on the context.
                context = new MyApplicationContext();
                Application.Run(context);

                // Hide notify icon on quit
                notifyIcon.Visible = false;
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
}