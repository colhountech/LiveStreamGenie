using System.Text;

namespace LiveStreamGenie
{
    public class MyApplicationContext : ApplicationContext
    {
        
        private readonly SettingsForm _form1;
        private readonly AboutForm _form2;

        private Rectangle _form1Position;
        private Rectangle _form2Position;
        private int _formCount;

        private readonly Settings settings = new();

        public MyApplicationContext()
        {
            
            _formCount = 0;

            // Handle the ApplicationExit event to know when the application is exiting.
            Application.ApplicationExit += OnApplicationExit;


            try
            {
                if (new FileStream(Application.UserAppDataPath + "\\appdata.txt", FileMode.OpenOrCreate)
                    is FileStream userData)
                {
                    _userData = userData;
                }
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
            _form1.Disposed += Form_Disposed;
            _formCount++;

            _form2 = new AboutForm();
            _form2.Closed += OnFormClosed;
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

        #region Private Methods FormData Read/Write Methods

        private readonly FileStream? _userData;
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
                    _userData?.Seek(0, SeekOrigin.Begin);
                    _userData?.Write(dataToWrite, 0, dataToWrite.Length);
                    _userData?.Flush();

                    _userData?.SetLength(dataToWrite.Length);
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

            if (_userData?.Length is long length && new byte[length] is byte[] dataToRead)
            {
                try
                {
                    // Set the read position to the start of the file and read.
                    _userData?.Seek(0, SeekOrigin.Begin);
                    _userData?.Read(dataToRead, 0, dataToRead.Length);
                }
                catch (IOException e)
                {
                    // TODO: Log this error
                    string errorInfo = e.ToString();


                    MessageBox.Show("An error occurred while attempting to Read Form Data From File." +
                               "The error is:" + errorInfo);

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


        #endregion

        #region Handers

        private void OnFormClosed(object? sender, EventArgs e)
        {
            // When a form is closed, decrement the count of open forms.
            // When the count gets to 0, exit the app 
            _formCount--;
            if (_formCount == 0 && settings.QuitOnAllFormsClosed)
            {
                ExitThread();
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
                _userData?.Close();
            }
            catch { }
        }

        #endregion
    }
}