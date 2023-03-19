namespace LiveStreamGenie
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            grpObs = new GroupBox();
            txtServer = new TextBox();
            lblServer = new Label();
            txtPassword = new MaskedTextBox();
            lblPassword = new Label();
            txtPort = new TextBox();
            lblPort = new Label();
            grpSystem = new GroupBox();
            chkMinimized = new CheckBox();
            grpObs.SuspendLayout();
            grpSystem.SuspendLayout();
            SuspendLayout();
            // 
            // grpObs
            // 
            grpObs.Controls.Add(txtServer);
            grpObs.Controls.Add(lblServer);
            grpObs.Controls.Add(txtPassword);
            grpObs.Controls.Add(lblPassword);
            grpObs.Controls.Add(txtPort);
            grpObs.Controls.Add(lblPort);
            grpObs.Dock = DockStyle.Top;
            grpObs.Location = new Point(0, 0);
            grpObs.Name = "grpObs";
            grpObs.Size = new Size(536, 271);
            grpObs.TabIndex = 0;
            grpObs.TabStop = false;
            grpObs.Text = "OBS Server Settings";
            // 
            // txtServer
            // 
            txtServer.Location = new Point(235, 68);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(200, 39);
            txtServer.TabIndex = 4;
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(19, 71);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(138, 32);
            lblServer.TabIndex = 3;
            lblServer.Text = "OBS Server:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(235, 184);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(200, 39);
            txtPassword.TabIndex = 1;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(19, 184);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(116, 32);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Password:";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(235, 123);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(200, 39);
            txtPort.TabIndex = 1;
            txtPort.Text = "4455";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(19, 126);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(135, 32);
            lblPort.TabIndex = 0;
            lblPort.Text = "Server Port:";
            // 
            // grpSystem
            // 
            grpSystem.Controls.Add(chkMinimized);
            grpSystem.Dock = DockStyle.Top;
            grpSystem.Location = new Point(0, 271);
            grpSystem.Name = "grpSystem";
            grpSystem.Size = new Size(536, 127);
            grpSystem.TabIndex = 5;
            grpSystem.TabStop = false;
            grpSystem.Text = "System Settings";
            // 
            // chkMinimized
            // 
            chkMinimized.AutoSize = true;
            chkMinimized.Location = new Point(39, 53);
            chkMinimized.Name = "chkMinimized";
            chkMinimized.Size = new Size(214, 36);
            chkMinimized.TabIndex = 5;
            chkMinimized.Text = "Start Minimized";
            chkMinimized.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(536, 441);
            Controls.Add(grpSystem);
            Controls.Add(grpObs);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            Text = "SettingsForm";
            FormClosing += SettingsForm_FormClosing;
            FormClosed += SettingsForm_FormClosed;
            Load += SettingsForm_Load;
            grpObs.ResumeLayout(false);
            grpObs.PerformLayout();
            grpSystem.ResumeLayout(false);
            grpSystem.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpObs;
        private MaskedTextBox txtPassword;
        private Label lblPassword;
        private TextBox txtPort;
        private Label lblPort;
        private TextBox txtServer;
        private Label lblServer;
        private GroupBox grpSystem;
        private CheckBox chkMinimized;
    }
}