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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.LoadCyy = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbAccount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LoadCyy
            // 
            this.LoadCyy.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.LoadCyy.Image = ((System.Drawing.Image)(resources.GetObject("LoadCyy.Image")));
            this.LoadCyy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LoadCyy.Location = new System.Drawing.Point(96, 275);
            this.LoadCyy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoadCyy.Name = "LoadCyy";
            this.LoadCyy.Size = new System.Drawing.Size(193, 30);
            this.LoadCyy.TabIndex = 3;
            this.LoadCyy.Text = "  登      录";
            this.LoadCyy.UseVisualStyleBackColor = true;
            this.LoadCyy.Click += new System.EventHandler(this.LoadCyy_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.tbPassword.Location = new System.Drawing.Point(132, 231);
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
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(94, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 15;
            this.label2.Text = "密码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(94, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "帐号";
            // 
            // cbAccount
            // 
            this.cbAccount.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cbAccount.Location = new System.Drawing.Point(132, 199);
            this.cbAccount.Name = "cbAccount";
            this.cbAccount.Size = new System.Drawing.Size(157, 25);
            this.cbAccount.TabIndex = 0;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 343);
            this.Controls.Add(this.cbAccount);
            this.Controls.Add(this.LoadCyy);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Login_FormClosing);
            this.Load += new System.EventHandler(this.Login_Load);
            this.Resize += new System.EventHandler(this.Login_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button LoadCyy;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox cbAccount;
    }
}