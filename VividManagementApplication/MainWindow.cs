﻿using System;
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

        public static bool IS_LOGED_IN = false;
        public static string ID = "";
        public static string USER_ID = "";
        public static string PASSWORD_HASH = "";
        public static string REAL_NAME = "";
        public static string WORKLOADS = "";
        public static string COMPANY_NAME = "";
        public static string COMPANY_OWNER = "";
        public static string ADDRESS = "";
        public static string BANK_NAME = "";
        public static string BANK_CARD = "";
        public static string PHONE = "";
        public static string FAX = "";
        public static string QQ = "";
        public static string EMAIL = "";
        public static string ADDTIME = "";
        public static string NOTIFICATION = "";
        public static int COMPANY_BALANCE = 0; // 公司结余暂存

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
            lbUserName.Text = USER_ID;
            dataBaseFilePrefix = USER_ID + "_";

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

        // 测试
        private void qqButton1_Click(object sender, EventArgs e)
        {
            //DatabaseConnections.GetInstence().UserLogin("00000","00000");
            //CreateMainListView();

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
                //FormBasicFeatrues.GetInstence().UpLoadFile(fileName, "", "ftp://qyw28051:cyy2014@qyw28051.my3w.com/products/caiYY/backup/" + dataBaseFilePrefix + "data.txt");
                FormBasicFeatrues.GetInstence().UpLoadFile(fileName, "", "ftp://vividappftp:vividappftp@www.vividapp.net/Project/VMA/Users/" + USER_ID + "/" + dataBaseFilePrefix + "data.txt");
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
            string urlName = "http://www.vividapp.net/Project/VMA/Users/" + USER_ID + "/" + dataBaseFilePrefix + "data.txt";
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

        private void CreateMainDataGridView(DataGridViewColumn[] dgvcArray, string table, int discardFlagIndex, string[] queryArray)
        {
            string order = " ORDER BY id ASC ";
            this.MainDataGridView.Columns.Clear();
            this.MainDataGridView.Rows.Clear();

            this.MainDataGridView.Columns.AddRange(dgvcArray);
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
            this.MainDataGridView.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
        }

        // 收支汇总表
        private void CreateMainDataGridViewPZ(DataGridViewColumn[] dgvcArray, string table, string[] queryArray)
        {
            string order = " ORDER BY id ASC "; // 顺序
            this.MainDataGridView.Columns.Clear();
            this.MainDataGridView.Rows.Clear();
            int tempBalance = COMPANY_BALANCE;

            this.MainDataGridView.Columns.AddRange(dgvcArray);
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

        // 客户管理
        private void listCxButton_Click(object sender, EventArgs e)
        {
            ViewButton.Enabled = true;
            CURRENT_LIST_BUTTON = listCxButton;
            CURRENT_TAB = 1;
            mainDGVTitle.Text = listCxButton.Text;
            Column1.HeaderText = "客户编号";
            Column2.HeaderText = "客户名称(公司名称)";
            Column3.HeaderText = "联系地址";
            Column4.HeaderText = "联系人";
            Column5.HeaderText = "联系电话";

            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 }, "clients", -1,
                new string[] { "clientID", "company", "address", "contact", "phone" });
        }

        // 商品管理
        private void listSpButton_Click(object sender, EventArgs e)
        {
            ViewButton.Enabled = true;
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
            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7 }, "goods", -1,
                new string[] { "goodID", "name", "guige", "dengji", "unit", "currntsalesPrice", "beizhu" });
        }

        #region 仓储管理
        // 库存
        private void listKcButton_Click(object sender, EventArgs e)
        {
            ViewButton.Enabled = false;
            CURRENT_LIST_BUTTON = listKcButton;
            CURRENT_TAB = 3;
            mainDGVTitle.Text = listKcButton.Text;
            Column1.HeaderText = "商品编号";
            Column2.HeaderText = "商品名称";
            Column3.HeaderText = "规格";
            Column4.HeaderText = "等级";
            Column5.HeaderText = "单位";
            Column6.HeaderText = "库存数量";
            Column7.HeaderText = "进货单价";
            Column8.HeaderText = "备注";

            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8 }, "goods", -1,
                new string[] { "goodID", "name", "guige", "dengji", "unit", "currentCount", "purchasePrice", "beizhu" });
        }

        private void listJcdButton_Click(object sender, EventArgs e)
        {
            ViewButton.Enabled = true;
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
            ViewButton.Enabled = true;
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
            ViewButton.Enabled = true;
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
            ViewButton.Enabled = true;
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
            ViewButton.Enabled = false;
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

        // 客户对账单
        private void listKhdzButton_Click(object sender, EventArgs e)
        {
            ViewButton.Enabled = false;
            this.MainDataGridView.Rows.Clear();
            this.MainDataGridView.Columns.Clear();

            Filter flt = new Filter();
            flt.ShowDialog();

            if (flt.FilterOKClickedIndex == 1) // OK键
            {
                CURRENT_LIST_BUTTON = listKhdzButton;
                CURRENT_TAB = 4;
                mainDGVTitle.Text = listKhdzButton.Text;
                Column1.HeaderText = "凭证号码";
                Column2.HeaderText = "日期";
                Column3.HeaderText = "凭证类型";
                Column4.HeaderText = "交易金额";
                Column5.HeaderText = "结余金额";

                Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                int tempBalance = COMPANY_BALANCE;

                this.MainDataGridView.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 });
                List<string[]> resultsList = DatabaseConnections.GetInstence().LocalGetDataFromOriginalSQL(
                    "SELECT pzID, cast (modifyTime as VARCHAR) as modifyTime"
                    + ",case when leixing = '0' then '收款凭证' when leixing = '1' then '付款凭证' when leixing = '2' then '领款凭证' when leixing = '3' then '还款凭证' else '报销凭证' end as 'leixing'"
                    + ",operateMoney,remaintingMoney FROM pzList WHERE clientID = '" + flt.clientID.Text + "' AND discardFlag = 0"
                    + " AND (modifyTime BETWEEN '" + flt.fromDate.Value.ToShortDateString() + "' AND '" + flt.toDate.Value.ToShortDateString() + "')"
                    + " ORDER BY modifyTime ASC",
                    new String[] { "pzID", "modifyTime", "leixing", "operateMoney", "remaintingMoney" });

                for (int i = 0; i < resultsList.Count; i++)
                {
                    if ((resultsList[i][2].Equals("收款凭证")) || resultsList[i][2].Equals("还款凭证"))
                    {
                        tempBalance += int.Parse(resultsList[i][3]);
                    }
                    else
                    {
                        tempBalance -= int.Parse(resultsList[i][3]);
                    }
                    resultsList[i][4] = tempBalance.ToString();
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
            ViewButton.Enabled = true;
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
            //UploadFiles("备份数据库!");
        }

        private void settingQQButton_Click(object sender, EventArgs e)
        {
            Setting st = new Setting();
            st.ShowDialog();
        }

        private void qqButton1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(FormBasicFeatrues.GetInstence().addCharIntoString("   ", "购买合同") + "haha");
        }

    }
}
