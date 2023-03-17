namespace LiveStreamGenie
{
    partial class AboutForm
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
            pictureBox1 = new PictureBox();
            lblAbout = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = Properties.Resources.Live_Stream_Genie;
            pictureBox1.Location = new Point(0, 177);
            pictureBox1.Margin = new Padding(6, 6, 6, 6);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(563, 783);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lblAbout
            // 
            lblAbout.Dock = DockStyle.Top;
            lblAbout.Location = new Point(0, 0);
            lblAbout.Margin = new Padding(6, 0, 6, 0);
            lblAbout.Name = "lblAbout";
            lblAbout.Size = new Size(563, 177);
            lblAbout.TabIndex = 1;
            lblAbout.Text = "Live Streaming Genie (c) Copyright 2023 ColhounTech Limited";
            lblAbout.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(563, 960);
            Controls.Add(pictureBox1);
            Controls.Add(lblAbout);
            Margin = new Padding(6, 6, 6, 6);
            Name = "AboutForm";
            Text = "AboutForm";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Label lblAbout;
    }
}