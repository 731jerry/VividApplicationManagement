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

namespace VividManagementApplication
{
    public partial class DetailedInfo : Form
    {
        public string ItemId = "-1";
        public string table = "";
        public string[] queryArray = new string[] { };
        public string controlsPreName = "";
        public Panel detailedPanel = new Panel();
        public int indexCount = 0;
        public string mainID = "";
        public bool canPrint = false;

        // 打印标识符
        public int printFlag = 0;
        public int pageWidth;
        public int pageHeight;

        public DetailedInfo()
        {
            InitializeComponent();
        }

        private void DetailedInfo_Load(object sender, EventArgs e)
        {
            InitDetailedInfoWindow();
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;
            //this.StartPosition = FormStartPosition.CenterScreen; 
        }

        private void InitDetailedInfoWindow()
        {
            int detailedLocationY = 60;
            int detailedHeightDis = 100;

            string[] resultStringArray = new string[] { };

            switch (MainWindow.CURRENT_TAB)
            {
                case 1:
                    detailedHeightDis = 200;
                    table = "clients";
                    detailedPanel = DetailedClientPanel;
                    queryArray = new string[] { "id", "sex", "type", "name", "contact", "address", "phone", "taxNumber", "email", "bankInfo", "otherContacts", "PrimaryAccount", "beizhu" };
                    controlsPreName = "tbClient";
                    indexCount = 13;
                    mainID = tbClient1.Text;
                    canPrint = false;
                    break;
                case 2:
                    detailedHeightDis = 200;
                    table = "goods";
                    detailedPanel = DetailedGoodsPanel;
                    queryArray = new string[] { "id", "dengji", "name", "guige", "unit", "storageName", "storageManager", "storageManagerPhone", "storageLocation", "storageAddress", "initalCount", "purchasePrice", "purchaseTotal", "currentCount", "currntsalesPrice", "currentTotal", "beizhu" };
                    controlsPreName = "tbGoods";
                    indexCount = 17;
                    mainID = tbGoods1.Text;
                    canPrint = false;
                    break;
                case 3:
                    detailedHeightDis = 60;
                    detailedPanel = DetailedDanziPanel;
                    canPrint = true;
                    danziComboBox.SelectedIndex = 0;

                    break;
                default:
                    detailedHeightDis = 60;
                    detailedPanel = DetailedDanziPanel;
                    canPrint = true;
                    break;
                /*
            case 4:
                detailedPanel = DetailedDanziPanel;
                detailedHeightDis = 60;

                break;
            case 5:
                detailedPanel = DetailedDanziPanel;
                detailedHeightDis = 60;

                break;
                 */
                case 6:
                    detailedPanel = DetailedHTPanel;
                    detailedLocationY = 80;
                    detailedHeightDis = 60;
                    canPrint = true;
                    break;
            }

            if (!ItemId.Equals("-1"))
            {
                try
                {
                    FormBasicFeatrues.GetInstence().SetControlsVaule(controlsPreName, detailedPanel, DatabaseConnections.GetInstence().LocalGetOneRowDataById(table, queryArray, ItemId));
                }
                catch (Exception ex)
                {
                    FormBasicFeatrues.GetInstence().RecordLog(ex, "无法查看详细");
                    MessageBox.Show("无法查看详细" + ex.Message, "错误");
                    this.Close();
                }
            }

            this.Size = new Size(this.Size.Width, this.Size.Height - detailedHeightDis);
            PreviewPrintButton.Location = new Point(PreviewPrintButton.Location.X, PreviewPrintButton.Location.Y - detailedHeightDis);
            SaveButton.Location = new Point(SaveButton.Location.X, SaveButton.Location.Y - detailedHeightDis);
            detailedPanel.Parent = this;
            detailedPanel.Location = new Point(15, 5);
            DetailedTabView.Visible = false;

            if (!canPrint)
            {
                PreviewPrintButton.Visible = false;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseConnections.GetInstence().LocalReplaceIntoData(table, queryArray, FormBasicFeatrues.GetInstence().GetControlsVaule(controlsPreName, detailedPanel, indexCount), mainID);
                if (ItemId.Equals("-1"))
                {
                    MessageBox.Show("新建成功!", "恭喜");
                }
                else
                {
                    MessageBox.Show("保存成功!", "恭喜");
                }
            }
            catch (Exception ex)
            {
                FormBasicFeatrues.GetInstence().RecordLog(ex, "");
                MessageBox.Show(ex.Message, "错误");
            }
            this.Close();
        }

        private void PreviewPrintButton_Click(object sender, EventArgs e)
        {
            SetPrintPreview(1);
        }

        #region 打印
        // 打印
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", 850, 1100);
            //FormBasicFeatrues.GetInstence().PrintPageSet(panel6, 15, 0, null, e);
            switch (printFlag)
            {
                default:
                case 1: // 进出仓单
                    //PrintGeneral(0, 0, panelJCD, e);//panelJCD
                    PrintDZ(0, 30, 0, e);
                    break;
                case 2: // 采购销售单
                    //PrintCGXSD(0, 0, yewuPanel, e);
                    break;
                case 3: // 采购销售列表
                    //PrintWithDGV(0, 0, panel15, dgvCGXS, 30, e);
                    break;
                case 4: // 对账单
                    //PrintWithDGV(0, 0, panel9, dgvYWDZ, 30, e);
                    break;
                case 5: // 凭证
                    //PrintGeneral(0, 0, panelPZ, e);
                    break;
                case 6: // 收付汇总表
                    //PrintWithDGV(0, 0, panelHZB, dgvSFHZB, 30, e);
                    break;
                case 7: // 合同
                    printHetong(20, 50, e);
                    break;
            }
        }
        // 通用preview
        private void SetPrintPreview(int flag)
        {
            printFlag = flag;
           
            //this.printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom",this.printDocument1.DefaultPageSettings.PaperSize.Width, 600);

            pageWidth = this.printDocument1.DefaultPageSettings.PaperSize.Width;
            pageHeight = this.printDocument1.DefaultPageSettings.PaperSize.Height;

            //注意指定其Document(获取或设置要预览的文档)属性
            this.printPreviewDialog1.Document = this.printDocument1;
            //ShowDialog方法：将窗体显示为模式对话框，并将当前活动窗口设置为它的所有者
            this.printPreviewDialog1.WindowState = FormWindowState.Maximized;
            this.printPreviewDialog1.ShowDialog();
        }

        private void PrintGeneral(int x, int y, Panel innerPanel, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;   //先建立画布 
            //g.DrawImage(this.BackgroundImage, 50, 50);
            foreach (Control item in innerPanel.Controls)
            {
                if (item is Label)
                {
                    Control tx = (item as Control);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x, tx.Top + y);
                }
                if (item is TextBox)
                {
                    TextBox tx = (item as TextBox);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Location.X + x, tx.Location.Y + y + 5);
                    if (tx.BorderStyle == BorderStyle.FixedSingle)
                    {
                        g.DrawRectangle(new Pen(Color.Black), tx.Left + x, tx.Top + y, tx.Width - 1, tx.Height - 1);
                    }
                }
                if (item is ComboBox)
                {
                    ComboBox tx = (item as ComboBox);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x, tx.Top + y + 5);
                    g.DrawRectangle(new Pen(Color.Black), tx.Left + x, tx.Top + y, tx.Width - 1, tx.Height - 1);
                }
                if (item is DateTimePicker)
                {
                    DateTimePicker tx = (item as DateTimePicker);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x, tx.Top + y + 5);
                    g.DrawRectangle(new Pen(Color.Black), tx.Left + x, tx.Top + y, tx.Width - 1, tx.Height - 1);
                }
                /*
                if (item is DataGridView)
                {
                    DataGridView tx = (item as DataGridView);
                    for (int i = 0; i < tx.ColumnCount; i++)
                    {
                        for (int j = 0; j < tx.RowCount; j++)
                        {
                            g.DrawString(tx.Rows[j].Cells[i].Value.ToString(), tx.Font, new SolidBrush(item.ForeColor), tx.Left + x, tx.Top + y);
                        }
                    }
                }
                */
                /*
                if (dgv != null)
                {
                    int iX = 35;
                    int iY = 140;
                    PrintDataGridView.Print(dgv, true, e, ref iX, ref iY);
                }
                 */
            }
        }

        private void PrintDZ(int x, int y, int option, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;   //先建立画布
            SizeF fontSize;

            Font f1 = new Font("黑体", 20, FontStyle.Bold);
            Font f2 = new Font("微软雅黑", 9);
            Font f3 = new Font("微软雅黑", 11);

            string title = lbDzTitle.Text;
            //测试
            abDz1.Text = "hahahh ";
            abDz2.Text = "hahahh ";
            abDz3.Text = "hahahh ";
            tbDz3.Text = "大法师打发";
            tbDz1.Text = "dffdf11212";
            tbDz2.Text = "dffdf11212";

            int fontDisX = 3;
            int fontDisY = 3;
            int tableX = 50;
            int tableY = 185;

            fontSize = g.MeasureString(title, f1);
            g.DrawString(title, f1, new SolidBrush(Color.Black), pageWidth / 2 - fontSize.Width / 2 + x, y);

            fontSize = g.MeasureString("TAL：", f2);
            g.DrawString("TAL：", tbDz2.Font, new SolidBrush(abDz1.ForeColor), tableX + x, 50 + y + fontDisY);
            //g.DrawRectangle(new Pen(Color.Black), tableX + x + 733 - tbDz2.Size.Width, 80 + y, tbDz2.Size.Width, tbDz2.Height);
            g.DrawString("", tbDz1.Font, new SolidBrush(abDz1.ForeColor), tableX + x, 50 + y + fontDisY);

            fontSize = g.MeasureString("FAX：", f2);
            g.DrawString("FAX：", tbDz2.Font, new SolidBrush(abDz1.ForeColor), tableX + x, 80 + y + fontDisY);
            //g.DrawRectangle(new Pen(Color.Black), tableX + x + 733 - tbDz2.Size.Width, 80 + y, tbDz2.Size.Width, tbDz2.Height);
            g.DrawString("", tbDz2.Font, new SolidBrush(abDz1.ForeColor), tableX + x, 80 + y + fontDisY);

            //   以质为根   以诚为本
            fontSize = g.MeasureString("   以质为根   以诚为本", f2);
            g.DrawString("   以质为根   以诚为本", tbDz2.Font, new SolidBrush(abDz1.ForeColor), tableX + x, tableY - 26 * 3 + y + fontDisY);
            //g.DrawRectangle(new Pen(Color.Black), tableX + x + 733 - tbDz2.Size.Width, 80 + y, tbDz2.Size.Width, tbDz2.Height);
            g.DrawString("", tbDz2.Font, new SolidBrush(abDz1.ForeColor), tableX + x, tableY - 26 * 3 + y + fontDisY);

            fontSize = g.MeasureString("*客户编号：", f2);
            g.DrawString("*客户编号：", tbDz2.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 733 - fontSize.Width - tbDz1.Size.Width - 5, 50 + y + fontDisY);
            //g.DrawRectangle(new Pen(Color.Black), tableX + x + 733 - tbDz2.Size.Width, 80 + y, tbDz2.Size.Width, tbDz2.Height);
            g.DrawString(tbDz1.Text, tbDz1.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 733 - tbDz2.Size.Width + fontDisX, 50 + y + fontDisY);

            fontSize = g.MeasureString("凭证号码：", f2);
            g.DrawString("凭证号码：", tbDz2.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 733 - fontSize.Width - tbDz2.Size.Width - 5, 80 + y + fontDisY);
            //g.DrawRectangle(new Pen(Color.Black), tableX + x + 733 - tbDz2.Size.Width, 80 + y, tbDz2.Size.Width, tbDz2.Height);
            g.DrawString(tbDz2.Text, tbDz2.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 733 - tbDz2.Size.Width + fontDisX, 80 + y + fontDisY);


            fontSize = g.MeasureString("业务联/白色； 财务联/黄色； 仓库联/蓝色； 客户联/红色", f2);
            g.DrawString("业务联/白色； 财务联/黄色； 仓库联/蓝色； 客户联/红色", f2, new SolidBrush(abDz1.ForeColor), tableX + x + fontDisX + 733 - fontSize.Width, tableY - 26 * 3 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x, tableY - 26 * 2 + y, 116, abDz1.Height);
            g.DrawString("对方单位：", abDz1.Font, new SolidBrush(abDz1.ForeColor), tableX + x + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, tableY - 26 * 2 + y, 268, abDz1.Height);
            g.DrawString(abDz1.Text, abDz1.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 116 + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 384, tableY - 26 * 2 + y, 111, abDz1.Height);
            g.DrawString("日期：", abDz1.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 384 + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 111 + 384, tableY - 26 * 2 + y, 238, abDz1.Height);
            g.DrawString(DateTime.Now.ToLongDateString(), abDz1.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 111 + 384 + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x, tableY - 26 + y, 116, abDz1.Height);
            g.DrawString("联系地址：", abDz1.Font, new SolidBrush(abDz1.ForeColor), tableX + x + fontDisX, tableY - 26 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, tableY - 26 + y, 268, abDz2.Height);
            g.DrawString(abDz2.Text, abDz1.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 116 + fontDisX, tableY - 26 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 384, tableY - 26 + y, 111, abDz1.Height);
            g.DrawString("联系电话：", abDz1.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 384 + fontDisX, tableY - 26 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 111 + 384, tableY - 26 + y, 238, abDz3.Height);
            g.DrawString(abDz3.Text, abDz1.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 111 + 384 + fontDisX, tableY - 26 + y + fontDisY);

            // 表格之后
            g.DrawRectangle(new Pen(Color.Black), tableX + x, 156 + y + tableY, 116, tbDz3.Height);
            g.DrawString("总金额：", tbDz3.Font, new SolidBrush(abDz1.ForeColor), tableX + x + fontDisX, 156 + y + fontDisY + tableY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, 156 + y + tableY, 617, tbDz3.Height);
            g.DrawString(tbDz3.Text, tbDz3.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 116 + fontDisX, 156 + y + fontDisY + tableY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x, 182 + y + tableY, 452, 104);
            g.DrawString("备注：", tbDz3.Font, new SolidBrush(abDz1.ForeColor), tableX + x + fontDisX, 182 + y + fontDisY + tableY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 452, 182 + y + tableY, 281, 78);
            g.DrawString("发票号码：", tbDz3.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 452 + fontDisX, 182 + y + fontDisY + tableY);
            g.DrawString("增：", tbDz3.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 452 + fontDisX, 208 + y + fontDisY + tableY);
            g.DrawString("普：", tbDz3.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 452 + fontDisX, 234 + y + fontDisY + tableY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 452, 260 + y + tableY, 281, 26);
            g.DrawString("附件凭证 " + tbDz8.Text + " 张", tbDz3.Font, new SolidBrush(abDz1.ForeColor), tableX + x + 452 + fontDisX, 260 + y + fontDisY + tableY);

            g.DrawString("对方送货人\n（签 字）：", f3, new SolidBrush(Color.Black), tableX + x, 300 + y + fontDisY + tableY);
            g.DrawString("业务经办人\n（签 字）：", f3, new SolidBrush(Color.Black), tableX + x + 733 / 2 - 90, 300 + y + fontDisY + tableY);
            g.DrawString("仓库验收人\n（签 字）：", f3, new SolidBrush(Color.Black), tableX + x + 733 - 190, 300 + y + fontDisY + tableY);

            foreach (Control item in PanelDZ.Controls)
            {
                if (item is Label)
                {
                    Control tx = (item as Control);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x + tableX, tx.Top + y + tableY);
                }
                if (item is TextBox)
                {
                    TextBox tx = (item as TextBox);
                    //g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Location.X + x + tableX, tx.Location.Y + y + 3 + tableY);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Location.X + x + tableX + fontDisX, tx.Location.Y + y + tableY + fontDisY);
                    if (tx.BorderStyle == BorderStyle.FixedSingle)
                    {
                        //g.DrawRectangle(new Pen(Color.Black), tx.Left + x + tableX, tx.Top + y + tableY, tx.Width, tx.Height - 4);
                        g.DrawRectangle(new Pen(Color.Black), tx.Left + x + tableX, tx.Top + y + tableY, tx.Width, tx.Height);
                    }
                }
                if (item is ComboBox)
                {
                    ComboBox tx = (item as ComboBox);
                    //g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x + tableX, tx.Top + y + 3 + tableY);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x + tableX + fontDisX, tx.Top + y + tableY + fontDisY);
                    //g.DrawRectangle(new Pen(Color.Black), tx.Left + x + tableX, tx.Top + y + tableY, tx.Width, tx.Height - 6);
                    g.DrawRectangle(new Pen(Color.Black), tx.Left + x + tableX, tx.Top + y + tableY, tx.Width, 26);
                }
            }
        }
        // 打印合同
        private void printHetong(int x, int y, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;   //先建立画布 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //先打印表头内容
            Font f1 = new Font("宋体", 18, FontStyle.Bold);
            Font f2 = new Font("黑体", 14, FontStyle.Bold);
            Font f3 = new Font("宋体", 12);
            Font f4 = new Font("华文行楷", 9);
            Font f45 = new Font("华文行楷", 9, FontStyle.Bold);
            Font f5 = new Font("华文行楷", 9, FontStyle.Underline);
            Font f6 = new Font("楷体", 9);
            Font f7 = new Font("华文行楷", 10);

            SizeF fontSize;

            //
            fontSize = g.MeasureString("桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司", f3);
            g.DrawString("桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司", f3, new SolidBrush(Color.Blue), pageWidth / 2 - fontSize.Width / 2 + x, y);

            //
            fontSize = g.MeasureString("购 销 合 同", f1);
            g.DrawString("购 销 合 同", f1, new SolidBrush(Color.Red), pageWidth / 2 - fontSize.Width / 2 + x, 30 + y);

            //
            g.DrawString("购货方：", f4, new SolidBrush(Color.Black), 40 + x, 80 + y);
            fontSize = g.MeasureString("购货方：", f4);
            g.DrawString("财盈盈软件公司", f5, new SolidBrush(Color.Black), 40 + fontSize.Width + x, 80 + y);

            g.DrawString("合同编号：", f4, new SolidBrush(Color.Black), 150 + pageWidth / 2 + x, 80 + y);
            fontSize = g.MeasureString("合同编号：", f4);
            g.DrawString(HTtbID.Text, f5, new SolidBrush(Color.Red), 150 + pageWidth / 2 + fontSize.Width + x, 80 + y);

            //
            g.DrawString("签约日期：", f4, new SolidBrush(Color.Black), 150 + pageWidth / 2 + x, 100 + y);
            fontSize = g.MeasureString("签约日期：", f4);
            g.DrawString(HTtbDate.Text, f5, new SolidBrush(Color.Black), 150 + pageWidth / 2 + fontSize.Width + x, 100 + y);

            // 
            g.DrawString("销货方：", f4, new SolidBrush(Color.Black), 40 + x, 120 + y);
            fontSize = g.MeasureString("销货方：", f4);
            g.DrawString("桐乡市瑞递曼尔工贸有限公司", f5, new SolidBrush(Color.Black), 40 + fontSize.Width + x, 120 + y);

            g.DrawString("签约地点：", f4, new SolidBrush(Color.Black), 150 + pageWidth / 2 + x, 120 + y);
            fontSize = g.MeasureString("签约地点：", f4);
            g.DrawString(HTtbLocation.Text, f5, new SolidBrush(Color.Black), 150 + pageWidth / 2 + fontSize.Width + x, 120 + y);

            // 
            g.DrawString("根据中华人民共和国经济合同法和有关政策规定，经购销双方协商后，一致同意签订本合同，合同内容如下：", f4, new SolidBrush(Color.Black), 40 + x, 160 + y);

            // 
            g.DrawString("1、交易内容（品名、规格、数量、单位、单价、金额、交（提）货日期）：", f4, new SolidBrush(Color.Black), 40 + x, 200 + y);

            /*
            g.DrawRectangle(new Pen(Color.Black), 40 + x, 220 + y, 40, 30);
            g.DrawString("货号", f4, new SolidBrush(Color.Black), 40 + x, 225 + y);

            g.DrawRectangle(new Pen(Color.Black), 40 + x, 250 + y, 40, 30);
            g.DrawRectangle(new Pen(Color.Black), 40 + x, 280 + y, 40, 30);
            g.DrawRectangle(new Pen(Color.Black), 40 + x, 310 + y, 40, 30);
            g.DrawRectangle(new Pen(Color.Black), 40 + x, 340 + y, 40, 30);
            g.DrawRectangle(new Pen(Color.Black), 40 + x, 370 + y, 40, 30);
            g.DrawRectangle(new Pen(Color.Black), 40 + x, 400 + y, 40, 30);
             */

            int fontDisX = 3;
            int fontDisY = 3;
            foreach (Control item in PanelHT.Controls)
            {
                if (item is Label)
                {
                    Control tx = (item as Control);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + 20 + x, tx.Top + 240 + y);
                }
                if (item is TextBox)
                {
                    TextBox tx = (item as TextBox);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Location.X + 20 + x + fontDisX, tx.Location.Y + 240 + y + +fontDisY);
                    if (tx.BorderStyle == BorderStyle.FixedSingle)
                    {
                        g.DrawRectangle(new Pen(Color.Black), tx.Left + 20 + x, tx.Top + 240 + y, tx.Width, tx.Height);
                    }
                }
                if (item is ComboBox)
                {
                    ComboBox tx = (item as ComboBox);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + 20 + x + fontDisX, tx.Top + 240 + y + fontDisY);
                    g.DrawRectangle(new Pen(Color.Black), tx.Left + 20 + x, tx.Top + 240 + y, tx.Width, 25);
                }
            }

            // 
            g.DrawString("2、质量标准及验收方法：", f4, new SolidBrush(Color.Black), 40 + x, 465 + y);
            fontSize = g.MeasureString("2、质量标准及验收方法：", f4);
            g.DrawString(HTcbChoose2.Text, f4, new SolidBrush(Color.Black), 40 + fontSize.Width + x, 465 + y);

            // 
            g.DrawString("3、包装方式及要求：", f4, new SolidBrush(Color.Black), 40 + x, 485 + y);
            fontSize = g.MeasureString("3、包装方式及要求：", f4);
            g.DrawString(HTcbChoose3.Text, f4, new SolidBrush(Color.Black), 40 + fontSize.Width + x, 485 + y);

            // 
            g.DrawString("4、运输方式及费用承担：", f4, new SolidBrush(Color.Black), 40 + x, 505 + y);
            fontSize = g.MeasureString("4、运输方式及费用承担：", f4);
            g.DrawString(HTcbChoose4.Text, f4, new SolidBrush(Color.Black), 40 + fontSize.Width + x, 505 + y);

            // 
            g.DrawString("5、交（提）货地点及验收方法：", f4, new SolidBrush(Color.Black), 40 + x, 525 + y);
            fontSize = g.MeasureString("5、交（提）货地点及验收方法：", f4);
            g.DrawString(HTcbChoose5.Text, f4, new SolidBrush(Color.Black), 40 + fontSize.Width + x, 525 + y);

            // 
            g.DrawString("6、验收异议期限及方式：", f4, new SolidBrush(Color.Black), 40 + x, 545 + y);
            fontSize = g.MeasureString("6、验收异议期限及方式：", f4);
            g.DrawString(HTcbChoose6.Text, f4, new SolidBrush(Color.Black), 40 + fontSize.Width + x, 545 + y);

            // *** 39
            //g.DrawString("7、货款结算方式及期限：", f4, new SolidBrush(Color.Black), 40 + x, 580 + y);
            //fontSize = g.MeasureString("7、货款结算方式及期限：", f4);
            string tempString = ("7、货款结算方式及期限：" + HTcbChoose7.Text);
            g.DrawString((tempString.Length > 51) ? tempString.Insert(51, "\n") : tempString, f4, new SolidBrush(Color.Black), 40 + x, 565 + y);
            Console.WriteLine("####" + HTcbChoose7.Text.Length);
            // 
            g.DrawString("8、违约责任：如本合同执行过程中发生纠纷，双方应本着友好合作的态度进行协商解决，遇协商不成时，遵照国家\n相关法律法规及法定程序，任何一方均有权提请销货方所在地的人民法院对有争议的事项依法做出裁决。", f4, new SolidBrush(Color.Black), 40 + x, 600 + y);

            // 
            g.DrawString("9、其它条款：(1)本合同未尽事宜皆按中华人民共和国各项法律之规定处理。（2）本合同如有附件，既与正文具有\n同等效力。（3）本合同一经生效，以前有关本合同（本批贸易）的函电、文件与本合同具有抵触的内容均为无效。\n（4）本合同一式二份（双方各执一份），经双方代理人签字（单位须加盖公章或合同章）后生效。双方必须全面履\n行本合同，任何一方不得擅自变更或解除。", f4, new SolidBrush(Color.Black), 40 + x, 635 + y);

            // 框
            int recWidth = (pageWidth - 60 * 2) / 2;
            int recHeight = 330;
            g.DrawRectangle(new Pen(Color.Black), 40 + x, 700 + y, recWidth, recHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + recWidth + x, 700 + y, recWidth, recHeight);

            // 框内
            fontSize = g.MeasureString("销  货  方", f45);
            g.DrawString("销  货  方", f45, new SolidBrush(Color.Black), 40 + x + recWidth / 2 - fontSize.Width / 2, 710 + y);
            g.DrawString("单位名称：（章）", f4, new SolidBrush(Color.Black), 40 + x + 10, 740 + y);
            g.DrawString("法人代表：", f4, new SolidBrush(Color.Black), 40 + x + 10, 770 + y);
            g.DrawString("地    址：", f4, new SolidBrush(Color.Black), 40 + x + 10, 800 + y);
            g.DrawString("电    话：", f4, new SolidBrush(Color.Black), 40 + x + 10, 850 + y);
            g.DrawString("传    真：", f4, new SolidBrush(Color.Black), 40 + x + 10, 880 + y);
            g.DrawString("代 理 人：\n（签 字）", f4, new SolidBrush(Color.Black), 40 + x + 10, 910 + y);
            g.DrawString("开户银行：", f4, new SolidBrush(Color.Black), 40 + x + 10, 960 + y);
            g.DrawString("帐    号：", f4, new SolidBrush(Color.Black), 40 + x + 10, 990 + y);

            fontSize = g.MeasureString("购  货  方", f45);
            g.DrawString("购  货  方", f45, new SolidBrush(Color.Black), 40 + x + recWidth / 2 - fontSize.Width / 2 + recWidth, 710 + y);
            g.DrawString("单位名称：（章）", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, 740 + y);
            g.DrawString("法人代表：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, 770 + y);
            g.DrawString("地    址：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, 800 + y);
            g.DrawString("电    话：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, 850 + y);
            g.DrawString("传    真：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, 880 + y);
            g.DrawString("代 理 人：\n（签 字）", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, 910 + y);
            g.DrawString("开户银行：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, 960 + y);
            g.DrawString("帐    号：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, 990 + y);

            // 水印
            //g.DrawString("合同版本由唯达软件提供提    ", f6, new SolidBrush(Color.Red), 40, 35);
            //g.DrawString("共1页 一式两份", f6, new SolidBrush(Color.Red), pageWidth / 2 - 50, pageHeight - 35);
            //fontSize = g.MeasureString("专业软件定制 888888888", f6);
            //g.DrawString("专业软件定制 888888888", f6, new SolidBrush(Color.Red), pageWidth - fontSize.Width - 40 + x, 35);

            g.DrawString("合同版本由唯达软件系统提供   软件定制电话: 0573-8888 8888", f6, new SolidBrush(Color.Red), 40 + x, pageHeight - 85);
            //g.DrawString("共1页 一式两份", f6, new SolidBrush(Color.Red), pageWidth / 2 - 50, pageHeight - 35);
            //fontSize = g.MeasureString("专业软件定制 888888888", f6);
            //g.DrawString("专业软件定制 888888888", f6, new SolidBrush(Color.Red), pageWidth - fontSize.Width - 40 + x, pageHeight - 50);
        }
        #endregion

        //为生成新行添加值
        private void DetailedDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[0].Value = "hahah";
        }

        // 进仓单 出仓单 采购单 销售单
        private void danziComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
             switch (danziComboBox.SelectedIndex)
             {
                case 0://进仓单
                    lbDzTitle.Text = "商品（货物）进仓单";
                    //DetailedDanziPanel.Height -= 130;
                    //this.Height -= 170;
                    //PreviewPrintButton.Location = new Point(PreviewPrintButton.Location.X, PreviewPrintButton.Location.Y - 170);
                    //SaveButton.Location = new Point(SaveButton.Location.X, SaveButton.Location.Y - 170);
                    moreDetaildpPanel.Visible = false;
                    break;
                case 1://出仓单
                      lbDzTitle.Text = "商品（货物）出仓单";
                    //DetailedDanziPanel.Height -= 130;
                    //this.Height -= 170;
                    //PreviewPrintButton.Location = new Point(PreviewPrintButton.Location.X, PreviewPrintButton.Location.Y - 170);
                    //SaveButton.Location = new Point(SaveButton.Location.X, SaveButton.Location.Y - 170);
                      moreDetaildpPanel.Visible = false;
                    break;
                case 2://采购单
                    lbDzTitle.Text = "商品（货物）采购单";
                    moreDetaildpPanel.Visible = true;
                    break;
                case 3://销售单
                    moreDetaildpPanel.Visible = true;
                    lbDzTitle.Text = "商品（货物）销售单";
                    break;
                default:
                    break;
            }
        }

    }
}
