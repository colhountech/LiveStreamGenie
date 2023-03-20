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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            pictureBox1 = new PictureBox();
            lblAbout = new Label();
            richTextBox1 = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Image = Properties.Resources.Live_Stream_Genie;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(6);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(563, 960);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lblAbout
            // 
            lblAbout.BackColor = Color.White;
            lblAbout.Dock = DockStyle.Fill;
            lblAbout.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblAbout.Location = new Point(563, 0);
            lblAbout.Margin = new Padding(6, 0, 6, 0);
            lblAbout.Name = "lblAbout";
            lblAbout.Size = new Size(849, 176);
            lblAbout.TabIndex = 1;
            lblAbout.Text = "Live Streaming Genie";
            lblAbout.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // richTextBox1
            // 
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Dock = DockStyle.Bottom;
            richTextBox1.Location = new Point(563, 176);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(849, 784);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "Activity Logger";
            richTextBox1.Click += richTextBox1_Click;
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1412, 960);
            Controls.Add(lblAbout);
            Controls.Add(richTextBox1);
            Controls.Add(pictureBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(6);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            Text = "About";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;

        private RichTextBox richTextBox1;
        private Label lblAbout;
    }
}