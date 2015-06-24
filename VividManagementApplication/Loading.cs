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

        private Boolean canClose = false;
        private int databaseSyncStart = 75;
        private int databaseSyncEnd = 95;

        private void Loading_Load(object sender, EventArgs e)
        {
            StartTimer.Enabled = true;
            if (!isExiting)
            {
                this.Text = "正在载入管账宝...";
                SetLoadingLabel("正在载入...请稍等...");
            }
            else
            {
                this.Text = "正在关闭软件...";
                SetLoadingLabel("正在关闭...请稍等...");
            }
        }

        private void StartTimer_Tick(object sender, EventArgs e)
        {
            StartTimer.Enabled = false;
            SetLoadingProgressBar(5);

            if (!isExiting) // 打开软件时
            {
                // 更新在线
                SetLoadingLabel("正在更新在线状态...");
                DatabaseConnections.Connector.OnlineUpdateDataFromOriginalSQL("UPDATE users SET GZB_isonline = 1, GZB_lastlogontime = NOW() WHERE userid = '" + MainWindow.USER_ID + "'");
                SetLoadingProgressBar(15);

                // 检测未处理签单的个数
                SetLoadingLabel("正在检查远程签单消息...");
                SetLoadingProgressBar(30);
                List<List<String>> remoteSignList = new List<List<string>>();
                try
                {
                    updateRemoteSignUndealedCountCheck();
                    remoteSignList = updateRemoteSign();
                }
                catch
                {
                    SetLoadingProgressBar(100);
                    canClose = true;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }

                SetLoadingLabel("检查远程签单消息完成...");
                SetLoadingProgressBar(60);

                SetLoadingLabel("正在检测数据库...");
                DatabaseConnections.Connector.LocalCreateDatabase(MainWindow.LOCAL_DATABASE_LOCATION);

                // 初始化数据库 备份数据库
                if (MainWindow.DEGREE > 0)
                {
                    SetLoadingLabel("正在同步数据库...");
                    SetLoadingProgressBar(databaseSyncStart);

                    Boolean isRemoteFileExists = FormBasicFeatrues.GetInstence().UriExists(MainWindow.ONLINE_DATABASE_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX);

                    if (isRemoteFileExists)
                    {
                        DownloadFileDirectly(MainWindow.ONLINE_DATABASE_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX, MainWindow.LOCAL_DATABASE_LOCATION);
                    }

                    DatabaseConnections.Connector.LocalClearTable("remoteSign");

                    if (remoteSignList.Count != 0)
                    {
                        foreach (List<String> item in remoteSignList)
                        {
                            DatabaseConnections.Connector.LocalReplaceIntoData("remoteSign", (new List<String>() { "Id", "fromGZBID", "toGZBID", "companyNickName", "isSigned", "signValue", "sendTime", "signTime", "refusedMessage" }).ToArray(), item.ToArray(), MainWindow.USER_ID);
                        }
                    }
                    SyncDataBase(isRemoteFileExists);
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
                    SetLoadingProgressBar(100);
                    canClose = true;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else // 关闭软件时
            {
                // 更新在线状态
                SetLoadingLabel("正在更新在线状态...");
                SetLoadingProgressBar(30);
                DatabaseConnections.Connector.OnlineUpdateDataFromOriginalSQL("UPDATE users SET GZB_isonline = 0 WHERE userid = '" + MainWindow.USER_ID + "'");

                // 初始化数据库 备份数据库
                if (MainWindow.DEGREE > 0)
                {
                    SetLoadingLabel("正在启动数据备份...请稍等..");
                    SetLoadingProgressBar(databaseSyncStart);
                    Boolean isRemoteFileExists = FormBasicFeatrues.GetInstence().UriExists(MainWindow.ONLINE_DATABASE_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX);

                    SyncDataBase(isRemoteFileExists);
                }
                else {
                    SetLoadingProgressBar(100);
                    canClose = true;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
                this.LoadingProgressBar.Value = percentage;
            }
        }

        delegate void SetLoadingLabelCallback(String str);
        private void SetLoadingLabel(String str)
        {
            this.Text = str;
            /*
            if (this.LoadingLabel.InvokeRequired)
            {
                SetLoadingLabelCallback d = new SetLoadingLabelCallback(SetLoadingLabel);
                this.Invoke(d, new object[] { str });
            }
            else
            {
                this.LoadingLabel.Text = str;
            }
             */
        }

        #region 数据库

        // 同步数据库
        private void SyncDataBase(Boolean isRemoteFileExists)
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

        private void UploadFile(String fileNamePath, String uriString)
        {
            SetLoadingLabel("正在上传数据库...");
            try
            {
                //FTPRenameRemoteFile(); // 重命名
                if (!FormBasicFeatrues.GetInstence().FTPRenameRemoteFile(MainWindow.ONLINE_DATABASE_FTP_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX, MainWindow.USER_ID + "_backup@" + DateTime.Now.ToString("yyyy-MM-dd&HH-mm-ss") + ".db")) {
                    canClose = true;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }

                using (WebClient client = new WebClient())
                {
                    Uri uri = new Uri(uriString);
                    client.UploadProgressChanged += new UploadProgressChangedEventHandler(UploadProgressCallback);
                    client.UploadFileCompleted += new UploadFileCompletedEventHandler(UploadFileCompleteCallback);
                    //DatabaseConnections.Connector.LocalDbClose();
                    client.UploadFileAsync(uri, "STOR", fileNamePath);
                    // client.Proxy = WebRequest.DefaultWebProxy;
                    //client.Proxy.Credentials = new NetworkCredential(ONLINE_FTP_USERNAME, ONLINE_FTP_PASSWORD, ONLINE_FTP_DOMAIN);
                    client.Dispose();
                }
            }
            catch
            {
                SetLoadingLabel("正在上传数据库错误，请在下次登录时重试...");
                canClose = true;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void UploadProgressCallback(object sender, System.Net.UploadProgressChangedEventArgs e)
        {
            SetLoadingLabel("正在上传数据库..." + ((e.ProgressPercentage * 2 > 100) ? 100 : (e.ProgressPercentage * 2)) + "%");
            SetLoadingProgressBar(databaseSyncStart + e.ProgressPercentage * (databaseSyncEnd - databaseSyncStart) / 100);
        }

        private void UploadFileCompleteCallback(Object sender, UploadFileCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("数据已同步(上传)", "提示");
            }
            else
            {
                SetLoadingLabel("正在上传数据库完成...");
            }
            SetLoadingProgressBar(100);
            canClose = true;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void DownloadFile(String uriString, String fileNamePath)
        {
            SetLoadingLabel("正在下载数据库...");
            try
            {
                using (WebClient client = new WebClient())
                {
                    Uri uri = new Uri(uriString);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleteCallback);
                    client.DownloadFileAsync(uri, fileNamePath);
                    //client.DownloadFile(uri, fileNamePath);
                    client.Dispose();
                }
            }
            catch
            {
                SetLoadingLabel("正在下载数据库错误，请在下次登录时重试...");
                canClose = true;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void DownloadProgressCallback(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            SetLoadingLabel("正在下载数据库..." + e.ProgressPercentage + "%");
            SetLoadingProgressBar(databaseSyncStart + e.ProgressPercentage * (databaseSyncEnd - databaseSyncStart) / 100);
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
                SetLoadingLabel("正在下载数据库完成...");
            }
            SetLoadingProgressBar(100);
            canClose = true;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void DownloadFileDirectly(String uriString, String fileNamePath)
        {
            try
            {
                using (WebClient wcClient = new WebClient())
                {
                    WebRequest webReq = WebRequest.Create(uriString);
                    WebResponse webRes = webReq.GetResponse();
                    long fileLength = webRes.ContentLength;

                    Stream srm = webRes.GetResponseStream();
                    StreamReader srmReader = new StreamReader(srm);

                    byte[] bufferbyte = new byte[fileLength];
                    int allByte = (int)bufferbyte.Length;
                    int startByte = 0;
                    while (fileLength > 0)
                    {
                        Application.DoEvents();
                        int downByte = srm.Read(bufferbyte, startByte, allByte);
                        if (downByte == 0) { break; };
                        startByte += downByte;
                        allByte -= downByte;
                    }

                    using (FileStream fs = new FileStream(fileNamePath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(bufferbyte, 0, bufferbyte.Length);
                    }

                    srm.Close();
                    srmReader.Close();
                }
            }
            catch { return; }
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

        private void Loading_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!canClose)
            {
                e.Cancel = true;
            }
        }

    }
}
