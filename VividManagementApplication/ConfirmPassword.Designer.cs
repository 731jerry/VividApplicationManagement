namespace VividManagementApplication
{
    partial class ConfirmPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmPassword));
            this.label1 = new System.Windows.Forms.Label();
            this.confirmPasswordText = new System.Windows.Forms.TextBox();
            this.ConfirmPasswordButton = new ControlExs.QQButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "注意：请输入密码用以确认电子签名!";
            // 
            // confirmPasswordText
            // 
            this.confirmPasswordText.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.confirmPasswordText.Location = new System.Drawing.Point(14, 41);
            this.confirmPasswordText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.confirmPasswordText.Name = "confirmPasswordText";
            this.confirmPasswordText.Size = new System.Drawing.Size(232, 26);
            this.confirmPasswordText.TabIndex = 156;
            this.confirmPasswordText.UseSystemPasswordChar = true;
            // 
            // ConfirmPasswordButton
            // 
            this.ConfirmPasswordButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.ConfirmPasswordButton.Image = ((System.Drawing.Image)(resources.GetObject("ConfirmPasswordButton.Image")));
            this.ConfirmPasswordButton.Location = new System.Drawing.Point(155, 85);
            this.ConfirmPasswordButton.Name = "ConfirmPasswordButton";
            this.ConfirmPasswordButton.Size = new System.Drawing.Size(91, 28);
            this.ConfirmPasswordButton.TabIndex = 157;
            this.ConfirmPasswordButton.Text = "确认";
            this.ConfirmPasswordButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ConfirmPasswordButton.UseVisualStyleBackColor = true;
            this.ConfirmPasswordButton.Click += new System.EventHandler(this.ConfirmPasswordButton_Click);
            // 
            // ConfirmPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 128);
            this.Controls.Add(this.ConfirmPasswordButton);
            this.Controls.Add(this.confirmPasswordText);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfirmPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请确认密码...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox confirmPasswordText;
        private ControlExs.QQButton ConfirmPasswordButton;
    }
}