namespace VividManagementApplication
{
    partial class Loading
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
            this.components = new System.ComponentModel.Container();
            this.LoadingProgressBar = new System.Windows.Forms.ProgressBar();
            this.LoadingLabel = new System.Windows.Forms.Label();
            this.LoadingPictureBox = new System.Windows.Forms.PictureBox();
            this.StartTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadingProgressBar
            // 
            this.LoadingProgressBar.Location = new System.Drawing.Point(0, 271);
            this.LoadingProgressBar.Name = "LoadingProgressBar";
            this.LoadingProgressBar.Size = new System.Drawing.Size(464, 21);
            this.LoadingProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.LoadingProgressBar.TabIndex = 0;
            // 
            // LoadingLabel
            // 
            this.LoadingLabel.AutoSize = true;
            this.LoadingLabel.Location = new System.Drawing.Point(11, 252);
            this.LoadingLabel.Name = "LoadingLabel";
            this.LoadingLabel.Size = new System.Drawing.Size(41, 12);
            this.LoadingLabel.TabIndex = 1;
            this.LoadingLabel.Text = "label1";
            // 
            // LoadingPictureBox
            // 
            this.LoadingPictureBox.BackgroundImage = global::VividManagementApplication.Properties.Resources.loginpage;
            this.LoadingPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LoadingPictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.LoadingPictureBox.InitialImage = null;
            this.LoadingPictureBox.Location = new System.Drawing.Point(0, 0);
            this.LoadingPictureBox.Name = "LoadingPictureBox";
            this.LoadingPictureBox.Size = new System.Drawing.Size(464, 245);
            this.LoadingPictureBox.TabIndex = 2;
            this.LoadingPictureBox.TabStop = false;
            // 
            // StartTimer
            // 
            this.StartTimer.Interval = 500;
            this.StartTimer.Tick += new System.EventHandler(this.StartTimer_Tick);
            // 
            // Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 293);
            this.Controls.Add(this.LoadingPictureBox);
            this.Controls.Add(this.LoadingLabel);
            this.Controls.Add(this.LoadingProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Loading";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "载入中...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Loading_FormClosing);
            this.Load += new System.EventHandler(this.Loading_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar LoadingProgressBar;
        private System.Windows.Forms.Label LoadingLabel;
        private System.Windows.Forms.PictureBox LoadingPictureBox;
        private System.Windows.Forms.Timer StartTimer;
    }
}