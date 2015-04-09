using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using ControlExs;

namespace VividManagementApplication
{
    public partial class Login : FormEx
    {
        //public UserInfo userInfo { get; private set; }

        public Login()
        {
            InitializeComponent();
        }

        private void LoadCyy_Click(object sender, EventArgs e)
        {
            DatabaseConnections.GetInstence().UserLogin(cbAccount.Text, tbPassword.Text);

            if (!MainWindow.IS_PASSWORD_CORRECT)
            {
                MessageBox.Show("请与管理员联系", "帐号或者密码错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (FormBasicFeatrues.GetInstence().ConvertDateTimeToTimestamp(MainWindow.EXPIRETIME) < FormBasicFeatrues.GetInstence().ConvertDateTimeToTimestamp(DateTime.Now))
                {
                    MessageBox.Show("您的账户以到期, 请与管理员联系", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MainWindow.IS_LOGED_IN = true;
                    this.Visible = false;
                    (Owner as MainWindow).Visible = true;
                    //this.Close();
                }
            }
        }

        /* 选择保存的密码
        private void cbAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            string configFilePath = System.Environment.CurrentDirectory + @"\config\" + cbAccount.Text.Trim() + @".cyy";

            File.SetAttributes(configFilePath, FileAttributes.Normal);

            using (FileStream fs = File.OpenRead(configFilePath))
            {
                byte[] b = new byte[fs.Length];
                UTF8Encoding temp = new UTF8Encoding(true);
                StringBuilder sb = new StringBuilder();
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    sb.Append(temp.GetString(b));
                }

                string datas = sb.ToString();
                string[] datasArr = datas.Split('!');

                if (datasArr[1].Equals("?"))
                {
                    tbPassword.Text = "";
                }
                else
                {
                    tbPassword.Text = datasArr[1];
                }
            }
        }
        */
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!MainWindow.IS_LOGED_IN)
            {
                Application.Exit(e);
            }
        }

        private void Psw_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                LoadCyy.PerformClick();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void Login_Resize(object sender, EventArgs e)
        {
            this.Size = new Size(390, 343);
        }

    }
}
