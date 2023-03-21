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
        public Settings Settings { get; set; }
        public SettingsForm(Settings settings)
        {
            InitializeComponent();
            Settings = settings;



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
            Settings.DisableHeartBeat = chkDisableHearbeat.Checked;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            txtServer.Text = Settings.ObsServer;
            txtPort.Text = Settings.ObsPort;
            txtPassword.Text = Settings.ObsPass;
            chkMinimized.Checked = Settings.StartMinimized;
            chkDisableHearbeat.Checked = Settings.DisableHeartBeat;
        }
    }
}
