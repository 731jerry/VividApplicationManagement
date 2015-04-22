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
        public static String REAL_NAME = "";
        public static String WORKLOADS = "";
        public static String COMPANY_NAME = "";
        public static String COMPANY_OWNER = "";
        public static String ADDRESS = "";
        public static String BANK_NAME = "";
        public static String BANK_CARD = "";
        public static String PHONE = "";
        public static String FAX = "";
        public static String QQ = "";
        public static String EMAIL = "";
        public static String ADDTIME = "";
        public static String NOTIFICATION = "";
        public static DateTime EXPIRETIME = new DateTime();
        public static int COMPANY_BALANCE = 0; // 公司结余暂存

        public static String LOCAL_DATABASE_LOCATION = Environment.CurrentDirectory + "\\data\\data.db";
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

        System.Timers.Timer updateDataTimersTimer;

        string dataBaseFilePrefix;

        public MainWindow()
        {
            InitializeComponent();
            //Control.CheckForIllegalCrossThreadCalls = false;//这一行是关键 
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            this.Visible = false;
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

            #region 软件版本
            productKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(PRODUCT_REG_KEY);
            String loginWindowLabel = "登录";
            try
            {
                CURRENT_APP_NAME = productKey.GetValue("DisplayName").ToString();
                CURRENT_APP_VERSION_NAME = productKey.GetValue("VersionName").ToString(); ;
                CURRENT_APP_VERSION_ID = productKey.GetValue("DisplayVersion").ToString();

                loginWindowLabel = CURRENT_APP_NAME + "(" + CURRENT_APP_VERSION_NAME + ")v" + CURRENT_APP_VERSION_ID;

                this.Text = CURRENT_APP_NAME + " " + CURRENT_APP_VERSION_NAME + "v" + CURRENT_APP_VERSION_ID;
            }
            catch (Exception exc)
            {

            }
            #endregion

            #region 登录
            Login loginWindow = new Login();
            this.Visible = false;
            loginWindow.Text = loginWindowLabel;
            loginWindow.ShowDialog(this);
            #endregion

            if (MainWindow.IS_LOGED_IN)
            {
                #region 初始化数据库
                if (!File.Exists(MainWindow.LOCAL_DATABASE_LOCATION))
                {
                    DatabaseConnections.GetInstence().LocalCreateDatabase();
                }
                File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                #endregion

                #region 窗体用户信息初始化
                lbUserName.Text = USER_ID;
                dataBaseFilePrefix = USER_ID + "_data.txt";

                // 广告计时器
                //commerceTimer.Enabled = true;

                #region 更新数据库计时器
                //DownloadFileWithNotice();

                updateDataTimer.Enabled = true;

                //Thread t = new Thread(new ParameterizedThreadStart(DownloadFileWithNoticeWithObject));
                //t.Start();

                //updateDataTimersTimer = new System.Timers.Timer(1000);
                //updateDataTimersTimer.Elapsed += new System.Timers.ElapsedEventHandler(updateDataTimersTimer_Elapsed);//到达时间的时候执行事件；  
                //updateDataTimersTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；  
                //updateDataTimersTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；  
                //if (this.IsDisposed)
                //{
                //    updateDataTimersTimer.Stop();
                //}  

                //using (BackgroundWorker bw = new BackgroundWorker())
                //{
                //    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                //    bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                //    bw.RunWorkerAsync("Hello World");
                //}

                //this.Invoke(new MethodInvoker(() =>
                //{
                //    DownloadFileWithNotice();
                //}));
                #endregion

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
                cxRadio.Checked = false;
                cxRadio.Checked = true;
                //cxRadio.PerformClick();
                listCxButton.PerformClick();
                //ViewButton.Enabled = false;
                //refeshButton.Enabled = false;
                //PrintButton.Enabled = false;
                #endregion

                #region 初始化登录信息
                lbExpireTime.Text = EXPIRETIME.ToLongDateString();
                keepOnlineTimer.Enabled = true;
                #endregion

                // 状态栏通知
                notifyIcon.Visible = true;
                SetNotifyIcon(currentImageIndex);
            }
            else
            {
                this.Visible = false;
                this.Close();
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
            //di.ShowIcon = false;
            di.Text = "新建";
            di.ShowDialog();
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            if (MainDataGridView.Rows.Count > 0)
            {
                DetailedInfo di = new DetailedInfo();
                //di.ShowIcon = false;
                //di.ItemId = this.MainListView.SelectedItems[0].Text;
                di.ItemId = this.MainDataGridView.SelectedRows[0].Cells[0].Value.ToString();
                di.Text = "查看" + di.ItemId;
                di.ShowDialog();
            }
        }

        #region 上传下载数据库文件 同步数据库
        private void updateDataTimersTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!this.InvokeRequired)
            {
                DownloadFileWithNotice();
            }
            else
            {
                this.Invoke(new MethodInvoker(() => { DownloadFileWithNotice(); }));
            }
        }

        private void DownloadFileWithNoticeWithObject(object obj)
        {
            DownloadFileWithNotice();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            //MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
            //e.Result = e.Argument;//这里只是简单的把参数当做结果返回，当然您也可以在这里做复杂的处理后，再返回自己想要的结果(这里的操作是在另一个线程上完成的)
            //DownloadFileWithNotice();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //这时后台线程已经完成，并返回了主线程，所以可以直接使用UI控件了
            //this.textBox1.Text = e.Result.ToString();
            //MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
            DownloadFileWithNotice();
        }

        private void updateDataTimer_Tick(object sender, EventArgs e)
        {
            updateDataTimer.Enabled = false;
            // 暂时备份
            DownloadFileWithNotice();
        }

        private void backupData_Click(object sender, EventArgs e)
        {
            pbUploadDownloadFile.Visible = true;
            UploadFileWithNotice("手动同步数据库！");
        }

        String UploadMoreInfo;

        // 下载
        private void DownloadFileWithNotice()
        {
            if (UriExists(ONLINE_DATABASE_LOCATION_DIR + dataBaseFilePrefix))
            {
                if (ifUpdateDatabasecheckLastModifiedTime(true))
                {
                    File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Normal);
                    DownloadFile(ONLINE_DATABASE_LOCATION_DIR + dataBaseFilePrefix, LOCAL_DATABASE_LOCATION);
                    File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                }
            }
            else
            {
                UploadFileWithNotice("初次同步, ");
            }
        }

        private void DownloadFile(String uriString, String fileNamePath)
        {
            visibleUploadDownloadGroup(true);
            WebClient client = new WebClient();
            Uri uri = new Uri(uriString);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleteCallback);
            client.DownloadFileAsync(uri, fileNamePath);

        }
        private void DownloadProgressCallback(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            pbUploadDownloadFile.Value = e.ProgressPercentage;
        }

        private void DownloadFileCompleteCallback(Object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);   //正常捕获
            }
            else
            {
                FormBasicFeatrues.GetInstence().SoundPlay(System.Environment.CurrentDirectory + @"\config\complete.wav");
                MessageBox.Show(UploadMoreInfo + "同步成功!", "成功");
            }
            visibleUploadDownloadGroup(false);
        }

        // 上传
        private void UploadFileWithNotice(String moreInfo)
        {
            UploadMoreInfo = moreInfo;
            if (getLocalFileSize(LOCAL_DATABASE_LOCATION) > 0)
            {
                if (ifUpdateDatabasecheckLastModifiedTime(false))
                {
                    UploadFile(LOCAL_DATABASE_LOCATION, ONLINE_DATABASE_FTP_LOCATION_DIR + dataBaseFilePrefix);
                }
            }
        }

        private void UploadFile(String fileNamePath, String uriString)
        {
            visibleUploadDownloadGroup(true);
            WebClient client = new WebClient();
            Uri uri = new Uri(uriString);
            client.UploadProgressChanged += new UploadProgressChangedEventHandler(UploadProgressCallback);
            client.UploadFileCompleted += new UploadFileCompletedEventHandler(UploadFileCompleteCallback);
            client.UploadFileAsync(uri, "STOR", fileNamePath);
            // client.Proxy = WebRequest.DefaultWebProxy;
            //client.Proxy.Credentials = new NetworkCredential(ONLINE_FTP_USERNAME, ONLINE_FTP_PASSWORD, ONLINE_FTP_DOMAIN);

        }
        private void UploadProgressCallback(object sender, System.Net.UploadProgressChangedEventArgs e)
        {
            pbUploadDownloadFile.Value = e.ProgressPercentage;
        }

        private void UploadFileCompleteCallback(Object sender, UploadFileCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);   //正常捕获
            }
            else
            {
                MessageBox.Show(UploadMoreInfo + "同步成功!", "成功");
            }
            visibleUploadDownloadGroup(false);
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
            HttpWebRequest gameFile = (HttpWebRequest)WebRequest.Create(ONLINE_DATABASE_LOCATION_DIR + dataBaseFilePrefix);
            HttpWebResponse gameFileResponse = (HttpWebResponse)gameFile.GetResponse();

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
            pbUploadDownloadFile.Visible = visible;
            pbUploadDownloadFile.Value = 0;
            pbUploadDownloadLabel.Visible = visible;
            //pbUploadDownloadLabel.Text = notice;
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

        private void commerceTimer_Tick(object sender, EventArgs e)
        {
            //Image[] images = new Image[2] { Image.FromFile(System.Environment.CurrentDirectory + @"\config\commerce\commerce1.png"), Image.FromFile(System.Environment.CurrentDirectory + @"\config\commerce\commerce2.png") };
            //CommercePictureBox.Image = images[0];
            //ImageClass.DanRu(new Bitmap(images[0]), CommercePictureBox);
            //CommercePictureBox.Image = images[1];
            //ImageClass.KuoSan(new Bitmap(images[1]), CommercePictureBox);
            //CommercePictureBox.Image = images[0];
            //System.Threading.Thread.Sleep(1000);
            //CommercePictureBox.Image = images[1];
        }

        private void refeshButton_Click(object sender, EventArgs e)
        {
            CURRENT_LIST_BUTTON.PerformClick();
            MessageBox.Show("刷新成功！");
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
            ableSubButtons(new List<QQButton>() { newCxButton, listCxButton }, cxRadio);
            listCxButton.PerformClick(); ;
        }

        private void spRadio_CheckedChanged(object sender, EventArgs e)
        {
            CURRENT_TAB = 2;
            ableSubButtons(new List<QQButton>() { newSpButton, listSpButton }, spRadio);
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
        }

        // 收支汇总表
        private void CreateMainDataGridViewPZ(DataGridViewColumn[] dgvcArray, string table, string[] queryArray)
        {
            string order = " ORDER BY id ASC "; // 顺序
            this.MainDataGridView.Columns.Clear();
            this.MainDataGridView.Rows.Clear();
            int tempBalance = COMPANY_BALANCE;

            this.MainDataGridView.Columns.AddRange(dgvcArray);
            ClearDataGridViewColumnSortOrder(dgvcArray.Length);
            List<string[]> resultsList = DatabaseConnections.GetInstence().LocalGetData(table, queryArray, order);

            for (int i = 0; i < resultsList.Count; i++)
            {
                if (resultsList[i][6].Equals("否"))
                {
                    if ((resultsList[i][2].Equals("收款凭证")) || resultsList[i][2].Equals("还款凭证"))
                    {
                        tempBalance += int.Parse(resultsList[i][4]);
                    }
                    else
                    {
                        tempBalance -= int.Parse(resultsList[i][4]);
                    }
                    resultsList[i][5] = tempBalance.ToString();
                }

            }
            resultsList.Reverse();
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
        }

        // DataGridView双击

        private void MainDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ViewButton.Enabled && e.RowIndex != -1)
            {
                ViewButton.PerformClick();
            }
        }

        // 客户管理
        private void listCxButton_Click(object sender, EventArgs e)
        {
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

            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 }, "clients", -1,
                new string[] { "clientID", "company", "address", "companyOwner", "phone" });
        }

        // 商品管理
        private void listSpButton_Click(object sender, EventArgs e)
        {
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
            refeshButton.Enabled = true;
            ViewButton.Enabled = true;
            PrintButton.Enabled = false;
            CURRENT_LIST_BUTTON = listXsButton;
            CURRENT_TAB = 4;
            mainDGVTitle.Text = listCgButton.Text;
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
            refeshButton.Enabled = true;
            ViewButton.Enabled = true;
            PrintButton.Enabled = false;
            CURRENT_LIST_BUTTON = listSfzhButton;
            CURRENT_TAB = 5;
            mainDGVTitle.Text = listSfzhButton.Text;
            Column1.HeaderText = "凭证号码";
            Column2.HeaderText = "日期";
            Column3.HeaderText = "凭证类型";
            Column4.HeaderText = "对方单位名称";
            Column5.HeaderText = "交易金额";
            Column6.HeaderText = "结余金额";
            Column7.HeaderText = "作废标识";

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

        // 客户对账单
        private void listKhdzButton_Click(object sender, EventArgs e)
        {
            refeshButton.Enabled = true;
            ViewButton.Enabled = false;
            PrintButton.Enabled = true;
            this.MainDataGridView.Rows.Clear();
            this.MainDataGridView.Columns.Clear();

            Filter flt = new Filter();
            flt.ShowDialog();

            if (flt.FilterOKClickedIndex == 1) // OK键
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

                Column1.Width = 200;
                Column2.Width = 200;
                Column3.Width = 100;
                Column4.Width = 100;
                Column5.Width = 100;
                Column6.Width = 100;

                Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                int tempBalance = COMPANY_BALANCE;

                DataGridViewColumn[] dgvcArray = new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6 };
                this.MainDataGridView.Columns.AddRange(dgvcArray);
                ClearDataGridViewColumnSortOrder(dgvcArray.Length);
                List<string[]> resultsList = DatabaseConnections.GetInstence().LocalGetDataFromOriginalSQL(
                    "SELECT pzID, cast (modifyTime as VARCHAR) as modifyTime"
                    + ",case when leixing = '0' then '收款凭证' when leixing = '1' then '付款凭证' when leixing = '2' then '领款凭证' when leixing = '3' then '还款凭证' else '报销凭证' end as 'leixing'"
                    + ",zhaiyao"
                    + ",operateMoney,remaintingMoney FROM pzList WHERE clientID = '" + flt.clientID.Text + "' AND discardFlag = 0"
                    + " AND (modifyTime BETWEEN '" + flt.fromDate.Value.ToShortDateString() + "' AND '" + flt.toDate.Value.ToShortDateString() + "')"
                    + " ORDER BY modifyTime ASC",
                    new String[] { "pzID", "modifyTime", "leixing", "zhaiyao", "operateMoney", "remaintingMoney" });

                for (int i = 0; i < resultsList.Count; i++)
                {
                    if ((resultsList[i][2].Equals("收款凭证")) || resultsList[i][2].Equals("还款凭证"))
                    {
                        tempBalance += int.Parse(resultsList[i][4]);
                    }
                    else
                    {
                        tempBalance -= int.Parse(resultsList[i][4]);
                    }
                    resultsList[i][5] = tempBalance.ToString();
                }
                resultsList.Reverse();
                for (int j = 0; j < resultsList.Count; j++)
                {
                    this.MainDataGridView.Rows.Add(resultsList[j]);
                }
            }
            else if (flt.FilterOKClickedIndex == 0) // 取消键
            {
                listSfzhButton.PerformClick(); // 切换到收付汇总表
            }
            else
            {
            }
        }

        // 合同列表
        private void listHtButton_Click(object sender, EventArgs e)
        {
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
                File.SetAttributes(MainWindow.LOCAL_DATABASE_LOCATION, FileAttributes.Hidden);
                DatabaseConnections.GetInstence().OnlineUpdateDataFromOriginalSQL("UPDATE users SET GZB_isonline = 0 WHERE userid = '" + MainWindow.USER_ID + "'");
                UploadFileWithNotice("关闭前同步数据库!");

                if (notifyIcon != null)
                {
                    notifyIcon.Visible = false;
                    notifyIcon.Icon = null; // required to make icon disappear
                    notifyIcon.Dispose();
                    notifyIcon = null;
                }
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
                foreach (DataGridViewColumn dgvGridCol in MainDataGridView.Columns)
                {
                    iTotalWidth += dgvGridCol.Width;
                }
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

                //For the first page to print set the cell width and header height
                if (bFirstPage)
                {
                    foreach (DataGridViewColumn GridCol in MainDataGridView.Columns)
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
                while (iRow <= MainDataGridView.Rows.Count - 1)
                {
                    DataGridViewRow GridRow = MainDataGridView.Rows[iRow];
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
                            e.Graphics.DrawString("- " + pageIndex.ToString() + " -", new Font(MainDataGridView.Font, FontStyle.Bold),
                                   Brushes.Black, this.printDocument1.DefaultPageSettings.PaperSize.Width / 2 - fontSize.Width / 2,
                                   this.printDocument1.DefaultPageSettings.PaperSize.Height - 45);

                            // 页脚
                            fontSize = e.Graphics.MeasureString("此账单版本由唯达软件系统提供 http://www.vividapp.net/   软件定制电话: 15024345993   QQ: 70269387", new Font("微软雅黑", 7));
                            e.Graphics.DrawString("此账单版本由唯达软件系统提供 http://www.vividapp.net/   软件定制电话: 15024345993   QQ: 70269387", new Font("微软雅黑", 7), new SolidBrush(Color.Red),
                                this.printDocument1.DefaultPageSettings.PaperSize.Width / 2 - fontSize.Width / 2,
                                this.printDocument1.DefaultPageSettings.PaperSize.Height - 65);

                            //Draw Header
                            e.Graphics.DrawString(leftString, new Font(MainDataGridView.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
                                    e.Graphics.MeasureString(leftString, new Font(MainDataGridView.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Height + 15);

                            //Draw Date
                            e.Graphics.DrawString(rightString, new Font(MainDataGridView.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                    e.Graphics.MeasureString(rightString, new Font(MainDataGridView.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top -
                                    e.Graphics.MeasureString(leftString, new Font(new Font(MainDataGridView.Font,
                                    FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height + 15);

                            //Draw Columns                 
                            iTopMargin = e.MarginBounds.Top + 20; // 表格位置 原本为0
                            foreach (DataGridViewColumn GridCol in MainDataGridView.Columns)
                            {
                                e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                                    new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                e.Graphics.DrawRectangle(Pens.Black,
                                    new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                e.Graphics.DrawString(GridCol.HeaderText, GridCol.InheritedStyle.Font,
                                    new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                    new RectangleF((int)arrColumnLefts[iCount], iTopMargin,
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
                                e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font,
                                            new SolidBrush(Cel.InheritedStyle.ForeColor),
                                            new RectangleF((int)arrColumnLefts[iCount], (float)iTopMargin,
                                            (int)arrColumnWidths[iCount], (float)iCellHeight), strFormat);
                            }
                            //Drawing Cells Borders 
                            e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)arrColumnLefts[iCount],
                                    iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));

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
                DatabaseConnections.GetInstence().OnlineUpdateDataFromOriginalSQL("UPDATE users SET GZB_isonline = 1, GZB_lastlogontime = NOW(), GZB_logonmins = GZB_logonmins+1 WHERE userid = '" + MainWindow.USER_ID + "'");
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
            this.Close();
        }

        private void notifyBlinkTimer_Tick(object sender, EventArgs e)
        {
            currentImageIndex = 1 - currentImageIndex;
            SetNotifyIcon(currentImageIndex);
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

        private void qqButton2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem newMenuItem = new ToolStripMenuItem("通知", VividManagementApplication.Properties.Resources.Signature);
            newMenuItem.Click += new EventHandler(mnuCopy_Click);
            notifyIconContextMenuStrip.Items.Insert(1, newMenuItem);
            //notifyIconContextMenuStrip.Items.AddRange(new ToolStripItem[] { newMenuItem });
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ahah");
        }
        #endregion
    }
}
