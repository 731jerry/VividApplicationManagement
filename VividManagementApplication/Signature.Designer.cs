namespace VividManagementApplication
{
    partial class Signature
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
            this.SignPictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.uploadSignPicButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SignPictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SignPictureBox
            // 
            this.SignPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SignPictureBox.BackColor = System.Drawing.Color.White;
            this.SignPictureBox.Location = new System.Drawing.Point(12, 66);
            this.SignPictureBox.Name = "SignPictureBox";
            this.SignPictureBox.Size = new System.Drawing.Size(688, 302);
            this.SignPictureBox.TabIndex = 3;
            this.SignPictureBox.TabStop = false;
            this.SignPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.SignPictureBox_Paint);
            this.SignPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SignPictureBox_MouseDown);
            this.SignPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SignPictureBox_MouseMove);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.uploadSignPicButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Location = new System.Drawing.Point(12, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(688, 47);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(15, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "注意：电子签名将被用于远程签单使用！";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(593, 11);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "重写";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(489, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // uploadSignPicButton
            // 
            this.uploadSignPicButton.Location = new System.Drawing.Point(385, 12);
            this.uploadSignPicButton.Name = "uploadSignPicButton";
            this.uploadSignPicButton.Size = new System.Drawing.Size(75, 23);
            this.uploadSignPicButton.TabIndex = 3;
            this.uploadSignPicButton.Text = "上传签名图";
            this.uploadSignPicButton.UseVisualStyleBackColor = true;
            this.uploadSignPicButton.Click += new System.EventHandler(this.uploadSignPicButton_Click);
            // 
            // Signature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 378);
            this.Controls.Add(this.SignPictureBox);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Signature";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "电子签名";
            ((System.ComponentModel.ISupportInitialize)(this.SignPictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox SignPictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button uploadSignPicButton;
    }
}