using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ControlExs;
using System.Drawing.Drawing2D;
using System.Web;
using System.Net;
using System.IO;
using System.Collections;
using MengYu.Image;

namespace VividManagementApplication
{
    public partial class MainWindow : Form
    {
        public static int CURRENT_TAB = 1;
        public static QQButton CURRENT_LIST_BUTTON;

        public static Boolean IS_PASSWORD_CORRECT = false;
        public static Boolean IS_LOGED_IN = false;
        public static Boolean IS_USER_ONLINE = false;
        public static String ID = "";
        public static String USER_ID = "";
        public static String PASSWORD_HASH = "";
        public static String WORKLOADS = "";
        public static String COMPANY_NAME = "";
        public static String COMPANY_NICKNAME = "";
        public static String COMPANY_OWNER = "";
        public static String ADDRESS = "";
        public static String BANK_NAME = "";
        public static String BANK_CARD = "";
        public static String PHONE = "";
        public static String FAX = "";
        public static String QQ = "";
        public static String EMAIL = "";
        public static DateTime ADDTIME = new DateTime();
        public static String NOTIFICATION = "";
        public static int DEGREE = 0; // 是否付费
        public static DateTime EXPIRETIME = new DateTime();
        public static String SIGNATURE = "";
        public static float COMPANY_BALANCE = 0f; // 公司结余暂存

        public static Bitmap SIGN_BITMAP = null;
        public static String SIGN_IMAGE_NAME = "sign";
        public static String SIGN_IMAGE_LOCATION = Environment.CurrentDirectory + "\\temp\\" + SIGN_IMAGE_NAME;

        public static String LOCAL_DATABASE_LOCATION = Environment.CurrentDirectory + "\\data\\" + USER_ID + "_data.db";
        public static String LOCAL_DATABASE_LOCATION_COPY = Environment.CurrentDirectory + "\\temp\\temp.gzb";
        public static String ONLINE_DATABASE_FTP_LOCATION_DIR = "ftp://vividappftp:vividappftp@www.vividapp.net/Project/GZB/Users/";//"ftp://qyw28051:cyy2014@qyw28051.my3w.com/products/caiYY/backup/"
        public static String ONLINE_DATABASE_LOCATION_DIR = "http://www.vividapp.net/Project/GZB/Users/";
        public static String ONLINE_DATABASE_BASIC_LOCATION_DIR = "/Project/GZB/Users/";
        public static String ONLINE_FTP_HOSTNAME = "121.42.154.95";
        public static String ONLINE_FTP_DOMAIN = "vividapp.net";
        public static String ONLINE_FTP_USERNAME = "vividappftp";
        public static String ONLINE_FTP_PASSWORD = "vividappftp";

        Microsoft.Win32.RegistryKey productKey;
        public static String PRODUCT_REG_KEY = @"Software\VividApp\GZB\";

        public static String CURRENT_APP_NAME = "管账宝"; //财盈盈账务管理系统
        public static String CURRENT_APP_VERSION_NAME = "精华版"; // 商贸版
        public static String CURRENT_APP_VERSION_ID = "-1"; // 1.0.1
        public static String UPDATE_APP_URL_DIR = "http://www.vividapp.net/Project/GZB/Update/"; // 最新版本app地址
        public static String UPDATE_VERSION_URL = "http://www.vividapp.net/Project/GZB/Update/version.txt"; // 更新app版本的文件地址
        public static String UPDATE_VERSION_LOG_URL = "http://www.vividapp.net/Project/GZB/Update/versionlog.txt"; // 更新app版本记录的文件地址

        //System.Timers.Timer updateDataTimersTimer;
        System.Timers.Timer updateRemoteSignTimer;
        System.Timers.Timer lablTextChangeTimer; // 状态栏信息修改Timer

        string onlineDataBaseFilePrefix;

        public MainWindow()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;//这一行是关键 
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            this.Visible = false;
            //Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            String loginWindowLabel = "登录";

            #region 软件版本
            try
            {
                productKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(PRODUCT_REG_KEY);
                CURRENT_APP_NAME = productKey.GetValue("DisplayName").ToString();
                CURRENT_APP_VERSION_NAME = productKey.GetValue("VersionName").ToString(); ;
                CURRENT_APP_VERSION_ID = productKey.GetValue("DisplayVersion").ToString();

                loginWindowLabel = CURRENT_APP_NAME + "(" + CURRENT_APP_VERSION_NAME + ")v" + CURRENT_APP_VERSION_ID;

                this.Text = CURRENT_APP_NAME + " " + CURRENT_APP_VERSION_NAME + "v" + CURRENT_APP_VERSION_ID;
            }
            catch { }

            #endregion

            #region 登录
            Login loginWindow = new Login();
            this.Visible = false;
            loginWindow.Text = loginWindowLabel;
            loginWindow.ShowDialog(this);
            #endregion

            if (MainWindow.IS_LOGED_IN)
            {
                #region 窗体用户信息初始化
                lbUserName.Text = COMPANY_NICKNAME + "(" + USER_ID + ")";
                onlineDataBaseFilePrefix = USER_ID + "_online.db";

                LOCAL_DATABASE_LOCATION = Environment.CurrentDirectory + "\\data\\" + USER_ID + "_data.db";
                #endregion

                #region 初始化数据库 备份数据库
                if (DEGREE > 0)
                {
                    //Thread t = new Thread(new ParameterizedThreadStart(InitLocalDataBaseWithObject));
                    //t.Start();
                    //t.DisableComObjectEagerCleanup();
                    SyncDatabaseStarter();
                }
                else
                {
                    backupData.Enabled = false;
                }

                DatabaseConnections.GetInstence().LocalCreateDatabase(LOCAL_DATABASE_LOCATION);

                #endregion

                #region 更新远程签单数据
                // 检测未处理签单的个数
                Thread tt = new Thread(new ParameterizedThreadStart(updateRemoteSignUndealedCountCheckWithObject));
                tt.Start();
                tt.DisableComObjectEagerCleanup();

                //remoteSignTimer.Enabled = true;
                updateRemoteSignTimer = new System.Timers.Timer(45000);
                updateRemoteSignTimer.Elapsed += new System.Timers.ElapsedEventHandler(updateRemoteSignTimer_Elapsed);//到达时间的时候执行事件；  
                updateRemoteSignTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；  
                updateRemoteSignTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；  
                if (this.IsDisposed)
                {
                    updateRemoteSignTimer.Stop();
                }
                #endregion

                #region 窗体滚动通知初始化
                tmrShows.Enabled = true;

                lblSHOWS.Text = NOTIFICATION;
                lblSHOWS2.Text = NOTIFICATION;

                if (lblSHOWS.Width < notificationPanel.Width)
                {
                    lblSHOWS2.Left = lblSHOWS.Left + notificationPanel.Width;
                }
                else
                {
                    lblSHOWS2.Left = lblSHOWS.Left + lblSHOWS.Width;
                }
                #endregion

                #region 初始化客户列表
                listCxButton.PerformClick();
                #endregion

                #region 初始化登录信息
                lbExpireTime.Text = "至" + EXPIRETIME.ToLongDateString() + "止";
                keepOnlineTimer.Enabled = true;
                TimeSpan ts = EXPIRETIME - ADDTIME;
                if (DEGREE == 0)
                {
                    UserDegreeLabel.Text = "免费版用户";
                }
                else
                {
                    int disDays = ts.Days / 365;
                    UserDegreeLabel.Text = "VIP" + (disDays + 1).ToString();
                }

                UserDegreeLabel.Location = new Point(lbUserName.Location.X + lbUserName.Size.Width + 5, UserDegreeLabel.Location.Y);
                #endregion

                lablTextChangeTimer = new System.Timers.Timer(5000);

                // 状态栏通知
                notifyIcon.Visible = true;
                notifyIcon.Text = "管账宝(" + MainWindow.USER_ID + ")";
                SetNotifyIcon(currentImageIndex);

                #region 初始化企业基本信息设置
                if (COMPANY_NAME.Equals("") || SIGNATURE.Equals("") || COMPANY_NICKNAME.Equals("") || PHONE.Equals("") || ADDRESS.Equals("") || BANK_NAME.Equals("") || BANK_CARD.Equals(""))
                {
                    Setting st = new Setting();
                    st.Text = "初始化用户信息";
                    st.ShowDialog();
                }
                #endregion

            }
            else
            {
                this.Visible = false;
                this.Close();
            }
        }

        // 初始化本地数据库
        private void InitLocalDataBaseWithObject(object obj)
        {
            InitLocalDataBase();
        }
        private void InitLocalDataBase()
        {
            if (File.Exists(MainWindow.LOCAL_DATABASE_LOCATION))
            {
                if (UriExists(ONLINE_DATABASE_LOCATION_DIR + onlineDataBaseFilePrefix))
                {
                    DownloadFileWithNotice();
                }
                else
                {
                    UploadFileWithNotice("");
                }
            }
            else
            {
                if (UriExists(ONLINE_DATABASE_LOCATION_DIR + onlineDataBaseFilePrefix))
                {
                    MainPanel.Enabled = false;
                    File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Normal);
                    DownloadFile(ONLINE_DATABASE_LOCATION_DIR + onlineDataBaseFilePrefix, LOCAL_DATABASE_LOCATION);
                    File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                    MainPanel.Enabled = true;
                }
                else
                {
                    DatabaseConnections.GetInstence().LocalCreateDatabase(LOCAL_DATABASE_LOCATION);
                    File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                    UploadFileWithNotice("初次同步, ");
                }
            }
        }

        // 同步数据库
        private void SyncDatabaseStarter()
        {
            Thread t = new Thread(new ParameterizedThreadStart(SyncDatabaseWithObject));
            t.Start();
            t.DisableComObjectEagerCleanup();
        }

        private void SyncDatabaseWithObject(object obj)
        {
            if (File.Exists(MainWindow.LOCAL_DATABASE_LOCATION))
            {
                if (UriExists(ONLINE_DATABASE_LOCATION_DIR + onlineDataBaseFilePrefix))
                {
                    if (getLocalFileSize(LOCAL_DATABASE_LOCATION) > 0)
                    {
                        if (ifUpdateDatabasecheckLastModifiedTime(true))
                        {
                            File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Normal);
                            DownloadFile(ONLINE_DATABASE_LOCATION_DIR + onlineDataBaseFilePrefix, LOCAL_DATABASE_LOCATION);
                            File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                        }
                        else
                        {
                            System.IO.File.Copy(LOCAL_DATABASE_LOCATION, LOCAL_DATABASE_LOCATION_COPY, true);
                            UploadFile(LOCAL_DATABASE_LOCATION_COPY, ONLINE_DATABASE_FTP_LOCATION_DIR + onlineDataBaseFilePrefix);
                        }
                    }
                }
                else
                {
                    UploadFile(LOCAL_DATABASE_LOCATION, ONLINE_DATABASE_FTP_LOCATION_DIR + onlineDataBaseFilePrefix);
                }
            }
            else
            {
                if (UriExists(ONLINE_DATABASE_LOCATION_DIR + onlineDataBaseFilePrefix))
                {
                    MainPanel.Enabled = false;
                    File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Normal);
                    DownloadFile(ONLINE_DATABASE_LOCATION_DIR + onlineDataBaseFilePrefix, LOCAL_DATABASE_LOCATION);
                    File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                    MainPanel.Enabled = true;
                }
                else
                {
                    DatabaseConnections.GetInstence().LocalCreateDatabase(LOCAL_DATABASE_LOCATION);
                    File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                    UploadFile(LOCAL_DATABASE_LOCATION, ONLINE_DATABASE_FTP_LOCATION_DIR + onlineDataBaseFilePrefix);
                }
            }
        }

        // MainWindow 窗口固定
        private void MainWindow_Resize(object sender, EventArgs e)
        {
            this.Size = new Size(1061, 688);
        }

        private void CreateDetailedWindow()
        {
            DetailedInfo di = new DetailedInfo();
            di.isNewWindow = true;
            //di.ShowIcon = false;
            di.Text = "新建";
            di.ShowDialog();

            /*
            // 是否存在客户和商品信息
            if (CURRENT_TAB != 1 && CURRENT_TAB != 2)
            {
                int SpCount = DatabaseConnections.GetInstence().LocalGetCountOfTable("goods", "");
                int CxCount = DatabaseConnections.GetInstence().LocalGetCountOfTable("clients", "");
                if (CxCount > 0 && SpCount > 0)
                {

                }
                DetailedInfo di = new DetailedInfo();
                di.isNewWindow = true;
                //di.ShowIcon = false;
                di.Text = "新建";
                di.ShowDialog();
            }
            else
            {
                MessageBox.Show("请先输入客户和客户信息!", "提示");
            }
            */ 
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            if (MainDataGridView.Rows.Count > 0)
            {
                if (CURRENT_TAB == 7)
                {
                    ViewRemoteSignDetail(this.MainDataGridView.SelectedRows[0].Cells[0].Value.ToString());
                }
                else
                {
                    DetailedInfo di = new DetailedInfo();
                    di.ItemId = this.MainDataGridView.SelectedRows[0].Cells[0].Value.ToString();
                    di.isNewWindow = false;
                    di.Text = "查看" + di.ItemId;
                    di.ShowDialog();
                }
            }
        }

        private void ViewRemoteSignDetail(String remoteSignId)
        {
            if (!remoteSignId.Equals(""))
            {
                BillSign bs = new BillSign();
                bs.isSendRequest = false;
                bs.remoteSignId = remoteSignId;
                String[] resultArray = DatabaseConnections.GetInstence().LocalGetOneRowDataById("remoteSign", new String[] { "fromGZBID", "toGZBID", "companyNickName", "signValue", "isSigned" }, "Id", remoteSignId);
                if (resultArray.Length > 0)
                {
                    bs.gzbIDStirng = resultArray[0];
                    bs.companyNameStirng = resultArray[2];
                    bs.Text = resultArray[2] + "发来的电子签单";
                    bs.signImage = FormBasicFeatrues.GetInstence().Base64StringToImage(FormBasicFeatrues.GetInstence().DecompressString(FormBasicFeatrues.GetInstence().DecompressString(resultArray[3])));
                    bs.isSigned = resultArray[4].Equals("0") ? false : true;
                }
                if (bs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    bs.Close();
                    checkRemoteSignList();
                    refeshButton.PerformClick();
                }
            }
        }

        #region 上传下载数据库文件 同步数据库
        private void DownloadFileWithNoticeWithObject(object obj)
        {
            DownloadFileWithNotice();
        }

        private void backupData_Click(object sender, EventArgs e)
        {
            /*
            //visibleUploadDownloadGroup(true);
            SetpbUploadDownloadLabel(true);
            SetpbUploadDownloadFile(true, 0);
            Thread t = new Thread(new ParameterizedThreadStart(UploadFileWithNoticeWithObjectBackupData));
            t.Start("手动同步数据库！");
            t.DisableComObjectEagerCleanup();
             */
            SyncDatabaseStarter();
        }

        private void UploadFileWithNoticeWithObjectBackupData(object obj)
        {
            UploadFileWithNotice(obj.ToString());
        }

        public static String UploadMoreInfo;

        // 下载
        private void DownloadFileWithNotice()
        {
            if (ifUpdateDatabasecheckLastModifiedTime(true))
            {
                File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Normal);
                DownloadFile(ONLINE_DATABASE_LOCATION_DIR + onlineDataBaseFilePrefix, LOCAL_DATABASE_LOCATION);
                File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
            }
        }

        private void DownloadFile(String uriString, String fileNamePath)
        {
            //visibleUploadDownloadGroup(true);
            SetpbUploadDownloadLabel(true);
            SetpbUploadDownloadFile(true, 0);

            WebClient client = new WebClient();
            Uri uri = new Uri(uriString);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleteCallback);
            client.DownloadFileAsync(uri, fileNamePath);

        }
        private void DownloadProgressCallback(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            //pbUploadDownloadFile.Value = e.ProgressPercentage;
            SetpbUploadDownloadFile(true, e.ProgressPercentage);
        }

        private void DownloadFileCompleteCallback(Object sender, AsyncCompletedEventArgs e)
        {
            //visibleUploadDownloadGroup(false);
            SetpbUploadDownloadLabel(false);
            SetpbUploadDownloadFile(false, 0);

            if (e.Error != null)
            {
                //MessageBox.Show(e.Error.Message);   //正常捕获
                MessageBox.Show("数据已同步(下载)", "提示");
            }
            else
            {
                FormBasicFeatrues.GetInstence().SoundPlay(VividManagementApplication.Properties.Resources.complete);
                File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                //MessageBox.Show(UploadMoreInfo + "同步成功!", "成功");
                StatusToolStripStatusLabel.Text = UploadMoreInfo + "同步成功!";
            }
        }

        // 上传
        private void UploadFileWithNotice(String moreInfo)
        {
            UploadMoreInfo = moreInfo;
            if (getLocalFileSize(LOCAL_DATABASE_LOCATION) > 0)
            {
                if (UriExists(ONLINE_DATABASE_LOCATION_DIR + onlineDataBaseFilePrefix))
                {
                    if (ifUpdateDatabasecheckLastModifiedTime(false))
                    {
                        UploadFile(LOCAL_DATABASE_LOCATION, ONLINE_DATABASE_FTP_LOCATION_DIR + onlineDataBaseFilePrefix);
                    }
                }
                else
                {
                    UploadFile(LOCAL_DATABASE_LOCATION, ONLINE_DATABASE_FTP_LOCATION_DIR + onlineDataBaseFilePrefix);
                }
            }
        }

        private void UploadFile(String fileNamePath, String uriString)
        {
            //visibleUploadDownloadGroup(true);
            SetpbUploadDownloadLabel(true);
            SetpbUploadDownloadFile(true, 0);

            WebClient client = new WebClient();
            Uri uri = new Uri(uriString);
            client.UploadProgressChanged += new UploadProgressChangedEventHandler(UploadProgressCallback);
            client.UploadFileCompleted += new UploadFileCompletedEventHandler(UploadFileCompleteCallback);
            DatabaseConnections.GetInstence().LocalDbClose();
            client.UploadFileAsync(uri, "STOR", fileNamePath);
            // client.Proxy = WebRequest.DefaultWebProxy;
            //client.Proxy.Credentials = new NetworkCredential(ONLINE_FTP_USERNAME, ONLINE_FTP_PASSWORD, ONLINE_FTP_DOMAIN);

        }
        private void UploadProgressCallback(object sender, System.Net.UploadProgressChangedEventArgs e)
        {
            //pbUploadDownloadFile.Value = e.ProgressPercentage;
            SetpbUploadDownloadFile(true, e.ProgressPercentage);
        }

        private void UploadFileCompleteCallback(Object sender, UploadFileCompletedEventArgs e)
        {
            //visibleUploadDownloadGroup(false);
            SetpbUploadDownloadLabel(false);
            SetpbUploadDownloadFile(false, 0);

            if (e.Error != null)
            {
                //MessageBox.Show(e.Error.Message);   //正常捕获
                MessageBox.Show("数据已同步(上传)", "提示");
            }
            else
            {
                //FormBasicFeatrues.GetInstence().SoundPlay(System.Environment.CurrentDirectory + @"\config\complete.wav");
                FormBasicFeatrues.GetInstence().SoundPlay(VividManagementApplication.Properties.Resources.complete);
                //MessageBox.Show(UploadMoreInfo + "同步成功!", "成功");
                StatusToolStripStatusLabel.Text = UploadMoreInfo + "同步成功!";
            }
        }

        // 检测远程文件是否存在
        private bool UriExists(string url)
        {
            try
            {
                new System.Net.WebClient().OpenRead(url);
                return true;
            }
            catch (System.Net.WebException)
            {
                return false;
            }
        }

        private bool ifUpdateDatabasecheckLastModifiedTime(bool isDownload)
        {
            HttpWebRequest gameFile = (HttpWebRequest)WebRequest.Create(ONLINE_DATABASE_LOCATION_DIR + onlineDataBaseFilePrefix);
            gameFile.Timeout = 5000;
            HttpWebResponse gameFileResponse;
            try
            {
                gameFileResponse = (HttpWebResponse)gameFile.GetResponse();
            }
            catch
            {
                if (isDownload)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            DateTime localFileModifiedTime = File.GetLastWriteTime(LOCAL_DATABASE_LOCATION);
            DateTime onlineFileModifiedTime = gameFileResponse.LastModified;
            gameFile.Abort();
            gameFileResponse.Close();
            if (localFileModifiedTime >= onlineFileModifiedTime)
            {
                if (isDownload)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                //Download new Update
                if (isDownload)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // 获取本地文件大小
        private long getLocalFileSize(String filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return fi.Length;
        }

        // 提示消息能否看到
        private void visibleUploadDownloadGroup(Boolean visible)
        {
            pbUploadDownloadToolStripStatusLabel.Visible = visible;
            pbUploadDownloadFileToolStripProgressBar.Visible = visible;
            //pbUploadDownloadLabel.Text = notice;
        }

        // label
        delegate void SetpbUploadDownloadLabelCallback(Boolean visible);
        private void SetpbUploadDownloadLabel(Boolean visible)
        {
            //this.pbUploadDownloadToolStripStatusLabel.Visible = visible;
            /*
            if (this.pbUploadDownloadToolStripStatusLabel.InvokeRequired)
            {
                SetpbUploadDownloadLabelCallback d = new SetpbUploadDownloadLabelCallback(SetpbUploadDownloadLabel);
                this.Invoke(d, new object[] { visible });
            }
            else
            {
                this.pbUploadDownloadToolStripStatusLabel.Visible = visible;
            }
             */
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    this.pbUploadDownloadToolStripStatusLabel.Visible = visible;
                }));
            }
        }

        // lable
        delegate void SetpbUploadDownloadFileCallback(Boolean visible, int percentage);
        private void SetpbUploadDownloadFile(Boolean visible, int percentage)
        {
            //this.pbUploadDownloadFileToolStripProgressBar.Visible = visible;
            //this.pbUploadDownloadFileToolStripProgressBar.Value = percentage;
            /*
            if (this.pbUploadDownloadFileToolStripProgressBar.ProgressBar.InvokeRequired)
            {
                SetpbUploadDownloadFileCallback d = new SetpbUploadDownloadFileCallback(SetpbUploadDownloadFile);
                this.Invoke(d, new object[] { visible, percentage });
            }
            else
            {
                this.pbUploadDownloadFileToolStripProgressBar.ProgressBar.Visible = visible;
                this.pbUploadDownloadFileToolStripProgressBar.ProgressBar.Value = percentage;
            }
             */
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    this.pbUploadDownloadFileToolStripProgressBar.ProgressBar.Visible = visible;
                    this.pbUploadDownloadFileToolStripProgressBar.ProgressBar.Value = percentage;
                }));
            }
        }

        #endregion

        private void tmrShows_Tick(object sender, EventArgs e)
        {
            lblSHOWS.Left--;
            lblSHOWS2.Left--;

            if (lblSHOWS.Left < -lblSHOWS.Width)
            {
                if (lblSHOWS.Width < notificationPanel.Width)
                    lblSHOWS.Left = notificationPanel.Width + lblSHOWS2.Left;
                else
                    lblSHOWS.Left = lblSHOWS.Width + lblSHOWS2.Left;
            }

            if (lblSHOWS2.Left < -lblSHOWS2.Width)
            {
                if (lblSHOWS2.Width < notificationPanel.Width)
                    lblSHOWS2.Left = notificationPanel.Width + lblSHOWS.Left;
                else
                    lblSHOWS2.Left = lblSHOWS2.Width + lblSHOWS.Left;
            }
        }

        private void refeshButton_Click(object sender, EventArgs e)
        {
            StatusToolStripStatusLabel.Text = "正在刷新...";
            if (CURRENT_LIST_BUTTON == listQdButton)
            {
                Thread t = new Thread(new ParameterizedThreadStart(refreshCheckRemoteSignListWithObject));
                t.Start();
                t.DisableComObjectEagerCleanup();
                return;
            }
            else
            {
                CURRENT_LIST_BUTTON.PerformClick();
                //MessageBox.Show("刷新成功！");
                StatusToolStripStatusLabel.Text = "刷新成功！";
            }
        }

        private void refreshCheckRemoteSignListWithObject(object obj)
        {
            checkRemoteSignList();
            //CURRENT_LIST_BUTTON.PerformClick();
            //MessageBox.Show("刷新成功！");
            StatusToolStripStatusLabel.Text = "刷新成功！";
        }

        #region 左侧栏选择
        private void ableSubButtons(List<QQButton> btList, QQRadioButton rd)
        {
            foreach (QQButton bt in btList)
            {
                bt.Enabled = rd.Checked;
            }
        }

        private void cxRadio_CheckedChanged(object sender, EventArgs e)
        {
            CURRENT_TAB = 1;
            ableSubButtons(new List<QQButton>() { newCxButton, listCxButton, dataCxImport }, cxRadio);
            listCxButton.PerformClick(); ;
        }

        private void spRadio_CheckedChanged(object sender, EventArgs e)
        {
            CURRENT_TAB = 2;
            ableSubButtons(new List<QQButton>() { newSpButton, listSpButton, dataSpImport }, spRadio);
            listSpButton.PerformClick();
        }

        private void ccRadio_CheckedChanged(object sender, EventArgs e)
        {
            CURRENT_TAB = 3;
            ableSubButtons(new List<QQButton>() { listKcButton, listJcdButton, listCcdButton }, ccRadio);
            listKcButton.PerformClick();
        }

        private void ywRadio_CheckedChanged(object sender, EventArgs e)
        {
            CURRENT_TAB = 4;
            ableSubButtons(new List<QQButton>() { newCgZsButton, listCgButton, listXsButton }, ywRadio);
            listCgButton.PerformClick();
        }

        private void cwRadio_CheckedChanged(object sender, EventArgs e)
        {
            CURRENT_TAB = 5;
            ableSubButtons(new List<QQButton>() { newPzButton, listSfzhButton, listKhdzButton }, cwRadio);
            listSfzhButton.PerformClick();
        }

        private void htRadio_CheckedChanged(object sender, EventArgs e)
        {
            CURRENT_TAB = 6;
            ableSubButtons(new List<QQButton>() { newHtButton, listHtButton }, htRadio);
            listHtButton.PerformClick();
        }

        private void QdRadio_CheckedChanged(object sender, EventArgs e)
        {
            CURRENT_TAB = 7;
            ableSubButtons(new List<QQButton>() { listQdButton }, QdRadio);
            listQdButton.PerformClick();
        }
        #endregion

        #region 创建MainDataGridView表格

        DataGridViewTextBoxColumn Column1 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column2 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column3 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column4 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column5 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column6 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column7 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column8 = new DataGridViewTextBoxColumn();

        private void ClearDataGridViewColumnSortOrder(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.MainDataGridView.Columns[i].HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }

        private void CreateMainDataGridView(DataGridViewColumn[] dgvcArray, string table, int discardFlagIndex, string[] queryArray)
        {
            string order = " ORDER BY id ASC ";
            this.MainDataGridView.Columns.Clear();
            this.MainDataGridView.Rows.Clear();

            this.MainDataGridView.Columns.AddRange(dgvcArray);
            ClearDataGridViewColumnSortOrder(dgvcArray.Length);
            List<string[]> resultsList = DatabaseConnections.GetInstence().LocalGetData(table, queryArray, order);
            for (int i = 0; i < resultsList.Count; i++)
            {
                this.MainDataGridView.Rows.Add(resultsList[i]);
                if (discardFlagIndex != -1)
                {
                    if (!resultsList[i][discardFlagIndex].Equals("否"))
                    {
                        this.MainDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                        this.MainDataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
                    }
                }
            }
            this.MainDataGridView.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            if (this.MainDataGridView.Rows.Count <= 0)
            {
                ViewButton.Enabled = false;
            }
        }

        // 收支汇总表
        private void CreateMainDataGridViewPZ(DataGridViewColumn[] dgvcArray, string table, string[] queryArray)
        {
            string order = " ORDER BY id ASC "; // 顺序
            this.MainDataGridView.Columns.Clear();
            this.MainDataGridView.Rows.Clear();
            float tempBalance = COMPANY_BALANCE;

            this.MainDataGridView.Columns.AddRange(dgvcArray);
            ClearDataGridViewColumnSortOrder(dgvcArray.Length);
            List<string[]> resultsList = DatabaseConnections.GetInstence().LocalGetData(table, queryArray, order);

            for (int i = 0; i < resultsList.Count; i++)
            {
                if (resultsList[i][6].Equals("否"))
                {
                    if ((resultsList[i][2].Equals("收款凭证")) || resultsList[i][2].Equals("还款凭证"))
                    {
                        tempBalance += float.Parse(resultsList[i][4].Equals("") ? "0" : resultsList[i][4]);
                    }
                    else
                    {
                        tempBalance -= float.Parse(resultsList[i][4].Equals("") ? "0" : resultsList[i][4]);
                    }
                    resultsList[i][5] = tempBalance.ToString();
                }

            }
            //resultsList.Reverse();
            for (int j = 0; j < resultsList.Count; j++)
            {
                this.MainDataGridView.Rows.Add(resultsList[j]);
                if (!resultsList[j][6].Equals("否"))
                {
                    this.MainDataGridView.Rows[j].DefaultCellStyle.BackColor = Color.LightGray;
                    this.MainDataGridView.Rows[j].DefaultCellStyle.ForeColor = Color.Gray;
                }
            }
            // this.MainDataGridView.Columns[2].HeaderCell.SortGlyphDirection = SortOrder.Descending;
            if (this.MainDataGridView.Rows.Count <= 0)
            {
                ViewButton.Enabled = false;
            }
        }

        // DataGridView双击

        private void MainDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ViewButton.Enabled && e.RowIndex != -1)
            {
                ViewButton.PerformClick();
            }
        }

        private void ClearMainDataGridView()
        {
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
        }

        // 客户管理
        private void listCxButton_Click(object sender, EventArgs e)
        {
            try
            {
                ClearMainDataGridView();
                refeshButton.Enabled = true;
                ViewButton.Enabled = true;
                PrintButton.Enabled = false;
                CURRENT_LIST_BUTTON = listCxButton;
                CURRENT_TAB = 1;
                mainDGVTitle.Text = listCxButton.Text;
                Column1.HeaderText = "客户编号";
                Column2.HeaderText = "公司名称";
                Column3.HeaderText = "联系地址";
                Column4.HeaderText = "联系人";
                Column5.HeaderText = "联系电话";

                Column2.Width = 200;
                Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                Column3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 }, "clients", -1,
                    new string[] { "clientID", "company", "address", "companyOwner", "phone" });
            }
            catch { return; };
        }

        // 商品管理
        private void listSpButton_Click(object sender, EventArgs e)
        {
            ClearMainDataGridView();
            refeshButton.Enabled = true;
            ViewButton.Enabled = true;
            PrintButton.Enabled = false;
            CURRENT_LIST_BUTTON = listSpButton;
            CURRENT_TAB = 2;
            mainDGVTitle.Text = listSpButton.Text;
            Column1.HeaderText = "商品编号";
            Column2.HeaderText = "商品名称";
            Column3.HeaderText = "规格";
            Column4.HeaderText = "等级";
            Column5.HeaderText = "单位";
            Column6.HeaderText = "销售单价";
            Column7.HeaderText = "备注";

            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7 }, "goods", -1,
                new string[] { "goodID", "name", "guige", "dengji", "unit", "currntsalesPrice", "beizhu" });
        }

        #region 仓储管理
        // 库存
        private void listKcButton_Click(object sender, EventArgs e)
        {
            ClearMainDataGridView();
            refeshButton.Enabled = true;
            ViewButton.Enabled = false;
            PrintButton.Enabled = false;
            CURRENT_LIST_BUTTON = listKcButton;
            CURRENT_TAB = 3;
            mainDGVTitle.Text = listKcButton.Text;
            Column1.HeaderText = "商品编号";
            Column2.HeaderText = "商品名称";
            Column3.HeaderText = "规格";
            Column4.HeaderText = "等级";
            Column5.HeaderText = "单位";
            Column6.HeaderText = "库存数量";
            Column7.HeaderText = "备注";

            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7 }, "goods", -1,
                new string[] { "goodID", "name", "guige", "dengji", "unit", "currentCount", "beizhu" });
        }

        private void listJcdButton_Click(object sender, EventArgs e)
        {
            ClearMainDataGridView();
            refeshButton.Enabled = true;
            ViewButton.Enabled = true;
            PrintButton.Enabled = false;
            CURRENT_LIST_BUTTON = listJcdButton;
            CURRENT_TAB = 3;
            mainDGVTitle.Text = listJcdButton.Text;
            Column1.HeaderText = "凭证号码";
            Column2.HeaderText = "单位名称";
            Column3.HeaderText = "商品名称";
            Column4.HeaderText = "作废标识";

            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4 }, "jcdList", 3,
                new string[] { "jcdID", "companyName", "goodsName", "case when discardFlag = '0' then '否' else '已作废' end as 'discardFlag'" });
        }

        private void listCcdButton_Click(object sender, EventArgs e)
        {
            ClearMainDataGridView();
            refeshButton.Enabled = true;
            ViewButton.Enabled = true;
            PrintButton.Enabled = false;
            CURRENT_LIST_BUTTON = listCcdButton;
            CURRENT_TAB = 3;
            mainDGVTitle.Text = listCcdButton.Text;
            Column1.HeaderText = "凭证号码";
            Column2.HeaderText = "单位名称";
            Column3.HeaderText = "商品名称";
            Column4.HeaderText = "作废标识";

            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4 }, "ccdList", 3,
                new string[] { "ccdID", "companyName", "goodsName", "case when discardFlag = '0' then '否' else '已作废' end as 'discardFlag'" });
        }

        #endregion

        #region 业务管理

        private void listCgXsButton_Click(object sender, EventArgs e)
        {
            ClearMainDataGridView();
            refeshButton.Enabled = true;
            ViewButton.Enabled = true;
            PrintButton.Enabled = false;
            CURRENT_LIST_BUTTON = listCgButton;
            CURRENT_TAB = 4;
            mainDGVTitle.Text = listCgButton.Text;
            Column1.HeaderText = "凭证号码";
            Column2.HeaderText = "单位名称";
            Column3.HeaderText = "商品名称";
            Column4.HeaderText = "作废标识";

            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4 }, "cgdList", 3,
                new string[] { "cgdID", "companyName", "goodsName", "case when discardFlag = '0' then '否' else '已作废' end as 'discardFlag'" });
        }

        private void listXsButton_Click(object sender, EventArgs e)
        {
            ClearMainDataGridView();
            refeshButton.Enabled = true;
            ViewButton.Enabled = true;
            PrintButton.Enabled = false;
            CURRENT_LIST_BUTTON = listXsButton;
            CURRENT_TAB = 4;
            mainDGVTitle.Text = listXsButton.Text;
            Column1.HeaderText = "凭证号码";
            Column2.HeaderText = "单位名称";
            Column3.HeaderText = "商品名称";
            Column4.HeaderText = "作废标识";

            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4 }, "xsdList", 3,
                new string[] { "xsdID", "companyName", "goodsName", "case when discardFlag = '0' then '否' else '已作废' end as 'discardFlag'" });

        }
        #endregion

        // 凭证列表 收付汇总表
        private void listSfzhButton_Click(object sender, EventArgs e)
        {
            ClearMainDataGridView();
            refeshButton.Enabled = true;
            ViewButton.Enabled = true;
            PrintButton.Enabled = false;
            CURRENT_LIST_BUTTON = listSfzhButton;
            CURRENT_TAB = 5;
            mainDGVTitle.Text = listSfzhButton.Text;
            Column1.HeaderText = "凭证号码";
            Column2.HeaderText = "日期";
            Column3.HeaderText = "凭证类型";
            Column4.HeaderText = "对方单位";
            Column5.HeaderText = "交易金额";
            Column6.HeaderText = "结余金额";
            Column7.HeaderText = "作废标识";

            Column1.Width = 80;
            Column2.Width = 140;

            Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridViewPZ(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7 }, "pzList",
                new string[] { "pzID", 
                    "cast(modifyTime as VARCHAR)", 
                    "case when leixing = '0' then '收款凭证' when leixing = '1' then '付款凭证' when leixing = '2' then '领款凭证' when leixing = '3' then '还款凭证' else '报销凭证' end as 'leixing'", 
                    "companyName", 
                    "operateMoney", 
                    "remaintingMoney", 
                    "case when discardFlag = '0' then '否' else '已作废' end as 'discardFlag'" });
        }

        private String clientIDFromFilter = "";
        private String clientNameFromFilter = "";

        private Boolean isYS = false;
        // 客户对账单
        private void listKhdzButton_Click(object sender, EventArgs e)
        {
            ClearMainDataGridView();
            refeshButton.Enabled = true;
            ViewButton.Enabled = false;
            PrintButton.Enabled = true;
            this.MainDataGridView.Rows.Clear();
            this.MainDataGridView.Columns.Clear();

            Filter flt = new Filter();
            flt.Text = "请筛选...";

            if (flt.ShowDialog() == System.Windows.Forms.DialogResult.OK) // OK键
            {
                clientIDFromFilter = flt.clientID.Text;
                clientNameFromFilter = flt.clientName.Text;

                CURRENT_LIST_BUTTON = listKhdzButton;
                CURRENT_TAB = 4;
                mainDGVTitle.Text = listKhdzButton.Text;
                Column1.HeaderText = "凭证号码";
                Column2.HeaderText = "日期";
                Column3.HeaderText = "凭证类型";
                Column4.HeaderText = "摘要";
                Column5.HeaderText = "交易金额";
                Column6.HeaderText = "结余金额";

                Column1.Width = 80;
                Column2.Width = 140;
                Column3.Width = 100;
                Column5.Width = 100;
                Column6.Width = 100;

                Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                float tempBalance = COMPANY_BALANCE;

                DataGridViewColumn[] dgvcArray = new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6 };
                this.MainDataGridView.Columns.AddRange(dgvcArray);
                ClearDataGridViewColumnSortOrder(dgvcArray.Length);

                String dzTypeInDatabase, dzTypeID, dzTypeName, leixingLimitation;
                if (flt.DzTypeComboBox.SelectedIndex == 0)
                {
                    isYS = true;
                    dzTypeInDatabase = "xsdList";
                    dzTypeID = "xsdID";
                    dzTypeName = "销售凭证";
                    leixingLimitation = " (leixing = '0' OR leixing = '2') AND ";
                }
                else
                {
                    isYS = false;
                    dzTypeInDatabase = "cgdList";
                    dzTypeID = "cgdID";
                    dzTypeName = "采购凭证";
                    leixingLimitation = " (leixing = '1' OR leixing = '3') AND ";
                }

                String limitation = "clientID = '" + flt.clientID.Text + "' AND discardFlag = 0"
                    + " AND (modifyTime BETWEEN '" + flt.fromDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + flt.toDate.Value.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')";

                List<string[]> resultsList = DatabaseConnections.GetInstence().LocalGetDataFromOriginalSQL(
                    "SELECT pzID, cast (modifyTime as VARCHAR) as modifyTime"
                    + ",case when leixing = '0' then '收款凭证' when leixing = '1' then '付款凭证' when leixing = '2' then '领款凭证' when leixing = '3' then '还款凭证' else '报销凭证' end as 'leixing'"
                    + ",zhaiyao"
                    + ",operateMoney,remaintingMoney FROM pzList "
                    + "WHERE " + leixingLimitation + limitation
                    + " UNION "
                    + " SELECT " + dzTypeID + " as pzID ,cast (modifyTime as VARCHAR), '" + dzTypeName + "'as leixing, companyName as zhaiyao, sum as operateMoney, 0 as remaintingMoney FROM " + dzTypeInDatabase
                    + " WHERE " + limitation + " ORDER BY modifyTime ASC",
                    new String[] { "pzID", "modifyTime", "leixing", "zhaiyao", "operateMoney", "remaintingMoney" });

                for (int i = 0; i < resultsList.Count; i++)
                {
                    //tempBalance += float.Parse(resultsList[i][4]);
                    if (isYS)
                    {
                        if (resultsList[i][2].Equals("收款凭证"))
                        {
                            tempBalance -= float.Parse(resultsList[i][4]);
                        }
                        else
                        {
                            tempBalance += float.Parse(resultsList[i][4]);
                        }
                    }
                    else
                    {
                        if (resultsList[i][2].Equals("采购凭证"))
                        {
                            tempBalance += float.Parse(resultsList[i][4]);
                        }
                        else
                        {
                            tempBalance -= float.Parse(resultsList[i][4]);
                        }
                    }
                    resultsList[i][5] = tempBalance.ToString();
                }
                //resultsList.Reverse();
                for (int j = 0; j < resultsList.Count; j++)
                {
                    this.MainDataGridView.Rows.Add(resultsList[j]);
                }

                mainDGVTitle.Text = flt.DzTypeComboBox.Text;

            }
            else // 取消键
            {
                listSfzhButton.PerformClick(); // 切换到收付汇总表
            }
        }

        // 合同列表
        private void listHtButton_Click(object sender, EventArgs e)
        {
            ClearMainDataGridView();
            refeshButton.Enabled = true;
            ViewButton.Enabled = true;
            PrintButton.Enabled = false;
            CURRENT_LIST_BUTTON = listHtButton;
            CURRENT_TAB = 6;
            mainDGVTitle.Text = listHtButton.Text;
            Column1.HeaderText = "合同编号";
            Column2.HeaderText = "签订日期";
            Column3.HeaderText = "合同类型";
            Column4.HeaderText = "对方单位名称";
            Column5.HeaderText = "合同金额";
            Column6.HeaderText = "作废标识";

            Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6 }, "htList", 5,
                new string[] { "htID", "htDate", "leixing", "companyName", "sum", "case when discardFlag = '0' then '否' else '已作废' end as 'discardFlag'" });
        }

        // 签单列表

        private void listQdButton_Click(object sender, EventArgs e)
        {
            ClearMainDataGridView();
            refeshButton.Enabled = true;
            ViewButton.Enabled = true;
            PrintButton.Enabled = false;
            CURRENT_LIST_BUTTON = listQdButton;
            CURRENT_TAB = 7;
            mainDGVTitle.Text = listQdButton.Text;
            Column1.HeaderText = "ID";
            Column2.HeaderText = "操作时间";
            Column3.HeaderText = "标识";
            Column4.HeaderText = "对方管账宝ID";
            Column5.HeaderText = "客户名称";
            Column6.HeaderText = "备  注";
            Column7.HeaderText = "状  态";

            Column2.Width = 120;
            Column3.Width = 60;
            Column6.Width = 240;
            Column7.Width = 60;
            Column1.Visible = false;
            Column4.Visible = false;

            //Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //checkRemoteSignList();
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7 }, "remoteSign", -1,
                new string[] { 
                    "Id", 
                    "case when isSigned = '0' then sendTime else signTime end as 'operationTime'",
                    "case when fromGZBID = '"+MainWindow.USER_ID+"' then '发送' else '接收' end as 'operation'",
                    "case when fromGZBID = '"+MainWindow.USER_ID+"' then toGZBID else fromGZBID end as 'peerID'", 
                    "companyNickName", 
                    "refusedMessage",
                    "case when isSigned = '1' then '已签' when isSigned ='-1' then '拒签'  else '未处理' end as 'isSigned'"
                });
            this.MainDataGridView.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.Descending;
        }
        #endregion

        #region 新建DetailedWindow
        private void newCxButton_Click(object sender, EventArgs e)
        {
            CURRENT_TAB = 1;
            CreateDetailedWindow();
        }

        private void newSpButton_Click(object sender, EventArgs e)
        {
            CURRENT_TAB = 2;
            CreateDetailedWindow();
        }

        // 客户数据 excel导入
        private void dataCxImport_Click(object sender, EventArgs e)
        {
            DataImport di = new DataImport();
            di.isTypeCx = true;
            if (di.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("客户信息导入成功！已导入" + di.importedCount + "条客户信息", "提示");
                listCxButton.PerformClick();
            }
        }

        // 商品数据 excel导入
        private void dataSpImport_Click(object sender, EventArgs e)
        {
            DataImport di = new DataImport();
            di.isTypeCx = false;
            if (di.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("商品信息导入成功！已导入" + di.importedCount + "条商品信息", "提示");
                listSpButton.PerformClick();
            }
        }

        // 新建进出仓单
        private void newJCcButton_Click(object sender, EventArgs e)
        {
            CURRENT_TAB = 3;
            CreateDetailedWindow();
        }

        private void newCgZsButton_Click(object sender, EventArgs e)
        {
            CURRENT_TAB = 4;
            CreateDetailedWindow();
        }

        private void newPzButton_Click(object sender, EventArgs e)
        {
            MainWindow.CURRENT_LIST_BUTTON = listSfzhButton;
            CURRENT_TAB = 5;
            CreateDetailedWindow();
        }

        private void newHtButton_Click(object sender, EventArgs e)
        {
            CURRENT_TAB = 6;
            CreateDetailedWindow();
        }

        #endregion

        /// <summary>
        /// 关闭之前备份数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MainWindow.IS_LOGED_IN)
            {
                if (notifyIcon != null)
                {
                    notifyIcon.Visible = false;
                    notifyIcon.Icon = null; // required to make icon disappear
                    notifyIcon.Dispose();
                    notifyIcon = null;
                }

                updateRemoteSignTimer.Dispose();
                lablTextChangeTimer.Dispose();
                DatabaseConnections.GetInstence().LocalDbClose();

                //Thread t = new Thread(new ParameterizedThreadStart(UploadFileWithNoticeWithObjectBackupData));
                //t.Start("关闭前同步!");
                //t.DisableComObjectEagerCleanup();

                this.Dispose(true);
                File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                DatabaseConnections.GetInstence().OnlineUpdateDataFromOriginalSQL("UPDATE users SET GZB_isonline = 0 WHERE userid = '" + MainWindow.USER_ID + "'");
                DatabaseConnections.GetInstence().OnlineDbClose();

                Application.Exit();
            }
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            this.Close();
        }

        private void settingQQButton_Click(object sender, EventArgs e)
        {
            Setting st = new Setting();
            st.ShowDialog();
        }

        #region DataGridView打印

        StringFormat strFormat; //Used to format the grid rows.
        ArrayList arrColumnLefts = new ArrayList();//Used to save left coordinates of columns
        ArrayList arrColumnWidths = new ArrayList();//Used to save column widths
        int iCellHeight = 0; //Used to get/set the datagridview cell height
        int iTotalWidth = 0; //
        int iRow = 0;//Used as counter
        bool bFirstPage = false; //Used to check whether we are printing first page
        bool bNewPage = false;// Used to check whether we are printing a new page
        int iHeaderHeight = 0; //Used for the header height
        int pageIndex = 0;// 页码
        int totalPageNumber = 0; // 总页码

        #region Print Button Click Event
        /// <summary>
        /// 打印DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintButton_Click(object sender, EventArgs e)
        {
            //SetPrintPreview(1, 1200);

            ////Open the print preview dialog
            //PrintPreviewDialog objPPdialog = new PrintPreviewDialog();
            //objPPdialog.Document = printDocument1;
            //this.printPreviewDialog1.WindowState = FormWindowState.Maximized;
            //objPPdialog.ShowDialog();
            if (MainDataGridView.Rows.Count > 0)
            {
                //sprintDocument1.OriginAtMargins = true; //启用页边距
                //printDocument1.DefaultPageSettings.Margins.Top = 0; //设置顶部页边距 
                //printDocument1.DefaultPageSettings.Margins.Left = 20; //设置左部页边距
                SetDZDPrintFormat();
                //注意指定其Document(获取或设置要预览的文档)属性
                this.printPreviewDialog1.Document = this.printDocument1;
                //ShowDialog方法：将窗体显示为模式对话框，并将当前活动窗口设置为它的所有者
                this.printPreviewDialog1.WindowState = FormWindowState.Maximized;
                this.printPreviewDialog1.ShowDialog();
            }
            else
            {
                MessageBox.Show("表格数据缺失, 不能打印", "错误");
            }
        }

        DataGridView DZDPrintingDGV;
        private void SetDZDPrintFormat()
        {
            //MainDataGridView.Columns[1].ValueType = Type.datetime;
            this.MainDataGridView.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.Descending;
            DataGridView tempDGV = new DataGridView();
            tempDGV = CopyDataGridView(MainDataGridView);
            tempDGV.Columns.Add(new DataGridViewTextBoxColumn());
            tempDGV.Columns[0].HeaderText = "序";
            tempDGV.Columns[1].HeaderText = "日 期";
            tempDGV.Columns[2].HeaderText = "凭证号";
            tempDGV.Columns[3].HeaderText = "摘  要";
            tempDGV.Columns[4].HeaderText = isYS ? "应收金额(元)" : "应付金额(元)";
            tempDGV.Columns[5].HeaderText = isYS ? "已收金额(元)" : "已付金额(元)";
            tempDGV.Columns[6].HeaderText = "结余金额(元)";

            tempDGV.Columns[0].Width = 30;
            tempDGV.Columns[1].Width = 75;
            tempDGV.Columns[2].Width = 75;
            tempDGV.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tempDGV.Columns[4].Width = 65;
            tempDGV.Columns[5].Width = 65;
            tempDGV.Columns[6].Width = 65;

            DZDPrintingDGV = CopyDataGridView(tempDGV);
            float yingTotal = 0, yiTotal = 0, yuTotal = 0;
            for (int i = 0; i < MainDataGridView.Rows.Count; i++)
            {
                String leixing = tempDGV.Rows[i].Cells[2].Value.ToString().Substring(0, 1);
                DZDPrintingDGV.Rows[i].Cells[0].Value = (i + 1).ToString();
                DZDPrintingDGV.Rows[i].Cells[1].Value = DateTime.Parse(tempDGV.Rows[i].Cells[1].Value.ToString()).ToShortDateString();
                DZDPrintingDGV.Rows[i].Cells[2].Value = leixing + tempDGV.Rows[i].Cells[0].Value.ToString();
                DZDPrintingDGV.Rows[i].Cells[3].Value = tempDGV.Rows[i].Cells[3].Value.ToString();
                DZDPrintingDGV.Rows[i].Cells[4].Value = isYS ? ((leixing.Equals("收") ? "" : tempDGV.Rows[i].Cells[4].Value.ToString())) : (leixing.Equals("付") ? "" : tempDGV.Rows[i].Cells[4].Value.ToString());
                DZDPrintingDGV.Rows[i].Cells[5].Value = isYS ? ((leixing.Equals("收") ? tempDGV.Rows[i].Cells[4].Value.ToString() : "")) : (leixing.Equals("付") ? tempDGV.Rows[i].Cells[4].Value.ToString() : "");
                DZDPrintingDGV.Rows[i].Cells[6].Value = tempDGV.Rows[i].Cells[5].Value.ToString();
                yingTotal += float.Parse(DZDPrintingDGV.Rows[i].Cells[4].Value.ToString().Equals("") ? "0" : DZDPrintingDGV.Rows[i].Cells[4].Value.ToString());
                yiTotal += float.Parse(DZDPrintingDGV.Rows[i].Cells[5].Value.ToString().Equals("") ? "0" : DZDPrintingDGV.Rows[i].Cells[5].Value.ToString());
                yuTotal += float.Parse(DZDPrintingDGV.Rows[i].Cells[6].Value.ToString().Equals("") ? "0" : DZDPrintingDGV.Rows[i].Cells[6].Value.ToString());
            }
            DZDPrintingDGV.Rows.Add("结", "算：", "", "", "￥" + yingTotal, "￥" + yiTotal, "￥" + yuTotal);
            DZDPrintingDGV.Rows[DZDPrintingDGV.Rows.Count - 1].Cells[0].ToolTipText = "1";
            DZDPrintingDGV.Rows[DZDPrintingDGV.Rows.Count - 1].Cells[1].ToolTipText = "0";
            DZDPrintingDGV.Rows[DZDPrintingDGV.Rows.Count - 1].Cells[2].ToolTipText = "0";
            DZDPrintingDGV.Rows[DZDPrintingDGV.Rows.Count - 1].Cells[3].ToolTipText = "0";
            DZDPrintingDGV.Rows[DZDPrintingDGV.Rows.Count - 1].Cells[4].ToolTipText = "2";
            DZDPrintingDGV.Rows[DZDPrintingDGV.Rows.Count - 1].Cells[5].ToolTipText = "2";
            DZDPrintingDGV.Rows[DZDPrintingDGV.Rows.Count - 1].Cells[6].ToolTipText = "2";

            //DZDPrintingDGV.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DZDPrintingDGV.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DZDPrintingDGV.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DZDPrintingDGV.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private DataGridView CopyDataGridView(DataGridView dgv_org)
        {
            DataGridView dgv_copy = new DataGridView();
            try
            {
                if (dgv_copy.Columns.Count == 0)
                {
                    foreach (DataGridViewColumn dgvc in dgv_org.Columns)
                    {
                        dgv_copy.Columns.Add(dgvc.Clone() as DataGridViewColumn);
                    }
                }

                DataGridViewRow row = new DataGridViewRow();

                for (int i = 0; i < dgv_org.Rows.Count; i++)
                {
                    row = (DataGridViewRow)dgv_org.Rows[i].Clone();
                    int intColIndex = 0;
                    foreach (DataGridViewCell cell in dgv_org.Rows[i].Cells)
                    {
                        row.Cells[intColIndex].Value = cell.Value;
                        intColIndex++;
                    }
                    dgv_copy.Rows.Add(row);
                }
                dgv_copy.AllowUserToAddRows = false;
                dgv_copy.Refresh();

            }
            catch
            {
                //ShowExceptionErrorMsg("Copy DataGridViw", ex);
            }
            return dgv_copy;
        }

        #endregion

        #region Begin Print Event Handler
        /// <summary>
        /// Handles the begin print event of print document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Near;
                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.Trimming = StringTrimming.EllipsisCharacter;

                arrColumnLefts.Clear();
                arrColumnWidths.Clear();
                iCellHeight = 0;
                iRow = 0;
                bFirstPage = true;
                bNewPage = true;

                // Calculating Total Widths
                iTotalWidth = 0;
                foreach (DataGridViewColumn dgvGridCol in DZDPrintingDGV.Columns)
                {
                    iTotalWidth += dgvGridCol.Width;
                }
                //iTotalWidth = this.printDocument1.DefaultPageSettings.PaperSize.Width-150;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Print Page Event
        /// <summary>
        /// Handles the print page event of print document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                //Set the left margin
                int iLeftMargin = e.MarginBounds.Left;
                //Set the top margin
                int iTopMargin = e.MarginBounds.Top;
                //Whether more pages have to print or not
                bool bMorePagesToPrint = false;
                int iTmpWidth = 0;

                // 文字内部的左偏移
                int stringLeftMargin = 3;

                //For the first page to print set the cell width and header height
                if (bFirstPage)
                {
                    foreach (DataGridViewColumn GridCol in DZDPrintingDGV.Columns)
                    {
                        iTmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                                       (double)iTotalWidth * (double)iTotalWidth *
                                       ((double)e.MarginBounds.Width / (double)iTotalWidth))));


                        iHeaderHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText,
                                    GridCol.InheritedStyle.Font, iTmpWidth).Height) + 11;

                        // Save width and height of headres
                        arrColumnLefts.Add(iLeftMargin);
                        arrColumnWidths.Add(iTmpWidth);
                        iLeftMargin += iTmpWidth;
                    }
                }
                //Loop till all the grid rows not get printed
                while (iRow <= DZDPrintingDGV.Rows.Count - 1)
                {
                    DataGridViewRow GridRow = DZDPrintingDGV.Rows[iRow];
                    //Set the cell height
                    iCellHeight = GridRow.Height + 8; // 表格高度 原本是5
                    int iCount = 0;
                    //Check whether the current page settings allow more rows to print
                    if (iTopMargin + iCellHeight >= e.MarginBounds.Height + e.MarginBounds.Top + 38) // 表格长度 原本为0
                    {
                        bNewPage = true;
                        bFirstPage = false;
                        bMorePagesToPrint = true;
                        break;
                    }
                    else
                    {
                        if (bNewPage)
                        {
                            String titleString = mainDGVTitle.Text;
                            String leftString = "客户ID:" + clientIDFromFilter + "\t客户名称:" + clientNameFromFilter;
                            String rightString = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();

                            // 标题
                            //MainWindow.COMPANY_NAME = "桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司"; //测试
                            SizeF fontSize = e.Graphics.MeasureString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", MainWindow.COMPANY_NAME), new Font("微软雅黑", 13));//桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司
                            e.Graphics.DrawString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", MainWindow.COMPANY_NAME), new Font("微软雅黑", 13), new SolidBrush(Color.Blue), this.printDocument1.DefaultPageSettings.PaperSize.Width / 2 - fontSize.Width / 2, 30);

                            fontSize = e.Graphics.MeasureString(FormBasicFeatrues.GetInstence().addCharIntoString(" ", titleString), new Font("微软雅黑", 15));
                            e.Graphics.DrawString(FormBasicFeatrues.GetInstence().addCharIntoString(" ", titleString), new Font(new Font("微软雅黑", 15), FontStyle.Bold),
                                    Brushes.Black, this.printDocument1.DefaultPageSettings.PaperSize.Width / 2 - fontSize.Width / 2, 70);

                            // 页码
                            pageIndex++;
                            fontSize = e.Graphics.MeasureString("- " + pageIndex.ToString() + " -", new Font("微软雅黑", 12));
                            e.Graphics.DrawString("- " + pageIndex.ToString() + " -", new Font(DZDPrintingDGV.Font, FontStyle.Bold),
                                   Brushes.Black, this.printDocument1.DefaultPageSettings.PaperSize.Width / 2 - fontSize.Width / 2,
                                   this.printDocument1.DefaultPageSettings.PaperSize.Height - 45);

                            // 页脚
                            fontSize = e.Graphics.MeasureString("此账单版本由唯达软件系统提供 http://www.vividapp.net/   软件定制电话: 15024345993   QQ: 70269387", new Font("微软雅黑", 7));
                            e.Graphics.DrawString("此账单版本由唯达软件系统提供 http://www.vividapp.net/   软件定制电话: 15024345993   QQ: 70269387", new Font("微软雅黑", 7), new SolidBrush(Color.Red),
                                this.printDocument1.DefaultPageSettings.PaperSize.Width / 2 - fontSize.Width / 2,
                                this.printDocument1.DefaultPageSettings.PaperSize.Height - 65);

                            //Draw Header
                            e.Graphics.DrawString(leftString, new Font(DZDPrintingDGV.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
                                    e.Graphics.MeasureString(leftString, new Font(DZDPrintingDGV.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Height + 15);

                            //Draw Date
                            e.Graphics.DrawString(rightString, new Font(DZDPrintingDGV.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                    e.Graphics.MeasureString(rightString, new Font(DZDPrintingDGV.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top -
                                    e.Graphics.MeasureString(leftString, new Font(new Font(DZDPrintingDGV.Font,
                                    FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height + 15);

                            //Draw Columns                 
                            iTopMargin = e.MarginBounds.Top + 20; // 表格位置 原本为0
                            foreach (DataGridViewColumn GridCol in DZDPrintingDGV.Columns)
                            {
                                e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                                    new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                e.Graphics.DrawRectangle(Pens.Black,
                                    new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                e.Graphics.DrawString(GridCol.HeaderText, GridCol.InheritedStyle.Font,
                                    new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                    new RectangleF((int)arrColumnLefts[iCount] + stringLeftMargin, iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight), strFormat);
                                iCount++;
                            }
                            bNewPage = false;
                            iTopMargin += iHeaderHeight;
                        }
                        iCount = 0;
                        //Draw Columns Contents                
                        foreach (DataGridViewCell Cel in GridRow.Cells)
                        {
                            if (Cel.Value != null)
                            {
                                if (Cel.ToolTipText.Equals("2"))
                                {
                                    // 下划线
                                    e.Graphics.DrawString(Cel.Value.ToString(), new System.Drawing.Font(Cel.InheritedStyle.Font.Name, Cel.InheritedStyle.Font.Size + 1, FontStyle.Underline),
                                                new SolidBrush(Cel.InheritedStyle.ForeColor),
                                                new RectangleF((int)arrColumnLefts[iCount], (float)iTopMargin,
                                                (int)arrColumnWidths[iCount], (float)iCellHeight), strFormat);
                                }
                                else
                                {
                                    e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font,
                                                new SolidBrush(Cel.InheritedStyle.ForeColor),
                                                new RectangleF((int)arrColumnLefts[iCount] + stringLeftMargin, (float)iTopMargin,
                                                (int)arrColumnWidths[iCount], (float)iCellHeight), strFormat);
                                }
                            }
                            //Drawing Cells Borders 
                            if (Cel.ToolTipText.Equals("0"))
                            {
                            }
                            else if (Cel.ToolTipText.Equals("1"))
                            {
                                e.Graphics.DrawLine(Pens.Black, new Point((int)arrColumnLefts[iCount], iTopMargin), new Point((int)arrColumnLefts[iCount], iTopMargin + iCellHeight));
                                e.Graphics.DrawLine(Pens.Black, new Point((int)arrColumnLefts[iCount], iTopMargin + iCellHeight), new Point((int)arrColumnLefts[4], iTopMargin + iCellHeight));
                            }
                            else
                            {
                                e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)arrColumnLefts[iCount],
                                        iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));
                            }

                            iCount++;
                        }
                    }
                    iRow++;
                    iTopMargin += iCellHeight;
                }

                //If more lines exist, print another page.
                if (bMorePagesToPrint)
                {
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region 用户实时在线
        private void keepOnlineTimer_Tick(object sender, EventArgs e)
        {
            if (MainWindow.IS_LOGED_IN)
            {
                try
                {
                    DatabaseConnections.GetInstence().OnlineUpdateDataFromOriginalSQL("UPDATE users SET GZB_isonline = 1, GZB_lastlogontime = NOW(), GZB_logonmins = GZB_logonmins+1 WHERE userid = '" + MainWindow.USER_ID + "'");
                }
                catch { return; }
            }
        }

        #endregion

        #region 状态栏通知
        private int currentImageIndex = 0;  // 当前通知栏图标在ImageList中的索引

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            // 最小化窗体时，隐藏任务栏
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void ShowMainWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowMainWindow();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon_MouseClick(sender, e);
        }

        /// <summary>
        /// 通知图标-->显示主窗体
        /// </summary>
        private void showWindow_Click(object sender, EventArgs e)
        {
            ShowMainWindow();
        }

        /// <summary>
        /// 通知图标-->退出
        /// </summary>
        private void exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要退出管账宝?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void notifyBlinkTimer_Tick(object sender, EventArgs e)
        {
            currentImageIndex = 1 - currentImageIndex;
            SetNotifyIcon(currentImageIndex);
        }

        public void notifyBlink(Boolean isBlink)
        {
            if (isBlink)
            {
                if (!this.notifyBlinkTimer.Enabled)
                {
                    this.notifyBlinkTimer.Enabled = true;
                    SetNotifyIcon(0);
                }
            }
            else
            {
                this.notifyBlinkTimer.Enabled = false;
                SetNotifyIcon(0);
            }
        }

        private void btnFlicker_Click(object sender, EventArgs e)
        {
            if (this.notifyBlinkTimer.Enabled)
            {
                // this.btnFlicker.Text = "闪动图标";
                this.notifyBlinkTimer.Enabled = false;
                SetNotifyIcon(0);
            }
            else
            {
                // this.btnFlicker.Text = "停止闪动";
                this.notifyBlinkTimer.Enabled = true;
            }
        }

        /// <summary>
        /// 设置托盘显示的图标
        /// </summary>
        /// <param name="index">图像列表中图片的索引</param>
        private void SetNotifyIcon(int index)
        {
            Image img = this.notifyImageList.Images[index];
            Bitmap b = new Bitmap(img);
            Icon icon = Icon.FromHandle(b.GetHicon());
            this.notifyIcon.Icon = icon;
        }

        private void ClearNotifyIconContextMenuStripItems()
        {
            for (int i = 0; i < notifyIconContextMenuStrip.Items.Count - 3; i++)
            {
                notifyIconContextMenuStrip.Items.RemoveAt(i + 1);
            }
        }

        public void addNotifyItem(String id, String sender)
        {
            notifyBlink(true);
            ToolStripMenuItem newMenuItem = new ToolStripMenuItem(id + "发来远程签单请求", VividManagementApplication.Properties.Resources.Signature);
            newMenuItem.Tag = sender;
            newMenuItem.Click += new EventHandler(mnuNotify_Click);
            notifyIconContextMenuStrip.Items.Insert(1, newMenuItem);
        }

        private void mnuNotify_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("发送人:" + (sender as ToolStripMenuItem).Tag);
            ViewRemoteSignDetail((sender as ToolStripMenuItem).Tag.ToString());
            notifyIconContextMenuStrip.Items.Remove((sender as ToolStripMenuItem));
            if (notifyIconContextMenuStrip.Items.Count < 3)
            {
                notifyBlink(false);
            }
        }
        #endregion

        private void ExtendExpireLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.vividapp.net/");
        }

        private void lbUserName_Click(object sender, EventArgs e)
        {
            settingQQButton.PerformClick();
        }

        #region 远程签单

        private void updateRemoteSignUndealedCountCheckWithObject(Object obj)
        {
            updateRemoteSignUndealedCountCheck();
        }
        private void updateRemoteSignUndealedCountCheck()
        {
            List<List<String>> remoteSignUndealedList = DatabaseConnections.GetInstence().OnlineGetRowsDataById("gzb_remotesign", new List<String>() { "Id", "fromGZBID", "toGZBID", "companyNickName", "isSigned" }, "isSigned", "0", " AND (toGZBID ='" + MainWindow.USER_ID + "' OR fromGZBID='" + MainWindow.USER_ID + "')");
            NotifyToolStripStatusLabel.Text = "您有" + remoteSignUndealedList.Count + "条未处理信息";
        }

        private void updateRemoteSignTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //if (!this.InvokeRequired)
            //{
            //    checkRemoteSignList();
            //}
            //else
            //{
            //    this.Invoke(new MethodInvoker(() => { checkRemoteSignList(); }));
            //}
            Thread t = new Thread(new ParameterizedThreadStart(checkRemoteSignListWithObject));
            t.Start();
            t.DisableComObjectEagerCleanup();
        }

        private void checkRemoteSignListWithObject(object obj)
        {
            checkRemoteSignList();
        }

        private void checkRemoteSignList()
        {
            try
            {
                List<List<String>> remoteSignList = DatabaseConnections.GetInstence().OnlineGetRowsDataById("gzb_remotesign", new List<String>() { "Id", "fromGZBID", "toGZBID", "companyNickName", "isSigned", "signValue", "sendTime", "signTime", "refusedMessage" }, "toGZBID", MainWindow.USER_ID, " OR fromGZBID='" + MainWindow.USER_ID + "'");
                int remoteSignUndealedListCount = 0;
                //List<List<String>> remoteSignUndealedList = DatabaseConnections.GetInstence().OnlineGetRowsDataById("gzb_remotesign", new List<String>() { "Id" }, "isSigned", "0", " AND (toGZBID ='" + MainWindow.USER_ID + "')");

                DatabaseConnections.GetInstence().LocalClearTable("remoteSign");
                ClearNotifyIconContextMenuStripItems();

                if (remoteSignList.Count != 0)
                {
                    foreach (List<String> item in remoteSignList)
                    {
                        if (item[4].Equals("0") && item[2].Equals(MainWindow.USER_ID))
                        {
                            addNotifyItem(item[1], item[0]);
                            remoteSignUndealedListCount++;
                        }
                        DatabaseConnections.GetInstence().LocalReplaceIntoData("remoteSign", (new List<String>() { "Id", "fromGZBID", "toGZBID", "companyNickName", "isSigned", "signValue", "sendTime", "signTime", "refusedMessage" }).ToArray(), item.ToArray(), MainWindow.USER_ID);
                    }
                }

                NotifyToolStripStatusLabel.Text = "您有" + remoteSignUndealedListCount + "条未处理远程签单";
                listQdButton.PerformClick();
            }
            catch { return; }
        }

        #endregion

        private void StatusToolStripStatusLabel_TextChanged(object sender, EventArgs e)
        {
            lablTextChangeTimer.Elapsed += new System.Timers.ElapsedEventHandler(SetStatusToolStripStatusLabel);//到达时间的时候执行事件；  
            lablTextChangeTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；  
            lablTextChangeTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；  
            if (this.IsDisposed)
            {
                lablTextChangeTimer.Stop();
            }
        }
        private void SetStatusToolStripStatusLabel(object sender, System.Timers.ElapsedEventArgs e)
        {
            StatusToolStripStatusLabel.Text = "消息";
            lablTextChangeTimer.Enabled = false;
        }


    }
}
