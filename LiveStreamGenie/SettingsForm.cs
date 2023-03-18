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

namespace LiveStreamGenie
{
    public partial class SettingsForm : Form
    {
        public Settings Settings { get; set; } = new Settings();
        public SettingsForm()
        {
            InitializeComponent();
            Icon = Resources.favicon;
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.ObsServer = txtServer.Text.Trim();
            Settings.ObsPort = txtPort.Text.Trim();
            Settings.ObsPass = txtPassword.Text.Trim();
            Settings.StartMinimized = chkMinimized.Checked;
        }
    }
}
