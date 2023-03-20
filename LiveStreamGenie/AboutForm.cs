using LiveStreamGenie.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace LiveStreamGenie
{
    public partial class AboutForm : Form
    {
        public string ActivityLog
        {
            get { return richTextBox1.Text; }
            set { UpdateRichTextBox(value); }
        }

        private void UpdateRichTextBox(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action<string>(UpdateRichTextBox), text);
            }
            else
            {
                richTextBox1.Text += text;
            }
        }

        public AboutForm()
        {
            InitializeComponent();


        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }
    }
}
