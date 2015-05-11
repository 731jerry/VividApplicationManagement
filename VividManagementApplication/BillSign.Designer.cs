namespace VividManagementApplication
{
    partial class BillSign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BillSign));
            this.panel1 = new System.Windows.Forms.Panel();
            this.RefuseButton = new ControlExs.QQButton();
            this.OKButton = new ControlExs.QQButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SignPictureBox = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SignPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.RefuseButton);
            this.panel1.Controls.Add(this.OKButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(827, 51);
            this.panel1.TabIndex = 4;
            // 
            // RefuseButton
            // 
            this.RefuseButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.RefuseButton.Image = ((System.Drawing.Image)(resources.GetObject("RefuseButton.Image")));
            this.RefuseButton.Location = new System.Drawing.Point(723, 10);
            this.RefuseButton.Name = "RefuseButton";
            this.RefuseButton.Size = new System.Drawing.Size(91, 30);
            this.RefuseButton.TabIndex = 24;
            this.RefuseButton.Text = "拒绝";
            this.RefuseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.RefuseButton.UseVisualStyleBackColor = true;
            this.RefuseButton.Click += new System.EventHandler(this.RefuseButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.OKButton.Image = ((System.Drawing.Image)(resources.GetObject("OKButton.Image")));
            this.OKButton.Location = new System.Drawing.Point(567, 10);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(91, 30);
            this.OKButton.TabIndex = 23;
            this.OKButton.Text = "确认签单";
            this.OKButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(15, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "注意：如需签名请点击\"确认签单\"按钮!";
            // 
            // SignPictureBox
            // 
            this.SignPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SignPictureBox.BackColor = System.Drawing.Color.White;
            this.SignPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SignPictureBox.Location = new System.Drawing.Point(12, 70);
            this.SignPictureBox.Name = "SignPictureBox";
            this.SignPictureBox.Size = new System.Drawing.Size(827, 606);
            this.SignPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.SignPictureBox.TabIndex = 5;
            this.SignPictureBox.TabStop = false;
            // 
            // BillSign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 690);
            this.Controls.Add(this.SignPictureBox);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BillSign";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "电子签单";
            this.Load += new System.EventHandler(this.BillSign_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SignPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox SignPictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private ControlExs.QQButton RefuseButton;
        private ControlExs.QQButton OKButton;
    }
}