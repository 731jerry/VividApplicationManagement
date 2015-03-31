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
        public string baseName = "id";
        public string[] queryArray = new string[] { };
        public string controlsPreName = "";
        public Panel detailedPanel = new Panel();
        public int indexCount = 0;
        public string mainID = "";
        public bool canPrint = false;
        public List<Control> checkValidateControls;

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
            checkValidateControls = new List<Control>();

            switch (MainWindow.CURRENT_TAB)
            {
                case 1:
                    checkValidateControls = new List<Control>() { tbClient1, tbClient2, tbClient3, tbClient4, tbClient5 };
                    detailedHeightDis = 250;
                    detailedPanel = DetailedClientPanel;

                    table = "clients";
                    baseName = "clientID";
                    queryArray = new string[] { baseName, "sex", "type", "company", "contact", "address", "phone", "taxNumber", "email", "bankInfo", "otherContacts", "PrimaryAccount", "beizhu" };
                    controlsPreName = "tbClient";
                    indexCount = 13;
                    mainID = tbClient1.Text;

                    canPrint = false;

                    if (!ItemId.Equals("-1"))
                    {
                        try
                        {
                            FormBasicFeatrues.GetInstence().SetControlsVaule(controlsPreName, detailedPanel, DatabaseConnections.GetInstence().LocalGetOneRowDataById(table, queryArray, baseName, ItemId));
                        }
                        catch (Exception ex)
                        {
                            FormBasicFeatrues.GetInstence().RecordLog(ex, "无法查看详细");
                            MessageBox.Show("无法查看详细 - " + ex.Message, "错误");
                            this.Close();
                        }
                    }

                    break;
                case 2:
                    checkValidateControls = new List<Control>() { tbGoods1, tbGoods2, tbGoods3, tbGoods4, tbGoods4 };
                    detailedHeightDis = 250;
                    detailedPanel = DetailedGoodsPanel;

                    table = "goods";
                    baseName = "goodID";
                    queryArray = new string[] { baseName, "dengji", "name", "guige", "unit", "storageName", "storageManager", "storageManagerPhone", "storageLocation", "storageAddress", "initalCount", "purchasePrice", "purchaseTotal", "currentCount", "currntsalesPrice", "currentTotal", "beizhu" };
                    controlsPreName = "tbGoods";
                    indexCount = 17;
                    mainID = tbGoods1.Text;

                    canPrint = false;

                    if (!ItemId.Equals("-1"))
                    {
                        try
                        {
                            FormBasicFeatrues.GetInstence().SetControlsVaule(controlsPreName, detailedPanel, DatabaseConnections.GetInstence().LocalGetOneRowDataById(table, queryArray, baseName, ItemId));
                        }
                        catch (Exception ex)
                        {
                            FormBasicFeatrues.GetInstence().RecordLog(ex, "无法查看详细");
                            MessageBox.Show("无法查看详细 - " + ex.Message, "错误");
                            this.Close();
                        }
                    }
                    break;
                case 3:
                case 4:
                    // 进仓单 出仓单 采购单 销售单
                    checkValidateControls = new List<Control>() { tbDz2 };
                    detailedPanel = DetailedDanziPanel;

                    controlsPreName = "tbDz";
                    detailedPanel = DetailedDanziPanel;
                    indexCount = 6;
                    danziComboBox.Visible = false;
                    DiscardCheckBox.Visible = true;

                    // 添加客户编号
                    addItemsToCombox(DatabaseConnections.GetInstence().LocalGetIdsOfTable("clients", "clientID", " ORDER BY id ASC "), tbDz1);

                    // 添加商品编号
                    JCDcbA.Items.Add("");
                    JCDcbB.Items.Add("");
                    JCDcbC.Items.Add("");
                    JCDcbD.Items.Add("");
                    JCDcbE.Items.Add("");
                    addItemsToCombox(DatabaseConnections.GetInstence().LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), JCDcbA);
                    addItemsToCombox(DatabaseConnections.GetInstence().LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), JCDcbB);
                    addItemsToCombox(DatabaseConnections.GetInstence().LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), JCDcbC);
                    addItemsToCombox(DatabaseConnections.GetInstence().LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), JCDcbD);
                    addItemsToCombox(DatabaseConnections.GetInstence().LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), JCDcbE);

                    canPrint = true;

                    if (MainWindow.CURRENT_LIST_BUTTON.Text.Equals("进仓单列表"))
                    {
                        detailedHeightDis = 200;
                        lbDzTitle.Text = "商品（货物）进仓单";
                        table = "jcdList";
                        baseName = "jcdID";
                        makeControlsInvisibleForJCCD(false);
                        // moreDetaildpPanel.Visible = false;
                    }
                    else if (MainWindow.CURRENT_LIST_BUTTON.Text.Equals("出仓单列表"))
                    {
                        detailedHeightDis = 200;
                        lbDzTitle.Text = "商品（货物）出仓单";
                        table = "ccdList";
                        baseName = "ccdID";
                        makeControlsInvisibleForJCCD(false);
                        //  moreDetaildpPanel.Visible = false;
                    }
                    else if (MainWindow.CURRENT_LIST_BUTTON.Text.Equals("采购单列表"))
                    {
                        lbDzTitle.Text = "商品（货物）采购单";
                        table = "cgdList";
                        baseName = "cgdID";
                        makeControlsInvisibleForJCCD(true);
                        // moreDetaildpPanel.Visible = true;
                        // FormBasicFeatrues.GetInstence().moveParentPanel(moreDetaildpPanel, detailedPanel);
                    }
                    else
                    {
                        lbDzTitle.Text = "商品（货物）销售单";
                        table = "xsdList";
                        baseName = "xsdID";
                        makeControlsInvisibleForJCCD(true);
                        // moreDetaildpPanel.Visible = true;
                        // FormBasicFeatrues.GetInstence().moveParentPanel(moreDetaildpPanel, detailedPanel);
                    }


                    queryArray = new string[] { "clientID", baseName, "companyName", "goodsName", "jsonData", "sum", "beizhu", "fpPu", "fpZeng", "fpCount", "discardFlag", "addtime", "modifyTime", "kxQq", "kxXq", "kxJf", "kxSq", "kxDay" };

                    if (ItemId.Equals("-1"))
                    {
                        danziComboBox.Visible = true;
                        danziComboBox.Items.Clear();
                        danziComboBox.Items.Add("采购单");
                        danziComboBox.Items.Add("销售单");
                        danziComboBox.SelectedIndex = 0;

                        DzDateTextBox.Text = DateTime.Now.ToLongDateString();
                        tbDz1.SelectedIndex = 0;
                        DiscardCheckBox.Visible = false;

                        // 自动生成ID
                        tbDz2.Text = DatabaseConnections.GetInstence().LocalAutoincreaseID(table, baseName);
                    }
                    else
                    {
                        try
                        {
                            List<String> queryList = queryArray.ToList();
                            queryList.Remove("companyName");
                            queryList.Remove("jsonData");
                            queryList.Remove("sum");
                            queryList.Remove("addtime");
                            queryList.Remove("modifyTime");
                            if ((MainWindow.CURRENT_LIST_BUTTON.Text.Equals("进仓单列表")) || (MainWindow.CURRENT_LIST_BUTTON.Text.Equals("进仓单列表")))
                            {
                                queryList.Remove("kxQq");
                                queryList.Remove("kxXq");
                                queryList.Remove("kxJf");
                                queryList.Remove("kxSq");
                                queryList.Remove("kxDay");
                            }
                            queryArray = queryList.ToArray();

                            FormBasicFeatrues.GetInstence().SetControlsVaule(controlsPreName, detailedPanel, DatabaseConnections.GetInstence().LocalGetOneRowDataById(table, queryArray, baseName, ItemId));

                            String[] data = DatabaseConnections.GetInstence().LocalGetOneRowDataById(table, new String[] { "modifyTime", "jsonData" }, baseName, ItemId);
                            DzDateTextBox.Text = Convert.ToDateTime(data[0]).ToLongDateString();
                            data[1] = data[1].Replace("\n", "");
                            data[1] = data[1].Replace(" ", "");
                            JSONObject json = JSONConvert.DeserializeObject(data[1]);//执行反序列化
                            if (json != null)
                            {
                                if ((JSONObject)json["1"] != null)
                                {
                                    JCDcbA.Text = ((JSONObject)json["1"])["goodsID"].ToString().Equals("") ? "" : ((JSONObject)json["1"])["goodsID"].ToString();
                                    AJCDtb5.Text = ((JSONObject)json["1"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["1"])["goodsAmount"].ToString();
                                }
                                if ((JSONObject)json["2"] != null)
                                {
                                    JCDcbB.Text = ((JSONObject)json["2"])["goodsID"].Equals("") ? "" : ((JSONObject)json["2"])["goodsID"].ToString();
                                    BJCDtb5.Text = ((JSONObject)json["2"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["2"])["goodsAmount"].ToString();
                                }
                                if ((JSONObject)json["3"] != null)
                                {
                                    JCDcbC.Text = ((JSONObject)json["3"])["goodsID"].Equals("") ? "" : ((JSONObject)json["3"])["goodsID"].ToString();
                                    CJCDtb5.Text = ((JSONObject)json["3"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["3"])["goodsAmount"].ToString();
                                }
                                if ((JSONObject)json["4"] != null)
                                {
                                    JCDcbD.Text = ((JSONObject)json["4"])["goodsID"].Equals("") ? "" : ((JSONObject)json["4"])["goodsID"].ToString();
                                    DJCDtb5.Text = ((JSONObject)json["4"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["4"])["goodsAmount"].ToString();
                                }
                                if ((JSONObject)json["5"] != null)
                                {
                                    JCDcbE.Text = ((JSONObject)json["5"])["goodsID"].Equals("") ? "" : ((JSONObject)json["5"])["goodsID"].ToString();
                                    EJCDtb5.Text = ((JSONObject)json["5"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["5"])["goodsAmount"].ToString();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            FormBasicFeatrues.GetInstence().RecordLog(ex, "无法查看详细");
                            MessageBox.Show("无法查看详细 - " + ex.Message, "错误");
                            this.Close();
                        }
                    }
                    break;
                case 5:
                    // 收款凭证 付款凭证 领款凭证 还款凭证 报销凭证
                    checkValidateControls = new List<Control>() { tbPz2 };
                    detailedPanel = DetailedPZPanel;
                    detailedHeightDis = 250;

                    table = "pzList";
                    baseName = "pzID";
                    queryArray = new string[] { "clientID", "pzID", "leixing", "companyName", "jsonData", "operateMoney", "remaintingMoney", "beizhu", "discardFlag", "addtime", "modifyTime" };
                    controlsPreName = "tbPz";
                    indexCount = 11;
                    mainID = tbDz2.Text;

                    // 添加客户编号
                    addItemsToCombox(DatabaseConnections.GetInstence().LocalGetIdsOfTable("clients", "clientID", " ORDER BY id ASC "), tbPz1);

                    canPrint = true;
                    if (ItemId.Equals("-1"))
                    {
                        pzComboBox.Visible = true;
                        pzComboBox.Items.Clear();
                        pzComboBox.Items.Add("收款凭证");
                        pzComboBox.Items.Add("付款凭证");
                        pzComboBox.Items.Add("领款凭证");
                        pzComboBox.Items.Add("还款凭证");
                        pzComboBox.Items.Add("报销凭证");
                        pzComboBox.SelectedIndex = 0;

                        PzDateTextBox.Text = DateTime.Now.ToLongDateString();
                        tbPz1.SelectedIndex = 0;

                        // 自动生成ID
                        tbPz2.Text = DatabaseConnections.GetInstence().LocalAutoincreaseID(table, baseName);
                    }
                    else
                    {
                        pzComboBox.Visible = false;

                        try
                        {
                            List<String> queryList = queryArray.ToList();
                            queryList.Remove("leixing");
                            queryList.Remove("companyName");
                            queryList.Remove("jsonData");
                            queryList.Remove("operateMoney");
                            queryList.Remove("remaintingMoney");
                            queryList.Remove("sum");
                            queryList.Remove("addtime");
                            queryList.Remove("modifyTime");
                            queryList.Remove("discardFlag");
                            queryArray = queryList.ToArray();

                            FormBasicFeatrues.GetInstence().SetControlsVaule(controlsPreName, detailedPanel, DatabaseConnections.GetInstence().LocalGetOneRowDataById(table, queryArray, baseName, ItemId));

                            String[] data = DatabaseConnections.GetInstence().LocalGetOneRowDataById(table, new String[] { "modifyTime", "jsonData", "discardFlag" }, baseName, ItemId);
                            DzDateTextBox.Text = Convert.ToDateTime(data[0]).ToLongDateString();
                            DiscardCheckBox.Checked = data[2].Equals("0") ? false : true;
                            data[1] = data[1].Replace("\n", "");
                            data[1] = data[1].Replace(" ", "");
                            JSONObject json = JSONConvert.DeserializeObject(data[1]);//执行反序列化 
                            // "zhaiyao", "operateMoney", "payWay", "payNumber", "payCount"
                            // PzcbA,APztb0,APztb1,APztb2,APztb3
                            if (json != null)
                            {
                                if ((JSONObject)json["1"] != null)
                                {
                                    PzcbA.Text = ((JSONObject)json["1"])["zhaiyao"].ToString().Equals("无") ? "" : ((JSONObject)json["1"])["zhaiyao"].ToString();
                                    APztb0.Text = ((JSONObject)json["1"])["operateMoney"].ToString().Equals("无") ? "" : ((JSONObject)json["1"])["operateMoney"].ToString();
                                    APztb1.Text = ((JSONObject)json["1"])["payWay"].ToString().Equals("无") ? "" : ((JSONObject)json["1"])["payWay"].ToString();
                                    APztb2.Text = ((JSONObject)json["1"])["payNumber"].ToString().Equals("无") ? "" : ((JSONObject)json["1"])["payNumber"].ToString();
                                    APztb3.Text = ((JSONObject)json["1"])["payCount"].ToString().Equals("无") ? "" : ((JSONObject)json["1"])["payCount"].ToString();
                                }
                                if ((JSONObject)json["2"] != null)
                                {
                                    PzcbB.Text = ((JSONObject)json["2"])["zhaiyao"].ToString().Equals("无") ? "" : ((JSONObject)json["2"])["zhaiyao"].ToString();
                                    BPztb0.Text = ((JSONObject)json["2"])["operateMoney"].ToString().Equals("无") ? "" : ((JSONObject)json["2"])["operateMoney"].ToString();
                                    BPztb1.Text = ((JSONObject)json["2"])["payWay"].ToString().Equals("无") ? "" : ((JSONObject)json["2"])["payWay"].ToString();
                                    BPztb2.Text = ((JSONObject)json["2"])["payNumber"].ToString().Equals("无") ? "" : ((JSONObject)json["2"])["payNumber"].ToString();
                                    BPztb3.Text = ((JSONObject)json["2"])["payCount"].ToString().Equals("无") ? "" : ((JSONObject)json["2"])["payCount"].ToString();
                                }
                                if ((JSONObject)json["3"] != null)
                                {
                                    PzcbC.Text = ((JSONObject)json["3"])["zhaiyao"].ToString().Equals("无") ? "" : ((JSONObject)json["3"])["zhaiyao"].ToString();
                                    CPztb0.Text = ((JSONObject)json["3"])["operateMoney"].ToString().Equals("无") ? "" : ((JSONObject)json["3"])["operateMoney"].ToString();
                                    CPztb1.Text = ((JSONObject)json["3"])["payWay"].ToString().Equals("无") ? "" : ((JSONObject)json["3"])["payWay"].ToString();
                                    CPztb2.Text = ((JSONObject)json["3"])["payNumber"].ToString().Equals("无") ? "" : ((JSONObject)json["3"])["payNumber"].ToString();
                                    CPztb3.Text = ((JSONObject)json["3"])["payCount"].ToString().Equals("无") ? "" : ((JSONObject)json["3"])["payCount"].ToString();
                                }
                                if ((JSONObject)json["4"] != null)
                                {
                                    PzcbD.Text = ((JSONObject)json["4"])["zhaiyao"].ToString().Equals("无") ? "" : ((JSONObject)json["4"])["zhaiyao"].ToString();
                                    DPztb0.Text = ((JSONObject)json["4"])["operateMoney"].ToString().Equals("无") ? "" : ((JSONObject)json["4"])["operateMoney"].ToString();
                                    DPztb1.Text = ((JSONObject)json["4"])["payWay"].ToString().Equals("无") ? "" : ((JSONObject)json["4"])["payWay"].ToString();
                                    DPztb2.Text = ((JSONObject)json["4"])["payNumber"].ToString().Equals("无") ? "" : ((JSONObject)json["4"])["payNumber"].ToString();
                                    DPztb3.Text = ((JSONObject)json["4"])["payCount"].ToString().Equals("无") ? "" : ((JSONObject)json["4"])["payCount"].ToString();
                                }
                                if ((JSONObject)json["5"] != null)
                                {
                                    PzcbE.Text = ((JSONObject)json["5"])["zhaiyao"].ToString().Equals("无") ? "" : ((JSONObject)json["5"])["zhaiyao"].ToString();
                                    EPztb0.Text = ((JSONObject)json["5"])["operateMoney"].ToString().Equals("无") ? "" : ((JSONObject)json["5"])["operateMoney"].ToString();
                                    EPztb1.Text = ((JSONObject)json["5"])["payWay"].ToString().Equals("无") ? "" : ((JSONObject)json["5"])["payWay"].ToString();
                                    EPztb2.Text = ((JSONObject)json["5"])["payNumber"].ToString().Equals("无") ? "" : ((JSONObject)json["5"])["payNumber"].ToString();
                                    EPztb3.Text = ((JSONObject)json["5"])["payCount"].ToString().Equals("无") ? "" : ((JSONObject)json["5"])["payCount"].ToString();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            FormBasicFeatrues.GetInstence().RecordLog(ex, "无法查看详细");
                            MessageBox.Show("无法查看详细 - " + ex.Message, "错误");
                            this.Close();
                        }
                    }

                    break;
                case 6:
                    // 合同
                    detailedPanel = DetailedHTPanel;
                    detailedLocationY = 80;
                    detailedHeightDis = 60;

                    table = "htList";
                    baseName = "htID";
                    queryArray = new string[] { "htID", "leixing", "htDate", "companyName", "sum", "discardFlag" };
                    controlsPreName = "HTtbID";
                    indexCount = 6;
                    mainID = HTtbID.Text;

                    canPrint = true;
                    if (ItemId.Equals("-1"))
                    {
                        HTcbName.Visible = true;
                        HTcbName.Items.Clear();
                        HTcbName.Items.Add("购买合同");
                        HTcbName.Items.Add("销售合同");
                        HTcbName.SelectedIndex = 0;
                    }
                    else
                    {
                        HTcbName.Visible = false;

                        try
                        {
                            FormBasicFeatrues.GetInstence().SetControlsVaule(controlsPreName, detailedPanel, DatabaseConnections.GetInstence().LocalGetOneRowDataById(table, queryArray, baseName, ItemId));
                        }
                        catch (Exception ex)
                        {
                            FormBasicFeatrues.GetInstence().RecordLog(ex, "无法查看详细");
                            MessageBox.Show("无法查看详细 - " + ex.Message, "错误");
                            this.Close();
                        }
                    }
                    break;
            }

            this.Size = new Size(this.Size.Width, this.Size.Height - detailedHeightDis);
            PreviewPrintButton.Location = new Point(PreviewPrintButton.Location.X, PreviewPrintButton.Location.Y - detailedHeightDis);
            SaveButton.Location = new Point(SaveButton.Location.X, SaveButton.Location.Y - detailedHeightDis);
            DiscardCheckBox.Location = new Point(DiscardCheckBox.Location.X, DiscardCheckBox.Location.Y - detailedHeightDis);
            detailedPanel.Parent = this;
            detailedPanel.Location = new Point(15, 5);
            DetailedTabView.Visible = false;

            if (!canPrint)
            {
                PreviewPrintButton.Visible = false;
            }
        }

        private void makeControlsInvisibleForJCCD(Boolean isVisable)
        {
            label50.Visible = isVisable;
            label49.Visible = isVisable;
            label48.Visible = isVisable;
            label47.Visible = isVisable;
            label46.Visible = isVisable;
            tbDz9.Visible = isVisable;
            tbDz10.Visible = isVisable;
            tbDz11.Visible = isVisable;
            tbDz12.Visible = isVisable;
            tbDz13.Visible = isVisable;
        }

        /// <summary>
        /// 把Control值转到JSON
        /// </summary>
        /// <param name="keyList"></param>
        /// <param name="conList"></param>
        /// <returns></returns>
        private String ControlValueTransitToJson(List<String> keyList, List<List<Control>> conList)
        {
            JSONObject jsonRoot = new JSONObject();
            for (int i = 0; i < conList.Count; i++)
            {
                JSONObject jsonObj = new JSONObject();
                for (int j = 0; j < conList[i].Count; j++)
                {
                    jsonObj.Add(keyList[j], conList[i][j].Text.Equals("") ? "无" : conList[i][j].Text);
                }
                jsonRoot.Add((i + 1).ToString(), jsonObj);
            }
            return JSONConvert.SerializeObject(jsonRoot);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormBasicFeatrues.GetInstence().isPassValidateControls(checkValidateControls))
                {
                    if ((MainWindow.CURRENT_TAB == 3) || (MainWindow.CURRENT_TAB == 4)) // 进仓单 出仓单
                    {
                        String jsonData = ControlValueTransitToJson(
                            new List<String>() { "goodsID", "goodsAmount" },
                            new List<List<Control>>() {
                                new List<Control> (){JCDcbA, AJCDtb5 } ,
                                new List<Control> (){JCDcbB, BJCDtb5 } ,
                                new List<Control> (){JCDcbC, CJCDtb5 } ,
                                new List<Control> (){JCDcbD, DJCDtb5 } ,
                                new List<Control> (){JCDcbE, EJCDtb5 } 
                                }
                            );
                        // 
                        DatabaseConnections.GetInstence().LocalReplaceIntoData(
                            table,
                            queryArray,
                            new String[] { 
                                tbDz1.Text,
                                tbDz2.Text, 
                                dzCompany.Text,
                                AJCDtb0.Text+","+BJCDtb0.Text+","+CJCDtb0.Text+","+DJCDtb0.Text+","+EJCDtb0.Text,
                                jsonData,
                                tbDz3.Text.Split('=')[0],
                                tbDz4.Text,
                                tbDz6.Text,
                                tbDz7.Text,
                                tbDz8.Text,
                                (DiscardCheckBox.Checked?"1":"0"),
                                DateTime.Now.ToString(), 
                                DateTime.Now.ToString(),
                                tbDz9.Text,
                                tbDz10.Text,
                                tbDz11.Text,
                                tbDz12.Text,
                                tbDz13.Text},
                            mainID);
                    }
                    else if (MainWindow.CURRENT_TAB == 5)  // 凭证
                    {
                        String jsonData = ControlValueTransitToJson(
                                                   new List<String>() { "zhaiyao", "operateMoney", "payWay", "payNumber", "payCount" },
                                                   new List<List<Control>>() {
                                new List<Control> (){PzcbA,APztb0,APztb1,APztb2,APztb3 } ,
                                new List<Control> (){PzcbB,BPztb0,BPztb1,BPztb2,BPztb3 } ,
                                new List<Control> (){PzcbC,CPztb0,CPztb1,CPztb2,CPztb3 } ,
                                new List<Control> (){PzcbD,DPztb0,DPztb1,DPztb2,DPztb3 } ,
                                new List<Control> (){PzcbE,EPztb0,EPztb1,EPztb2,EPztb3 } 
                                }
                                                   );
                        // "pzID", "leixing", "clientID", "companyName", "jsonData", "operateMoney", "remaintingMoney", "discardFlag", "addtime", "modifyTime"
                        DatabaseConnections.GetInstence().LocalReplaceIntoData(
                            table,
                            queryArray,
                            new String[] { 
                                tbPz1.Text,
                                tbPz2.Text,
                                pzComboBox.SelectedIndex.ToString(), 
                                pzCompany.Text,
                                jsonData,
                                SumtbPz.Text.Split('=')[0],
                                "",
                                tbPz3.Text,
                                (DiscardCheckBox.Checked?"1":"0"),
                                DateTime.Now.ToString(), 
                                DateTime.Now.ToString()},
                            mainID);
                    }
                    else
                    {
                        DatabaseConnections.GetInstence().LocalReplaceIntoData(table, queryArray, FormBasicFeatrues.GetInstence().GetControlsVaule(controlsPreName, detailedPanel, indexCount), mainID);
                    }

                    if (ItemId.Equals("-1"))
                    {
                        MessageBox.Show("新建成功!", "恭喜");
                    }
                    else
                    {
                        MessageBox.Show("保存成功!", "恭喜");
                    }
                    // 刷新
                    MainWindow.CURRENT_LIST_BUTTON.PerformClick();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("请先填入必填项目再保存!", "提示");
                }
            }
            catch (Exception ex)
            {
                FormBasicFeatrues.GetInstence().RecordLog(ex, "");
                MessageBox.Show(ex.Message, "错误");
                this.Close();
            }
        }

        private void PreviewPrintButton_Click(object sender, EventArgs e)
        {
            SetPrintPreview(7);
            //SetPrintPreview(MainWindow.CURRENT_TAB);
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
            dzContact.Text = "hahahh ";
            dzAddress.Text = "hahahh ";
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
            g.DrawString("TAL：", tbDz2.Font, new SolidBrush(dzContact.ForeColor), tableX + x, 50 + y + fontDisY);
            //g.DrawRectangle(new Pen(Color.Black), tableX + x + 733 - tbDz2.Size.Width, 80 + y, tbDz2.Size.Width, tbDz2.Height);
            g.DrawString("", tbDz1.Font, new SolidBrush(dzContact.ForeColor), tableX + x, 50 + y + fontDisY);

            fontSize = g.MeasureString("FAX：", f2);
            g.DrawString("FAX：", tbDz2.Font, new SolidBrush(dzContact.ForeColor), tableX + x, 80 + y + fontDisY);
            //g.DrawRectangle(new Pen(Color.Black), tableX + x + 733 - tbDz2.Size.Width, 80 + y, tbDz2.Size.Width, tbDz2.Height);
            g.DrawString("", tbDz2.Font, new SolidBrush(dzContact.ForeColor), tableX + x, 80 + y + fontDisY);

            //   以质为根   以诚为本
            fontSize = g.MeasureString("   以质为根   以诚为本", f2);
            g.DrawString("   以质为根   以诚为本", tbDz2.Font, new SolidBrush(dzContact.ForeColor), tableX + x, tableY - 26 * 3 + y + fontDisY);
            //g.DrawRectangle(new Pen(Color.Black), tableX + x + 733 - tbDz2.Size.Width, 80 + y, tbDz2.Size.Width, tbDz2.Height);
            g.DrawString("", tbDz2.Font, new SolidBrush(dzContact.ForeColor), tableX + x, tableY - 26 * 3 + y + fontDisY);

            fontSize = g.MeasureString("*客户编号：", f2);
            g.DrawString("*客户编号：", tbDz2.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - fontSize.Width - tbDz1.Size.Width - 5, 50 + y + fontDisY);
            //g.DrawRectangle(new Pen(Color.Black), tableX + x + 733 - tbDz2.Size.Width, 80 + y, tbDz2.Size.Width, tbDz2.Height);
            g.DrawString(tbDz1.Text, tbDz1.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - tbDz2.Size.Width + fontDisX, 50 + y + fontDisY);

            fontSize = g.MeasureString("凭证号码：", f2);
            g.DrawString("凭证号码：", tbDz2.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - fontSize.Width - tbDz2.Size.Width - 5, 80 + y + fontDisY);
            //g.DrawRectangle(new Pen(Color.Black), tableX + x + 733 - tbDz2.Size.Width, 80 + y, tbDz2.Size.Width, tbDz2.Height);
            g.DrawString(tbDz2.Text, tbDz2.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - tbDz2.Size.Width + fontDisX, 80 + y + fontDisY);


            fontSize = g.MeasureString("业务联/白色； 财务联/黄色； 仓库联/蓝色； 客户联/红色", f2);
            g.DrawString("业务联/白色； 财务联/黄色； 仓库联/蓝色； 客户联/红色", f2, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX + 733 - fontSize.Width, tableY - 26 * 3 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x, tableY - 26 * 2 + y, 116, dzContact.Height);
            g.DrawString("对方单位：", dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, tableY - 26 * 2 + y, 268, dzContact.Height);
            g.DrawString(dzContact.Text, dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 116 + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 384, tableY - 26 * 2 + y, 111, dzContact.Height);
            g.DrawString("日期：", dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 384 + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 111 + 384, tableY - 26 * 2 + y, 238, dzContact.Height);
            g.DrawString(DateTime.Now.ToLongDateString(), dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 111 + 384 + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x, tableY - 26 + y, 116, dzContact.Height);
            g.DrawString("联系地址：", dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, tableY - 26 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, tableY - 26 + y, 268, dzAddress.Height);
            g.DrawString(dzAddress.Text, dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 116 + fontDisX, tableY - 26 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 384, tableY - 26 + y, 111, dzContact.Height);
            g.DrawString("联系电话：", dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 384 + fontDisX, tableY - 26 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 111 + 384, tableY - 26 + y, 238, dzPhone.Height);
            g.DrawString(dzPhone.Text, dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 111 + 384 + fontDisX, tableY - 26 + y + fontDisY);

            // 表格之后
            g.DrawRectangle(new Pen(Color.Black), tableX + x, 156 + y + tableY, 116, tbDz3.Height);
            g.DrawString("总金额：", tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, 156 + y + fontDisY + tableY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, 156 + y + tableY, 617, tbDz3.Height);
            g.DrawString(tbDz3.Text, tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 116 + fontDisX, 156 + y + fontDisY + tableY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x, 182 + y + tableY, 452, 104);
            g.DrawString("备注：", tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, 182 + y + fontDisY + tableY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 452, 182 + y + tableY, 281, 78);
            g.DrawString("发票号码：", tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 452 + fontDisX, 182 + y + fontDisY + tableY);
            g.DrawString("增：", tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 452 + fontDisX, 208 + y + fontDisY + tableY);
            g.DrawString("普：", tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 452 + fontDisX, 234 + y + fontDisY + tableY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 452, 260 + y + tableY, 281, 26);
            g.DrawString("附件凭证 " + tbDz8.Text + " 张", tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 452 + fontDisX, 260 + y + fontDisY + tableY);

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
            int htWidth = (pageWidth - 60 * 2) / 7;
            int htHeight = 37;
            int a = 40;
            int b = 230;
            //第一行 项目名称
            g.DrawString("商品名称", f4, new SolidBrush(Color.Black), 43 + x + 20, 242 + y);
            g.DrawString("商品规格", f4, new SolidBrush(Color.Black), 146 + x + 20, 242 + y);
            g.DrawString("数量", f4, new SolidBrush(Color.Black), 243 + x + 20, 242 + y);
            g.DrawString("单位", f4, new SolidBrush(Color.Black), 317 + x + 20, 242 + y);
            g.DrawString("单价", f4, new SolidBrush(Color.Black), 392 + x + 20, 242 + y);
            g.DrawString("金额", f4, new SolidBrush(Color.Black), 464 + x + 20, 242 + y);
            g.DrawString("交（提）货日期及地点", f4, new SolidBrush(Color.Black), 555 + x + 20, 242 + y);
            //第二行
            g.DrawString("1", f4, new SolidBrush(Color.Black), 43 + x + 20, 279 + y);
            g.DrawString("1", f4, new SolidBrush(Color.Black), 146 + x + 20, 279 + y);
            g.DrawString("1", f4, new SolidBrush(Color.Black), 243 + x + 20, 279 + y);
            g.DrawString("1", f4, new SolidBrush(Color.Black), 317 + x + 20, 279 + y);
            g.DrawString("1", f4, new SolidBrush(Color.Black), 392 + x + 20+2, 279 + y);
            g.DrawString("1", f4, new SolidBrush(Color.Black), 464 + x + 20+2, 279 + y);
            g.DrawString("1", f4, new SolidBrush(Color.Black), 555 + x + 20, 279 + y);
            //第三行
            g.DrawString("2", f4, new SolidBrush(Color.Black), 43 + x + 20, 318 + y);
            g.DrawString("2", f4, new SolidBrush(Color.Black), 146 + x + 20, 318 + y);
            g.DrawString("2", f4, new SolidBrush(Color.Black), 243 + x + 20, 318 + y);
            g.DrawString("2", f4, new SolidBrush(Color.Black), 317 + x + 20, 318 + y);
            g.DrawString("2", f4, new SolidBrush(Color.Black), 392 + x + 20+2, 318 + y);
            g.DrawString("2", f4, new SolidBrush(Color.Black), 464 + x + 20+2, 318 + y);
            g.DrawString("2", f4, new SolidBrush(Color.Black), 555 + x + 20, 318 + y);
            //第四行
            g.DrawString("3", f4, new SolidBrush(Color.Black), 43 + x + 20, 355 + y);
            g.DrawString("3", f4, new SolidBrush(Color.Black), 146 + x + 20, 355 + y);
            g.DrawString("3", f4, new SolidBrush(Color.Black), 243 + x + 20, 355 + y);
            g.DrawString("3", f4, new SolidBrush(Color.Black), 317 + x + 20, 355 + y);
            g.DrawString("3", f4, new SolidBrush(Color.Black), 392 + x + 20+2, 355 + y);
            g.DrawString("3", f4, new SolidBrush(Color.Black), 464 + x + 20+2, 355 + y);
            g.DrawString("3", f4, new SolidBrush(Color.Black), 555 + x + 20, 355 + y);
            //第五行
            g.DrawString("4", f4, new SolidBrush(Color.Black), 43 + x + 20, 392 + y);
            g.DrawString("4", f4, new SolidBrush(Color.Black), 146 + x + 20, 392 + y);
            g.DrawString("4", f4, new SolidBrush(Color.Black), 243 + x + 20, 392 + y);
            g.DrawString("4", f4, new SolidBrush(Color.Black), 317 + x + 20, 392 + y);
            g.DrawString("4", f4, new SolidBrush(Color.Black), 392 + x + 20+2, 392 + y);
            g.DrawString("4", f4, new SolidBrush(Color.Black), 464 + x + 20+2, 392 + y);
            g.DrawString("4", f4, new SolidBrush(Color.Black), 555 + x + 20, 392 + y);
            //第六行
            g.DrawString("5", f4, new SolidBrush(Color.Black), 43 + x + 20, 429 + y);
            g.DrawString("5", f4, new SolidBrush(Color.Black), 146 + x + 20, 429 + y);
            g.DrawString("5", f4, new SolidBrush(Color.Black), 243 + x + 20, 429 + y);
            g.DrawString("5", f4, new SolidBrush(Color.Black), 317 + x + 20, 429 + y);
            g.DrawString("5", f4, new SolidBrush(Color.Black), 392 + x + 20+2, 429 + y);
            g.DrawString("5", f4, new SolidBrush(Color.Black), 464 + x + 20+2, 429 + y);
            g.DrawString("5", f4, new SolidBrush(Color.Black), 555 + x + 20, 429 + y);
            //第一列
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y+htHeight, htWidth, htHeight-2);
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y + 2*htHeight-2, htWidth, htHeight+2);
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y + 3 * htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y + 4 * htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y + 5 * htHeight, htWidth, htHeight);
            //g.DrawString(, f4, new SolidBrush(Color.Black), 555 + x + 20, 242 + y);

           
            //第二列
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y, htWidth - 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y + htHeight, htWidth-2, htHeight-2);
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y + 2 * htHeight-2, htWidth - 2, htHeight+2);
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y + 3 * htHeight, htWidth - 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y + 4 * htHeight, htWidth - 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y + 5 * htHeight, htWidth - 2, htHeight);
            //第三列
            g.DrawRectangle(new Pen(Color.Black), a + 2*htWidth + x-2, b + y, htWidth-27, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 2 * htWidth + x-2, b + y + htHeight, htWidth -27, htHeight-2);
            g.DrawRectangle(new Pen(Color.Black), a + 2 * htWidth + x - 2, b + y + 2 * htHeight-2, htWidth - 27, htHeight+2);
            g.DrawRectangle(new Pen(Color.Black), a + 2 * htWidth + x - 2, b + y + 3 * htHeight, htWidth - 27, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 2 * htWidth + x - 2, b + y + 4 * htHeight, htWidth - 27, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 2 * htWidth + x - 2, b + y + 5 * htHeight, htWidth - 27, htHeight);
            //第四列
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27-2, b + y, htWidth - 27+1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27-2, b + y + htHeight, htWidth - 27+1, htHeight-2);
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27-2, b + y + 2 * htHeight-2, htWidth - 27+1, htHeight+2);
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27-2, b + y + 3 * htHeight, htWidth - 27+1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27-2, b + y + 4 * htHeight, htWidth - 27+1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27-2, b + y + 5 * htHeight, htWidth - 27+1, htHeight);
            //第五列
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54-1, b + y, htWidth - 27+1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54-1, b + y + htHeight, htWidth - 27+1, htHeight-2);
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54-1, b + y + 2 * htHeight-2, htWidth - 27+1, htHeight+2);
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54-1, b + y + 3 * htHeight, htWidth - 27+1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54-1, b + y + 4 * htHeight, htWidth - 27+1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54-1, b + y + 5 * htHeight, htWidth - 27+1, htHeight);
            //第六列
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x-81, b + y, htWidth - 27+2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x - 81, b + y + htHeight, htWidth - 27+2, htHeight-2);
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x - 81, b + y + 2 * htHeight-2, htWidth - 27+2, htHeight+2);
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x - 81, b + y + 3 * htHeight, htWidth - 27+2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x - 81, b + y + 4 * htHeight, htWidth - 27+2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x - 81, b + y + 5 * htHeight, htWidth - 27+2, htHeight);
            //第七列
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x-108+2, b + y, htWidth+108-2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x - 108+2, b + y + htHeight, htWidth + 108-2, htHeight-2);
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x - 108+2, b + y + 2 * htHeight-2, htWidth + 108-2, htHeight+2);
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x - 108+2, b + y + 3 * htHeight, htWidth + 108-2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x - 108+2, b + y + 4 * htHeight, htWidth + 108-2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x - 108+2, b + y + 5 * htHeight, htWidth + 108-2, htHeight);

            /*
             * //第八列
            g.DrawRectangle(new Pen(Color.Black), 40 + 7*htWidth + x, 223 + y, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + 7 * htWidth + x, 223 + y + htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + 7 * htWidth + x, 223 + y + 2 * htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + 7 * htWidth + x, 223 + y + 3 * htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + 7 * htWidth + x, 223 + y + 4 * htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + 7 * htWidth + x, 223 + y + 5 * htHeight, htWidth, htHeight);
            //第九列
            g.DrawRectangle(new Pen(Color.Black), 40 + 8*htWidth + x, 223 + y, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + 8 * htWidth + x, 223 + y + htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + 8 * htWidth + x, 223 + y + 2 * htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + 8 * htWidth + x, 223 + y + 3 * htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + 8 * htWidth + x, 223 + y + 4 * htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + 8 * htWidth + x, 223 + y + 5 * htHeight, htWidth, htHeight);
            /*
            /*
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
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Location.X + 36 + x, tx.Location.Y + 240+ y + fontDisY);
                    if (tx.BorderStyle == BorderStyle.FixedSingle)
                    {
                        g.DrawRectangle(new Pen(Color.Black), tx.Left + 20 + x, tx.Top + 240 + y, tx.Width, tx.Height);
                    }
                }
                if (item is ComboBox)
                {
                    ComboBox tx = (item as ComboBox);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + 40 + x + fontDisX, tx.Top + 240 + y + fontDisY);
                    g.DrawRectangle(new Pen(Color.Black), tx.Left + 20 + x, tx.Top + 240 + y, tx.Width, 25);
                }
            }
            */

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
            //框内文字
            
            g.DrawString("7", f4, new SolidBrush(Color.Black),  x +recWidth/2-20, 740 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black),  x + recWidth / 2-20, 770 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black),  x + recWidth / 2-20, 800 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black),  x + recWidth / 2-20, 850 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black),  x + recWidth / 2-20, 880 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black),  x + recWidth / 2-20, 910 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black),  x + recWidth / 2-20, 960 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black),  x + recWidth / 2-20, 990 + y);

            g.DrawString("7", f4, new SolidBrush(Color.Black), x + recWidth / 2 - 20 + recWidth, 740 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black), x + recWidth / 2 - 20 + recWidth, 770 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black), x + recWidth / 2 - 20 + recWidth, 800 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black), x + recWidth / 2 - 20 + recWidth, 850 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black), x + recWidth / 2 - 20 + recWidth, 880 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black), x + recWidth / 2 - 20 + recWidth, 910 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black), x + recWidth / 2 - 20 + recWidth, 960 + y);
            g.DrawString("7", f4, new SolidBrush(Color.Black), x + recWidth / 2 - 20 + recWidth, 990 + y);



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

        }

        // 进仓单 出仓单 采购单 销售单
        private void danziComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormBasicFeatrues.GetInstence().reTriggleCombox(JCDcbA);
            FormBasicFeatrues.GetInstence().reTriggleCombox(JCDcbB);
            FormBasicFeatrues.GetInstence().reTriggleCombox(JCDcbC);
            FormBasicFeatrues.GetInstence().reTriggleCombox(JCDcbD);
            FormBasicFeatrues.GetInstence().reTriggleCombox(JCDcbE);

            if (MainWindow.CURRENT_TAB == 3) //仓储管理
            {
                controlsPreName = "tbDz";
                indexCount = 11;
                mainID = tbDz2.Text;

                switch (danziComboBox.SelectedIndex)
                {
                    case 0://进仓单
                        lbDzTitle.Text = "商品（货物）进仓单";
                        table = "jcdList";
                        baseName = "jcdID";
                        queryArray = new string[] { "clientID", "jcdID", "companyName", "goodsName", "jsonData", "sum", "beizhu", "fpPu", "fpZeng", "fpCount", "discardFlag", "addtime", "modifyTime" };
                        break;
                    case 1://出仓单
                        lbDzTitle.Text = "商品（货物）出仓单";
                        table = "ccdList";
                        baseName = "ccdID";
                        queryArray = new string[] { "clientID", "ccdID", "companyName", "goodsName", "jsonData", "sum", "beizhu", "fpPu", "fpZeng", "fpCount", "discardFlag", "addtime", "modifyTime" };
                        break;
                    default:
                        break;
                }
            }
            else if (MainWindow.CURRENT_TAB == 4) //业务管理
            {
                switch (danziComboBox.SelectedIndex)
                {
                    case 0://采购单
                        lbDzTitle.Text = "商品（货物）采购单";
                        table = "cgdList";
                        baseName = "cgdID";
                        queryArray = new string[] { "clientID", baseName, "companyName", "goodsName", "jsonData", "sum", "beizhu", "fpPu", "fpZeng", "fpCount", "discardFlag", "addtime", "modifyTime", "kxQq", "kxXq", "kxJf", "kxSq", "kxDay" };
                        // 自动生成ID
                        tbDz2.Text = DatabaseConnections.GetInstence().LocalAutoincreaseID(table, baseName);
                        break;
                    case 1://销售单
                        lbDzTitle.Text = "商品（货物）销售单";
                        table = "xsdList";
                        baseName = "xsdID";
                        queryArray = new string[] { "clientID", baseName, "companyName", "goodsName", "jsonData", "sum", "beizhu", "fpPu", "fpZeng", "fpCount", "discardFlag", "addtime", "modifyTime", "kxQq", "kxXq", "kxJf", "kxSq", "kxDay" };
                        // 自动生成ID
                        tbDz2.Text = DatabaseConnections.GetInstence().LocalAutoincreaseID(table, baseName);
                        break;
                }
            }
        }

        private void pzComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            table = "pzList";
            baseName = "pzID";
            queryArray = new string[] { "clientID", "pzID", "leixing", "companyName", "jsonData", "operateMoney", "remaintingMoney", "beizhu", "discardFlag", "addtime", "modifyTime" };
            controlsPreName = "tbPz";
            // 自动生成ID
            tbPz2.Text = DatabaseConnections.GetInstence().LocalAutoincreaseID(table, baseName);
            switch (pzComboBox.SelectedIndex)
            {
                default:
                    break;
                case 0:// 收款凭证
                    lbPzTitle.Text = "收 款 凭 证";
                    break;
                case 1:// 付款凭证
                    lbPzTitle.Text = "付 款 凭 证";
                    break;
                case 2:// 领款凭证
                    lbPzTitle.Text = "领 款 凭 证";
                    break;
                case 3:// 还款凭证
                    lbPzTitle.Text = "还 款 凭 证";
                    break;
                case 4:// 报销凭证
                    lbPzTitle.Text = "报 销 凭 证";
                    break;
            }
        }

        private void addItemsToCombox(List<String> items, ComboBox cb)
        {
            foreach (string item in items)
            {
                cb.Items.Add(item);
            }
        }

        // 进仓单 出仓单 
        private void JCCDSetControlsValue(List<Control> lcs, Control byIdControl)
        {
            if (danziComboBox.SelectedIndex == 0)
            { // 进仓单
                FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(lcs, DatabaseConnections.GetInstence().LocalGetOneRowDataById("goods", new String[] { "name", "guige", "dengji", "unit", "purchasePrice" }, "id", byIdControl.Text).ToList<String>());
            }
            else
            { // 出仓单
                FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(lcs, DatabaseConnections.GetInstence().LocalGetOneRowDataById("goods", new String[] { "name", "guige", "dengji", "unit", "currntsalesPrice" }, "id", byIdControl.Text).ToList<String>());
            }
        }
        /// <summary>
        /// 清空之后的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void clearControlValueByList(List<Control> conList)
        {
            foreach (Control con in conList)
            {
                con.Text = "";
            }
        }

        private void JCDcbA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JCDcbA.SelectedIndex != -1)
            {
                if (JCDcbA.Text.Equals(""))
                {
                    AJCDtb5.ReadOnly = true;
                    clearControlValueByList(new List<Control>() { AJCDtb0, AJCDtb1, AJCDtb2, AJCDtb3, AJCDtb4, AJCDtb5, AJCDtb6 });
                }
                else
                {
                    AJCDtb5.ReadOnly = false;
                    JCCDSetControlsValue(new List<Control>() { AJCDtb0, AJCDtb1, AJCDtb2, AJCDtb3, AJCDtb4 }, JCDcbA);
                }
            }
        }

        private void JCDcbB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JCDcbB.SelectedIndex != -1)
            {
                if (JCDcbB.Text.Equals(""))
                {
                    BJCDtb5.ReadOnly = true;
                    clearControlValueByList(new List<Control>() { BJCDtb0, BJCDtb1, BJCDtb2, BJCDtb3, BJCDtb4, BJCDtb5, BJCDtb6 });
                }
                else
                {
                    BJCDtb5.ReadOnly = false;
                    JCCDSetControlsValue(new List<Control>() { BJCDtb0, BJCDtb1, BJCDtb2, BJCDtb3, BJCDtb4 }, JCDcbB);
                }
            }
        }

        private void JCDcbC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JCDcbC.SelectedIndex != -1)
            {
                if (JCDcbC.Text.Equals(""))
                {
                    CJCDtb5.ReadOnly = true;
                    clearControlValueByList(new List<Control>() { CJCDtb0, CJCDtb1, CJCDtb2, CJCDtb3, CJCDtb4, CJCDtb5, CJCDtb6 });
                }
                else
                {
                    CJCDtb5.ReadOnly = false;
                    JCCDSetControlsValue(new List<Control>() { CJCDtb0, CJCDtb1, CJCDtb2, CJCDtb3, CJCDtb4 }, JCDcbC);
                }
            }
        }

        private void JCDcbD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JCDcbD.SelectedIndex != -1)
            {
                if (JCDcbD.Text.Equals(""))
                {
                    DJCDtb5.ReadOnly = true;
                    clearControlValueByList(new List<Control>() { DJCDtb0, DJCDtb1, DJCDtb2, DJCDtb3, DJCDtb4, DJCDtb5, DJCDtb6 });
                }
                else
                {
                    DJCDtb5.ReadOnly = false;
                    JCCDSetControlsValue(new List<Control>() { DJCDtb0, DJCDtb1, DJCDtb2, DJCDtb3, DJCDtb4 }, JCDcbD);
                }
            }
        }

        private void JCDcbE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JCDcbE.SelectedIndex != -1)
            {
                if (JCDcbE.Text.Equals(""))
                {
                    EJCDtb5.ReadOnly = true;
                    clearControlValueByList(new List<Control>() { EJCDtb0, EJCDtb1, EJCDtb2, EJCDtb3, EJCDtb4, EJCDtb5, EJCDtb6 });
                }
                else
                {
                    EJCDtb5.ReadOnly = false;
                    JCCDSetControlsValue(new List<Control>() { EJCDtb0, EJCDtb1, EJCDtb2, EJCDtb3, EJCDtb4 }, JCDcbE);
                }
            }
        }

        //
        private void tbDz1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(new List<Control>() { dzContact, dzPhone, dzCompany, dzAddress }, DatabaseConnections.GetInstence().LocalGetOneRowDataById("clients", new String[] { "contact", "phone", "company", "address" }, "id", tbDz1.Text).ToList<String>());
        }

        private void tbPz1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(new List<Control>() { pzContact, pzPhone, pzCompany, pzAddress }, DatabaseConnections.GetInstence().LocalGetOneRowDataById("clients", new String[] { "contact", "phone", "company", "address" }, "id", tbPz1.Text).ToList<String>());
        }

        /// <summary>
        /// 计算金钱价格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        private void calculateSmallSum(Control left, Control right, Control result)
        {
            int test1, test2;
            if (left.Text.Equals("") || right.Text.Equals("") || !int.TryParse(left.Text, out test1) || !int.TryParse(right.Text, out test2))
            {

            }
            else
            {
                result.Text = (int.Parse(left.Text) * int.Parse(right.Text)).ToString();
            }
        }
        private void AJCDtb5_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(AJCDtb4, AJCDtb5, AJCDtb6);
        }

        private void BJCDtb5_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(BJCDtb4, BJCDtb5, BJCDtb6);
        }

        private void CJCDtb5_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(CJCDtb4, CJCDtb5, CJCDtb6);
        }

        private void DJCDtb5_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(DJCDtb4, DJCDtb5, DJCDtb6);
        }

        private void EJCDtb5_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(EJCDtb4, EJCDtb5, EJCDtb6);
        }

        /// <summary>
        /// 计算总共价钱并设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void SetTotalSum(List<Control> conList, Control resultControl)
        {
            int sum = 0;
            foreach (Control con in conList)
            {
                if (!con.Text.Equals(""))
                {
                    sum += int.Parse(con.Text);
                }
            }
            resultControl.Text = sum.ToString() + "=" + FormBasicFeatrues.GetInstence().MoneyToUpper(sum.ToString());
        }

        private void calculateSumForDz(object sender, EventArgs e)
        {
            SetTotalSum(new List<Control>() { AJCDtb6, BJCDtb6, CJCDtb6, DJCDtb6, EJCDtb6 }, tbDz3);
        }

        private void calculateSumForPz(object sender, EventArgs e)
        {
            SetTotalSum(new List<Control>() { APztb0, BPztb0, CPztb0, DPztb0, EPztb0 }, SumtbPz);
        }

        /// <summary>
        /// 只能输入字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberInputOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符
                }
            }
        }
        #region 选择现金 银行卡 其他

        private void APztb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (APztb1.SelectedIndex == 0)
            {
                APztb2.ReadOnly = true;
                APztb3.ReadOnly = true;
            }
            else
            {
                APztb2.ReadOnly = false;
                APztb3.ReadOnly = false;
            }
        }

        private void BPztb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BPztb1.SelectedIndex == 0)
            {
                BPztb2.ReadOnly = true;
                APztb3.ReadOnly = true;
            }
            else
            {
                BPztb2.ReadOnly = false;
                BPztb3.ReadOnly = false;
            }
        }

        private void CPztb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CPztb1.SelectedIndex == 0)
            {
                CPztb2.ReadOnly = true;
                CPztb3.ReadOnly = true;
            }
            else
            {
                CPztb2.ReadOnly = false;
                CPztb3.ReadOnly = false;
            }
        }

        private void DPztb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DPztb1.SelectedIndex == 0)
            {
                DPztb2.ReadOnly = true;
                DPztb3.ReadOnly = true;
            }
            else
            {
                DPztb2.ReadOnly = false;
                DPztb3.ReadOnly = false;
            }
        }

        private void EPztb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EPztb1.SelectedIndex == 0)
            {
                EPztb2.ReadOnly = true;
                EPztb3.ReadOnly = true;
            }
            else
            {
                EPztb2.ReadOnly = false;
                EPztb3.ReadOnly = false;
            }
        }
        #endregion

        private void TextBoxCheckIfDuplicate_Validated(object sender, EventArgs e)
        {
            if (ItemId.Equals("-1") || !ItemId.Equals((sender as TextBox).Text))
            {
                if (DatabaseConnections.GetInstence().LocalCheckIfDuplicate(table, baseName, (sender as TextBox).Text))
                {
                    MessageBox.Show("您设定的编号已经被占用, 请再次输入", "错误");
                    (sender as TextBox).Text = ItemId.Equals("-1") ? "" : ItemId;
                    (sender as TextBox).Focus();
                }
            }
        }



    }
}
