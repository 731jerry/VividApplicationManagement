using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Net;

namespace VividManagementApplication
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }

        public Boolean isExiting = false;
        private Boolean isNewCreatedDataBase = false;

        private void Loading_Load(object sender, EventArgs e)
        {
            StartTimer.Enabled = true;
            if (!isExiting)
            {
                LoadingLabel.Text = "正在载入...请稍等...";
            }
            else
            {
                LoadingLabel.Text = "正在启动数据备份...请稍等...";
            }
        }

        private void StartTimer_Tick(object sender, EventArgs e)
        {
            StartTimer.Enabled = false;
            if (!isExiting)
            {
                SetLoadingProgressBar(0);
                // 更新在线
                LoadingLabel.Text = "正在更新在线状态...";
                DatabaseConnections.Connector.OnlineUpdateDataFromOriginalSQL("UPDATE users SET GZB_isonline = 1, GZB_lastlogontime = NOW() WHERE userid = '" + MainWindow.USER_ID + "'");
                SetLoadingProgressBar(5);

                // 检测未处理签单的个数
                LoadingLabel.Text = "正在检查远程签单消息...";
                SetLoadingProgressBar(27);
                updateRemoteSignUndealedCountCheck();
                List<List<String>> remoteSignList = updateRemoteSign();
                LoadingLabel.Text = "检查远程签单消息完成...";
                SetLoadingProgressBar(75);

                // 初始化数据库 备份数据库
                if (MainWindow.DEGREE > 0)
                {
                    LoadingLabel.Text = "正在同步数据库...";
                    SetLoadingProgressBar(90);

                    Boolean isLocalFileExists = File.Exists(MainWindow.LOCAL_DATABASE_LOCATION);
                    Boolean isRemoteFileExists = FormBasicFeatrues.GetInstence().UriExists(MainWindow.ONLINE_DATABASE_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX);
                    if (!isLocalFileExists && !isRemoteFileExists)
                    {
                        DatabaseConnections.Connector.LocalCreateDatabase(MainWindow.LOCAL_DATABASE_LOCATION);
                        isLocalFileExists = true;
                    }

                    DatabaseConnections.Connector.LocalClearTable("remoteSign");
                    if (remoteSignList.Count != 0)
                    {
                        foreach (List<String> item in remoteSignList)
                        {
                            DatabaseConnections.Connector.LocalReplaceIntoData("remoteSign", (new List<String>() { "Id", "fromGZBID", "toGZBID", "companyNickName", "isSigned", "signValue", "sendTime", "signTime", "refusedMessage" }).ToArray(), item.ToArray(), MainWindow.USER_ID);
                        }
                    }
                    SyncDataBase(isLocalFileExists, isRemoteFileExists);
                }
                else
                {
                    DatabaseConnections.Connector.LocalClearTable("remoteSign");
                    if (remoteSignList.Count != 0)
                    {
                        foreach (List<String> item in remoteSignList)
                        {
                            DatabaseConnections.Connector.LocalReplaceIntoData("remoteSign", (new List<String>() { "Id", "fromGZBID", "toGZBID", "companyNickName", "isSigned", "signValue", "sendTime", "signTime", "refusedMessage" }).ToArray(), item.ToArray(), MainWindow.USER_ID);
                        }
                    }
                    SetLoadingProgressBar(99);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else
            {
                // 初始化数据库 备份数据库
                if (MainWindow.DEGREE > 0)
                {
                    LoadingLabel.Text = "正在同步数据库...";
                    //SyncDatabaseStarter();
                    //SyncDataBase();
                }
            }


        }

        delegate void SetLoadingProgressBarCallback(int percentage);
        private void SetLoadingProgressBar(int percentage)
        {
            if (this.LoadingProgressBar.InvokeRequired)
            {
                SetLoadingProgressBarCallback d = new SetLoadingProgressBarCallback(SetLoadingProgressBar);
                this.Invoke(d, new object[] { percentage });
            }
            else
            {
                //Console.WriteLine(percentage);
                this.LoadingProgressBar.Value = percentage;
            }
        }

        #region 数据库

        // 同步数据库
        private void SyncDataBase(Boolean isLocalFileExists, Boolean isRemoteFileExists)
        {
            if (isLocalFileExists)
            {
                if (isRemoteFileExists)
                {
                    if (FormBasicFeatrues.GetInstence().getLocalFileSize(MainWindow.LOCAL_DATABASE_LOCATION) > 0)
                    {
                        if (FormBasicFeatrues.GetInstence().ifUpdateDatabasecheckLastModifiedTime(true))
                        {
                            File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Normal);
                            DownloadFile(MainWindow.ONLINE_DATABASE_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX, MainWindow.LOCAL_DATABASE_LOCATION);
                            File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                        }
                        else
                        {
                            //System.IO.File.Copy(MainWindow.LOCAL_DATABASE_LOCATION, MainWindow.LOCAL_DATABASE_LOCATION_COPY, true);
                            //UploadFile(MainWindow.LOCAL_DATABASE_LOCATION_COPY, MainWindow.ONLINE_DATABASE_FTP_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX);
                            UploadFile(MainWindow.LOCAL_DATABASE_LOCATION, MainWindow.ONLINE_DATABASE_FTP_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX);
                        }
                    }
                    else
                    {
                        File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Normal);
                        DownloadFile(MainWindow.ONLINE_DATABASE_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX, MainWindow.LOCAL_DATABASE_LOCATION);
                        File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                    }
                }
                else
                {
                    UploadFile(MainWindow.LOCAL_DATABASE_LOCATION, MainWindow.ONLINE_DATABASE_FTP_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX);
                }
            }
            else
            {
                if (isRemoteFileExists)
                {
                    DownloadFile(MainWindow.ONLINE_DATABASE_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX, MainWindow.LOCAL_DATABASE_LOCATION);
                    File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                }
            }
        }

        private void UploadFile(String fileNamePath, String uriString)
        {
            LoadingLabel.Text = "正在上传数据库...";
            try
            {
                WebClient client = new WebClient();
                Uri uri = new Uri(uriString);
                client.UploadProgressChanged += new UploadProgressChangedEventHandler(UploadProgressCallback);
                client.UploadFileCompleted += new UploadFileCompletedEventHandler(UploadFileCompleteCallback);
                //DatabaseConnections.Connector.LocalDbClose();
                client.UploadFileAsync(uri, "STOR", fileNamePath);
                // client.Proxy = WebRequest.DefaultWebProxy;
                //client.Proxy.Credentials = new NetworkCredential(ONLINE_FTP_USERNAME, ONLINE_FTP_PASSWORD, ONLINE_FTP_DOMAIN);
                //client.Dispose();
            }
            catch (Exception ee)
            {
                return;
            }
        }

        private void UploadProgressCallback(object sender, System.Net.UploadProgressChangedEventArgs e)
        {
            LoadingLabel.Text = "正在上传数据库..." + e.ProgressPercentage + "%";
        }

        private void UploadFileCompleteCallback(Object sender, UploadFileCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("数据已同步(上传)", "提示");
            }
            else
            {
                LoadingLabel.Text = "正在上传数据库完成...";
            }
            SetLoadingProgressBar(99);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void DownloadFile(String uriString, String fileNamePath)
        {
            LoadingLabel.Text = "正在下载数据库...";

            WebClient client = new WebClient();
            Uri uri = new Uri(uriString);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleteCallback);
            client.DownloadFileAsync(uri, fileNamePath);
            //client.DownloadFile(uri, fileNamePath);
            client.Dispose();
        }

        private void DownloadProgressCallback(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            LoadingLabel.Text = "正在下载数据库..." + e.ProgressPercentage + "%";
        }

        private void DownloadFileCompleteCallback(Object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("数据已同步(下载)", "提示");
            }
            else
            {
                File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                LoadingLabel.Text = "正在下载数据库完成...";
            }
            SetLoadingProgressBar(99);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion

        #region 远程签单
        private void updateRemoteSignUndealedCountCheckWithObject(Object obj)
        {
            updateRemoteSignUndealedCountCheck();
        }
        private void updateRemoteSignUndealedCountCheck()
        {
            List<List<String>> remoteSignUndealedList = DatabaseConnections.Connector.OnlineGetRowsDataById("gzb_remotesign", new List<String>() { "Id", "fromGZBID", "toGZBID", "companyNickName", "isSigned" }, "isSigned", "0", " AND (toGZBID ='" + MainWindow.USER_ID + "')");
            MainWindow.REMOTE_SIGN_UNDEALED_LIST_COUNT = remoteSignUndealedList.Count;
        }

        private List<List<String>> updateRemoteSign()
        {
            List<List<String>> remoteSignList = DatabaseConnections.Connector.OnlineGetRowsDataById("gzb_remotesign", new List<String>() { "Id", "fromGZBID", "toGZBID", "companyNickName", "isSigned", "signValue", "sendTime", "signTime", "refusedMessage" }, "toGZBID", MainWindow.USER_ID, " OR fromGZBID='" + MainWindow.USER_ID + "'");

            return remoteSignList;
        }
        #endregion

    }
}
