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
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbAccount = new System.Windows.Forms.TextBox();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.productLinkLabel = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LoadCyy = new System.Windows.Forms.Button();
            this.regLinkLabel = new System.Windows.Forms.LinkLabel();
            this.findPwdLinkLabel = new System.Windows.Forms.LinkLabel();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbPassword
            // 
            this.tbPassword.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.tbPassword.Location = new System.Drawing.Point(123, 240);
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
            this.label2.Location = new System.Drawing.Point(83, 242);
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
            this.label1.Location = new System.Drawing.Point(83, 209);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "帐号";
            // 
            // cbAccount
            // 
            this.cbAccount.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cbAccount.Location = new System.Drawing.Point(123, 207);
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
            // productLinkLabel
            // 
            this.productLinkLabel.AutoSize = true;
            this.productLinkLabel.Font = new System.Drawing.Font("黑体", 9F);
            this.productLinkLabel.Location = new System.Drawing.Point(119, 339);
            this.productLinkLabel.Name = "productLinkLabel";
            this.productLinkLabel.Size = new System.Drawing.Size(163, 12);
            this.productLinkLabel.TabIndex = 16;
            this.productLinkLabel.TabStop = true;
            this.productLinkLabel.Text = "©唯达软件有限公司版权所有©";
            this.productLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.productLinkLabel_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(390, 183);
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // LoadCyy
            // 
            this.LoadCyy.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.LoadCyy.Image = ((System.Drawing.Image)(resources.GetObject("LoadCyy.Image")));
            this.LoadCyy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LoadCyy.Location = new System.Drawing.Point(123, 286);
            this.LoadCyy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoadCyy.Name = "LoadCyy";
            this.LoadCyy.Size = new System.Drawing.Size(157, 33);
            this.LoadCyy.TabIndex = 3;
            this.LoadCyy.Text = "  登      录";
            this.LoadCyy.UseVisualStyleBackColor = true;
            this.LoadCyy.Click += new System.EventHandler(this.LoadCyy_Click);
            // 
            // regLinkLabel
            // 
            this.regLinkLabel.AutoSize = true;
            this.regLinkLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.regLinkLabel.Location = new System.Drawing.Point(286, 211);
            this.regLinkLabel.Name = "regLinkLabel";
            this.regLinkLabel.Size = new System.Drawing.Size(56, 17);
            this.regLinkLabel.TabIndex = 18;
            this.regLinkLabel.TabStop = true;
            this.regLinkLabel.Text = "注册用户";
            this.regLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.regLinkLabel_LinkClicked);
            // 
            // findPwdLinkLabel
            // 
            this.findPwdLinkLabel.AutoSize = true;
            this.findPwdLinkLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.findPwdLinkLabel.Location = new System.Drawing.Point(286, 244);
            this.findPwdLinkLabel.Name = "findPwdLinkLabel";
            this.findPwdLinkLabel.Size = new System.Drawing.Size(56, 17);
            this.findPwdLinkLabel.TabIndex = 19;
            this.findPwdLinkLabel.TabStop = true;
            this.findPwdLinkLabel.Text = "找回密码";
            this.findPwdLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.findPwdLinkLabel_LinkClicked);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(266, 315);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 366);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.findPwdLinkLabel);
            this.Controls.Add(this.regLinkLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.productLinkLabel);
            this.Controls.Add(this.cbAccount);
            this.Controls.Add(this.LoadCyy);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1127);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.LinkLabel productLinkLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel regLinkLabel;
        private System.Windows.Forms.LinkLabel findPwdLinkLabel;
        private System.Windows.Forms.Button button1;
    }
}