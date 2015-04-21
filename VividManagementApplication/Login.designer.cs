namespace VividManagementApplication
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// 改动2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.LoadCyy = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbAccount = new System.Windows.Forms.TextBox();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // LoadCyy
            // 
            this.LoadCyy.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.LoadCyy.Image = ((System.Drawing.Image)(resources.GetObject("LoadCyy.Image")));
            this.LoadCyy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LoadCyy.Location = new System.Drawing.Point(96, 298);
            this.LoadCyy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoadCyy.Name = "LoadCyy";
            this.LoadCyy.Size = new System.Drawing.Size(193, 33);
            this.LoadCyy.TabIndex = 3;
            this.LoadCyy.Text = "  登      录";
            this.LoadCyy.UseVisualStyleBackColor = true;
            this.LoadCyy.Click += new System.EventHandler(this.LoadCyy_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.tbPassword.Location = new System.Drawing.Point(132, 250);
            this.tbPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbPassword.MaxLength = 12;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(157, 25);
            this.tbPassword.TabIndex = 1;
            this.tbPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Psw_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(94, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 15;
            this.label2.Text = "密码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(94, 218);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "帐号";
            // 
            // cbAccount
            // 
            this.cbAccount.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cbAccount.Location = new System.Drawing.Point(132, 216);
            this.cbAccount.MaxLength = 8;
            this.cbAccount.Name = "cbAccount";
            this.cbAccount.Size = new System.Drawing.Size(157, 25);
            this.cbAccount.TabIndex = 0;
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Interval = 500;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 372);
            this.Controls.Add(this.cbAccount);
            this.Controls.Add(this.LoadCyy);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1127);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button LoadCyy;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox cbAccount;
        private System.Windows.Forms.Timer UpdateTimer;
    }
}