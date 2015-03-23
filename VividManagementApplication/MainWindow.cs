using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ControlExs;
using System.Drawing.Drawing2D;
using System.Web;
using System.Net;
using System.IO;

using MengYu.Image;

namespace VividManagementApplication
{
    public partial class MainWindow : FormEx
    {
        public static int CURRENT_TAB = 1;
        public static QQButton CURRENT_LIST_BUTTON;

        public static bool IS_LOGED_IN;
        public static string LOGIN_ID;
        public static string NAME;
        public static string NICK_NAME;
        public static string NOTIFICATION;
        public static string LAST_LOGON_TIME;

        string dataBaseFilePrefix;
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Override

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    cp.ExStyle |= (int)WindowStyle.WS_CLIPCHILDREN;
                }
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawFromAlphaMainPart(this, e.Graphics);
        }

        #endregion

        #region Private

        /// <summary>
        /// 绘制窗体主体部分白色透明层
        /// </summary>
        /// <param name="form"></param>
        /// <param name="g"></param>
        public static void DrawFromAlphaMainPart(Form form, Graphics g)
        {
            Color[] colors = 
            {
                Color.FromArgb(5, Color.White),
                Color.FromArgb(60, Color.White),
                Color.FromArgb(145, Color.White),
                Color.FromArgb(150, Color.White),
                Color.FromArgb(60, Color.White),
                Color.FromArgb(5, Color.White)
            };

            float[] pos = 
            {
                0.0f,
                0.04f,
                0.10f,
                0.90f,
                0.97f,
                1.0f      
            };

            ColorBlend colorBlend = new ColorBlend(6);
            colorBlend.Colors = colors;
            colorBlend.Positions = pos;

            RectangleF destRect = new RectangleF(0, 0, form.Width, form.Height);
            using (LinearGradientBrush lBrush = new LinearGradientBrush(destRect, colors[0], colors[5], LinearGradientMode.Vertical))
            {
                lBrush.InterpolationColors = colorBlend;
                g.FillRectangle(lBrush, destRect);
            }
        }


        private void SetStyles()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        #endregion

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;

            cxRadio.Checked = true;

            #region 登录
            Login loginWindow = new Login();
            this.Visible = false;
            loginWindow.ShowDialog(this);
            #endregion

            #region 初始化客户列表
            listCxButton.PerformClick();
            #endregion

            #region 窗体用户信息初始化
            lbUserName.Text = NICK_NAME;
            lbLastLogonTime.Text = LAST_LOGON_TIME;
            dataBaseFilePrefix = LOGIN_ID + "_";

            // 广告计时器
            //commerceTimer.Enabled = true;
            // 更新数据库计时器
            updateDataTimer.Enabled = true;
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
            DetailedInfo di = new DetailedInfo();
            //di.ShowIcon = false;
            //di.ItemId = this.MainListView.SelectedItems[0].Text;
            di.ItemId = this.MainDataGridView.SelectedRows[0].Cells[0].Value.ToString();
            di.Text = "新建" + di.ItemId;
            di.ShowDialog();
        }

        // 测试
        private void qqButton1_Click(object sender, EventArgs e)
        {
        }

        private void backupData_Click(object sender, EventArgs e)
        {
            UploadFiles("手动备份数据库！");
        }


        public void UploadFiles(string moreInfo)
        {
            string fileName = System.Environment.CurrentDirectory + @"\data\data.db";
            try
            {
                FormBasicFeatrues.GetInstence().UpLoadFile(fileName, "", "ftp://vividappftp:vividappftp@www.vividapp.net/Project/VMA/Users/00000000/" + dataBaseFilePrefix + "data.txt");
                //FormBasicFeatrues.GetInstence().UpLoadFile(fileName, "", "ftp://qyw28051:cyy2014@qyw28051.my3w.com/products/caiYY/backup/" + dataBaseFilePrefix + "data.txt");
                MessageBox.Show(moreInfo + "数据库备份成功!", "成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "上传文件出现异常");
            }
        }

        Stream srm;
        StreamReader srmReader;
        public void DownLoadFile()
        {
            string urlName = "http://www.vividapp.net/Project/VMA/Users/00000000/" + dataBaseFilePrefix + "data.txt";
            //string urlName = "http://www.caiyingying.com/products/caiYY/backup/" + dataBaseFilePrefix + "data.txt";
            //string urlName = "http://www.caiyingying.com/products/caiYY/backup/test.txt";
            try
            {
                WebClient wcClient = new WebClient();

                long fileLength = 0;

                WebRequest webReq = WebRequest.Create(urlName);
                WebResponse webRes = webReq.GetResponse();

                pbDownFile.Visible = true;
                fileLength = webRes.ContentLength;

                pbDownFile.Value = 0;
                pbDownFile.Maximum = (int)fileLength;

                srm = webRes.GetResponseStream();
                srmReader = new StreamReader(srm);

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
                    pbDownFile.Value += downByte;

                    //Text = "数据库更新 [" + Convert.ToInt32(pbDownFile.Value / (float)pbDownFile.Maximum * 100) + "%]";

                    // float part = (float)startByte / 1024;
                    //float total = (float)bufferbyte.Length / 1024;
                    //int percent = Convert.ToInt32((part / total) * 100);

                }

                using (FileStream fs = new FileStream(System.Environment.CurrentDirectory + @"\data\data.db", FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bufferbyte, 0, bufferbyte.Length);
                }

                webRes.Close();
                srm.Close();
                srmReader.Close();
                pbDownFile.Visible = false;
                FormBasicFeatrues.GetInstence().SoundPlay(System.Environment.CurrentDirectory + @"\config\complete.wav");
                MessageBox.Show("数据库更新成功!", "成功");
            }
            catch (WebException webE)
            {
                if (webE.Status == WebExceptionStatus.ProtocolError)
                {
                    UploadFiles("初次备份, ");
                }
            }
            catch (Exception e)
            {
                if (MessageBox.Show("(" + urlName + ")错误信息:" + e.Message, "更新出现错误", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    this.Close();
                }
            }
        }

        private void updateDataTimer_Tick(object sender, EventArgs e)
        {
            updateDataTimer.Enabled = false;
            // 暂时备份
            //DownLoadFile();
        }

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
            Image[] images = new Image[2] { Image.FromFile(System.Environment.CurrentDirectory + @"\config\commerce\commerce1.png"), Image.FromFile(System.Environment.CurrentDirectory + @"\config\commerce\commerce2.png") };
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
            ableSubButtons(new List<QQButton>() { newJCcButton, listKcButton, listJcdButton, listCcdButton }, ccRadio);
            listKcButton.PerformClick();
        }

        private void ywRadio_CheckedChanged(object sender, EventArgs e)
        {
            CURRENT_TAB = 4;
            ableSubButtons(new List<QQButton>() { newCgZsButton, listCgXsButton, listKhdzButton }, ywRadio);
            listCgXsButton.PerformClick();
        }

        private void cwRadio_CheckedChanged(object sender, EventArgs e)
        {
            CURRENT_TAB = 5;
            ableSubButtons(new List<QQButton>() { newPzButton, listSfzhButton }, cwRadio);
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

        private void CreateMainDataGridView(DataGridViewColumn[] dgvcArray, string table, string[] queryArray)
        {
            string order = " ORDER BY id ASC ";
            this.MainDataGridView.Columns.Clear();
            this.MainDataGridView.Rows.Clear();

            this.MainDataGridView.Columns.AddRange(dgvcArray);
            List<string[]> resultsList = DatabaseConnections.GetInstence().LocalGetData(table, queryArray, order);
            for (int i = 0; i < resultsList.Count; i++)
            {
                this.MainDataGridView.Rows.Add(resultsList[i]);
            }
        }

        private void listCxButton_Click(object sender, EventArgs e)
        {
            CURRENT_LIST_BUTTON = listCxButton;
            CURRENT_TAB = 1;
            mainDGVTitle.Text = listCxButton.Text;
            Column1.HeaderText = "客户编号";
            Column2.HeaderText = "客户名称";
            Column3.HeaderText = "联系地址";
            Column4.HeaderText = "联系人";
            Column5.HeaderText = "联系电话";

            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 }, "clients", new string[] { "id", "name", "address", "contact", "phone" });
        }

        private void listSpButton_Click(object sender, EventArgs e)
        {
            CURRENT_LIST_BUTTON = listSpButton;
            CURRENT_TAB = 2;
            mainDGVTitle.Text = listSpButton.Text;
            Column1.HeaderText = "商品编号";
            Column2.HeaderText = "商品名称";
            Column3.HeaderText = "规格";
            Column4.HeaderText = "等级";
            Column5.HeaderText = "单位";
            Column6.HeaderText = "单价";
            Column7.HeaderText = "备注";

            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7 }, "goods", new string[] { "id", "name", "guige", "dengji", "unit", "currntsalesPrice", "beizhu" });
        }

        private void listKcButton_Click(object sender, EventArgs e)
        {
            CURRENT_LIST_BUTTON = listKcButton;
            CURRENT_TAB = 3;
            mainDGVTitle.Text = listKcButton.Text;
            Column1.HeaderText = "商品编号";
            Column2.HeaderText = "商品名称";
            Column3.HeaderText = "规格";
            Column4.HeaderText = "等级";
            Column5.HeaderText = "单位";
            Column6.HeaderText = "库存数量";
            Column7.HeaderText = "单价";
            Column8.HeaderText = "备注";

            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8 }, "goods", new string[] { "id", "name", "guige", "dengji", "currentCount", "currntsalesPrice", "beizhu" });
        }

        private void listJcdButton_Click(object sender, EventArgs e)
        {
            CURRENT_LIST_BUTTON = listJcdButton;
            CURRENT_TAB = 3;
            mainDGVTitle.Text = listJcdButton.Text;
            Column1.HeaderText = "单号";
            Column2.HeaderText = "单位名称";
            Column3.HeaderText = "货号";
            Column4.HeaderText = "商品名称";
            Column5.HeaderText = "规格";
            Column6.HeaderText = "作废标识";

            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6 }, "jcdList", new string[] { "jcdID", "companyName", "goodsIDs", "goodsName", "guige", "discardFlag" });
        }

        private void listCcdButton_Click(object sender, EventArgs e)
        {
            CURRENT_LIST_BUTTON = listCcdButton;
            CURRENT_TAB = 3;
            mainDGVTitle.Text = listCcdButton.Text;
            Column1.HeaderText = "单号";
            Column2.HeaderText = "单位名称";
            Column3.HeaderText = "货号";
            Column4.HeaderText = "商品名称";
            Column5.HeaderText = "规格";
            Column6.HeaderText = "作废标识";

            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6 }, "ccdList", new string[] { "ccdID", "companyName", "goodsIDs", "goodsName", "guige", "discardFlag" });
        }

        private void listCgXsButton_Click(object sender, EventArgs e)
        {
            CURRENT_LIST_BUTTON = listCgXsButton;
            CURRENT_TAB = 4;
            mainDGVTitle.Text = listCgXsButton.Text;
            Column1.HeaderText = "日期";
            Column2.HeaderText = "凭证号码";
            Column3.HeaderText = "类型";
            Column4.HeaderText = "摘要";
            Column5.HeaderText = "金额";
            Column6.HeaderText = "经办人";
            Column7.HeaderText = "√";

            Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7 }, "cgxsYWList", new string[] { "addtime", "cgxsID", "leixing", "clientIDs", "price", "operater", "discardFlag" });
        }

        private void listKhdzButton_Click(object sender, EventArgs e)
        {
            // 还未完成
            CURRENT_LIST_BUTTON = listKhdzButton;
            CURRENT_TAB = 4;
            mainDGVTitle.Text = listKhdzButton.Text;
        }

        private void listSfzhButton_Click(object sender, EventArgs e)
        {
            CURRENT_LIST_BUTTON = listSfzhButton;
            CURRENT_TAB = 5;
            mainDGVTitle.Text = listSfzhButton.Text;
            Column1.HeaderText = "日期";
            Column2.HeaderText = "凭证号码";
            Column3.HeaderText = "摘要";
            Column4.HeaderText = "收入金额";
            Column6.HeaderText = "付出金额";
            Column7.HeaderText = "√";
            Column8.HeaderText = "结存金额";

            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8 }, "pzList", new string[] { "addtime", "pzID", "zhaiyao", "cost", "fujianCount", "discardFlag", "discardFlag" });
        }

        private void listHtButton_Click(object sender, EventArgs e)
        {
            // 还未完成 合同
            CURRENT_LIST_BUTTON = listHtButton;
            CURRENT_TAB = 6;
            mainDGVTitle.Text = listHtButton.Text;
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
            //UploadFiles("备份数据库!");
        }
    }
}
