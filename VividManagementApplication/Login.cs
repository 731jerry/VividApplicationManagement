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

using System.Net;

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
                DatabaseConnections.Connector.UserLogin(cbAccount.Text, tbPassword.Text);
            }
            catch
            {
                this.Text = windowText;
                MessageBox.Show("登录错误!", "错误");
                return;
            }

            if (!MainWindow.IS_PASSWORD_CORRECT)
            {
                this.Text = windowText;
                MessageBox.Show("帐号或者密码错误, 请与管理员联系!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (FormBasicFeatrues.GetInstence().ConvertDateTimeToTimestamp(MainWindow.EXPIRETIME) < FormBasicFeatrues.GetInstence().ConvertDateTimeToTimestamp(DateTime.Now))
                {
                    this.Text = windowText;
                    MessageBox.Show("您的账户以到期, 请与管理员联系!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (MainWindow.IS_USER_ONLINE)
                    {
                        this.Text = windowText;
                        MessageBox.Show("此用户已在某处登录, 因为用户数据隐私的问题, 此版本不允许重复登录, 如有错误, 请与管理员联系!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MainWindow.IS_LOGED_IN = true;
                        MainWindow.LOCAL_DATABASE_LOCATION = Environment.CurrentDirectory + "\\data\\" + MainWindow.USER_ID + "_data.db";
                        MainWindow.LOCAL_DATABASE_LOCATION_COPY = Environment.CurrentDirectory + "\\temp\\" + MainWindow.USER_ID + "_temp.gzb";
                        MainWindow.ONLINE_DATABASE_FILE_PREFIX = MainWindow.USER_ID + "_online.db"; ;
                        //this.Visible = false;
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
            //t.IsBackground = true;
            t.Start();
            t.DisableComObjectEagerCleanup();
            //t.Abort();
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
                // 检测是否有最新版本
                String updateVersion = "";
                String updateLog = "";

                /*
                List<String> updateVersionString = DatabaseConnections.Connector.OnlineGetOneRowDataById("config", new List<string>() { "configValue" }, "configKey", "GZB_update_version");
                List<String> updateVersionLogString = DatabaseConnections.Connector.OnlineGetOneRowDataById("config", new List<string>() { "configValue" }, "configKey", "GZB_update_version_log");
                List<String> updateAppURLString = DatabaseConnections.Connector.OnlineGetOneRowDataById("config", new List<string>() { "configValue" }, "configKey", "GZB_update_app_url");
              
                 updateVersion = updateVersionString[0];
                 updateLog = updateVersionLogString[0];
                MainWindow.UPDATE_APP_URL_DIR = updateAppURLString[0];
                */

                List<List<String>> updateListString = DatabaseConnections.Connector.OnlineGetRowsDataByCondition("config", new List<string>() { "id", "configValue" }, " WHERE id < 4");

                updateVersion = updateListString[0][1];
                updateLog = updateListString[1][1];
                MainWindow.UPDATE_APP_URL_DIR = updateListString[2][1];

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
            catch
            {
                MessageBox.Show("系统检测到您使用的是最新版本!", "提示");
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

        // 
        private void productLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.productLinkLabel.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.vividapp.net/");
        }

        private void regLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.regLinkLabel.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.vividapp.net/GZB_register.php");
        }

        private void findPwdLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.findPwdLinkLabel.LinkVisited = true;
            if (DialogResult.OK == MessageBox.Show("如果忘记密码，请与客服联系！", "提示", MessageBoxButtons.OKCancel))
            {
                System.Diagnostics.Process.Start("http://www.vividapp.net/GZB_product.php");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FTPRenameRemoteFile();
            //FTPtest();
            //RenameFileName();
            //string _ftpURL = "121.42.154.95";         //Host URL or address of the FTP server
            //string _UserName = MainWindow.ONLINE_FTP_USERNAME;             //User Name of the FTP server
            //string _Password = MainWindow.ONLINE_FTP_PASSWORD;          //Password of the FTP server
            //string _ftpDirectory = "Project/GZB/Users";      //The directory in FTP server where the file will be uploaded
            //string _FileName = "test1.csv";         //File name, which one will be uploaded
            //string _ftpDirectoryProcessed = "Done"; //The directory in FTP server where the file will be moved
            //MoveFile(_ftpURL, _UserName, _Password, _ftpDirectory, _FileName, _ftpDirectoryProcessed);
            //RenameTest();
            //RenameTest();
        }
        private void FTPtest() {
            String path = "ftp://www.vividapp.net/Project/GZB/Users/000003_online.db";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
            request.Credentials = new NetworkCredential(MainWindow.ONLINE_FTP_USERNAME, MainWindow.ONLINE_FTP_PASSWORD);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                MessageBox.Show(response.StatusDescription);
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    //Does not exist
                }
            }
        }

        private void FTPRenameRemoteFile()
        {
            String path = "ftp://www.vividapp.net/Project/GZB/Users/000003_online.db";
            //String path = "ftp://vividappftp:vividappftp@www.vividapp.net/Project/GZB/Users/00000004_data.txt";
            FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://www.vividapp.net/Project/GZB/Users/000003_online_bk.db"));
            req.Credentials = new NetworkCredential(MainWindow.ONLINE_FTP_USERNAME, MainWindow.ONLINE_FTP_PASSWORD);
            req.KeepAlive = false;
            req.Method = WebRequestMethods.Ftp.Rename; 
            req.UseBinary = true;
            req.RenameTo = MainWindow.USER_ID + "_online_" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss") + ".db"; 

            req.GetResponse().Close();

            FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
            resp.Close();

            //try
            //{
            //    using (FtpWebResponse response = (FtpWebResponse)ftpReq.GetResponse())
            //    {
            //        // response.Close();
            //    }
            //}
            //catch (Exception ex) { 
            //}
        }

        private void RenameFileName()
        {
            string currentFilename = "Project/GZB/Users/000003_online.db";
            string newFilename = MainWindow.USER_ID + "_online_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".db";
            FtpWebRequest reqFTP = null;
            Stream ftpStream = null;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + "121.42.154.95" + "/" + currentFilename));
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(MainWindow.ONLINE_FTP_USERNAME, MainWindow.ONLINE_FTP_PASSWORD);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                if (ftpStream != null)
                {
                    ftpStream.Close();
                    ftpStream.Dispose();
                }
                throw new Exception(ex.Message.ToString());
            }
        }

        public void MoveFile(string ftpURL, string UserName, string Password, string ftpDirectory, string ftpDirectoryProcessed, string FileName)
        {
            FtpWebRequest ftpRequest = null;
            FtpWebResponse ftpResponse = null;
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(ftpURL + "/" + ftpDirectory + "/" + FileName);
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                ftpRequest.RenameTo = ftpDirectoryProcessed + "/" + FileName;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RenameTest() {
            FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://www.vividapp.net/Project/GZB/Users/000003_online.db"));
            req.Credentials = new NetworkCredential(MainWindow.ONLINE_FTP_USERNAME, MainWindow.ONLINE_FTP_PASSWORD);
            req.KeepAlive = false;
            req.Method = WebRequestMethods.Ftp.Rename;  ///更改这行
            req.UseBinary = true;
            req.RenameTo = "000003_online_bk.db";  ///加入这行

            req.GetResponse().Close();

            FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
            resp.Close();
            //using (FtpWebResponse Response = (FtpWebResponse)req.GetResponse())
            //{
            //    long size = Response.ContentLength;
            //    using (Stream datastream = Response.GetResponseStream())
            //    {
            //        using (StreamReader sr = new StreamReader(datastream))
            //        {
            //            sr.ReadToEnd();
            //            sr.Close();
            //        }
            //        datastream.Close();
            //    }
            //    Response.Close();
            //}
        }
    }
}
