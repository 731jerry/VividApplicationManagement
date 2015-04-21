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
using System.Threading;

namespace VividManagementApplication
{
    public partial class Login : Form
    {
        //public UserInfo userInfo { get; private set; }
        private String windowText = "";
        public Login()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;//这一行是关键 
        }

        private void LoadCyy_Click(object sender, EventArgs e)
        {
            this.Text = "正在登录...";
            try
            {
                DatabaseConnections.GetInstence().UserLogin(cbAccount.Text, tbPassword.Text);
            }
            catch (Exception exc)
            {
                this.Text = windowText;
                MessageBox.Show("登录错误!" + exc.Message, "错误");
                return;
            }

            if (!MainWindow.IS_PASSWORD_CORRECT)
            {
                MessageBox.Show("帐号或者密码错误, 请与管理员联系!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (FormBasicFeatrues.GetInstence().ConvertDateTimeToTimestamp(MainWindow.EXPIRETIME) < FormBasicFeatrues.GetInstence().ConvertDateTimeToTimestamp(DateTime.Now))
                {
                    MessageBox.Show("您的账户以到期, 请与管理员联系!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (MainWindow.IS_USER_ONLINE)
                    {
                        MessageBox.Show("此用户已在某处登录, 因为用户数据隐私的问题, 此版本不允许重复登录, 如有错误, 请与管理员联系!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void Psw_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                LoadCyy.PerformClick();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //UpdateTimer.Enabled = true;
            windowText = this.Text;

            Thread t = new Thread(new ParameterizedThreadStart(checkUpadteAppWithObj));
            t.Start();
        }

        private void checkUpadteAppWithObj(object obj)
        {
            checkUpdateApp();
        }

        private void checkUpdateApp()
        {
            LoadCyy.Enabled = false;
            this.Text = "正在检测更新...";
            try
            {
                //Console.WriteLine("1 " + DateTime.Now.ToString());
                // 检测是否有最新版本
                List<String> updateVersionString = DatabaseConnections.GetInstence().OnlineGetOneRowDataById("config", new List<string>() { "configValue" }, "configKey", "GZB_update_version");
                //Console.WriteLine("2 " + DateTime.Now.ToString());
                List<String> updateVersionLogString = DatabaseConnections.GetInstence().OnlineGetOneRowDataById("config", new List<string>() { "configValue" }, "configKey", "GZB_update_version_log");
                //Console.WriteLine("3 " + DateTime.Now.ToString());
                List<String> updateAppURLString = DatabaseConnections.GetInstence().OnlineGetOneRowDataById("config", new List<string>() { "configValue" }, "configKey", "GZB_update_app_url");
                //Console.WriteLine("4 " + DateTime.Now.ToString());

                String updateVersion = updateVersionString[0];
                String updateLog = updateVersionLogString[0];
                MainWindow.UPDATE_APP_URL_DIR = updateAppURLString[0];
                //string updateVersion = FormBasicFeatrues.GetInstence().getOnlineFile(MainWindow.UPDATE_VERSION_URL);

                if (!updateVersion.Equals(""))
                {
                    if (!MainWindow.CURRENT_APP_VERSION_NAME.Equals("") && !MainWindow.CURRENT_APP_VERSION_ID.Equals("-1"))
                    {
                        string localVersionString = "";
                        if (MainWindow.CURRENT_APP_VERSION_ID.Split('.').Length == 2)
                        {
                            localVersionString = MainWindow.CURRENT_APP_VERSION_ID + ".0";
                        }
                        else
                        {
                            localVersionString = MainWindow.CURRENT_APP_VERSION_ID;
                        }

                        if (FormBasicFeatrues.GetInstence().compareVersion(localVersionString, updateVersion, 2))
                        {
                            //string updateLog = FormBasicFeatrues.GetInstence().getOnlineFile(MainWindow.UPDATE_VERSION_LOG_URL);
                            if (!updateLog.Equals(""))
                            {
                                //this.Visible = false;
                                update ud = new update(MainWindow.UPDATE_APP_URL_DIR + MainWindow.CURRENT_APP_NAME + "-" + MainWindow.CURRENT_APP_VERSION_NAME + "v" + updateVersion + ".exe", updateVersion, updateLog);
                                ud.ShowDialog(this);
                            }
                        }
                    }
                }
            }
            catch (Exception exe)
            {
                MessageBox.Show("检测最新版本失败!" + exe.Message, "错误");
            }
            this.Text = windowText;
            LoadCyy.Enabled = true;
        }

        private void Login_Resize(object sender, EventArgs e)
        {
            this.Size = new Size(390, 343);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimer.Enabled = false;
            checkUpdateApp();
        }

    }
}
