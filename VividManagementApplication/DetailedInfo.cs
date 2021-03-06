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
        public Boolean isNewWindow = false;
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
                    checkValidateControls = new List<Control>() { tbClient1, tbClient3, tbClient4, tbClient5, tbClient6, tbClient7, tbClient12, tbClient13 };
                    detailedHeightDis = 250;
                    detailedPanel = DetailedClientPanel;

                    table = "clients";
                    baseName = "clientID";
                    queryArray = new string[] { baseName, "gzbID", "type", "company", "companyOwner", "address", "phone", "fax", "QQ", "taxNumber", "email", "bankName", "bankCard", "PrivateAccount", "shouldReceive", "shouldPay", "beizhu" };
                    controlsPreName = "tbClient";
                    indexCount = 17;
                    mainID = tbClient1.Text;

                    canPrint = false;
                    DiscardCheckBox.Visible = false;

                    if (ItemId.Equals("-1"))
                    {
                        // 自动生成ID
                        tbClient1.Text = DatabaseConnections.Connector.LocalAutoincreaseID(table, baseName);
                        ItemId = tbClient1.Text;
                    }
                    else
                    {
                        try
                        {
                            FormBasicFeatrues.GetInstence().SetControlsVaule(controlsPreName, detailedPanel, DatabaseConnections.Connector.LocalGetOneRowDataById(table, queryArray, baseName, ItemId));
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
                    checkValidateControls = new List<Control>() { tbGoods1, tbGoods3, tbGoods4, tbGoods5, tbGoods11, tbGoods12, tbGoods15 };
                    detailedHeightDis = 250;
                    detailedPanel = DetailedGoodsPanel;

                    table = "goods";
                    baseName = "goodID";
                    queryArray = new string[] { baseName, "dengji", "name", "guige", "unit", "storageName", "storageManager", "storageManagerPhone", "storageLocation", "storageAddress", "initalCount", "purchasePrice", "purchaseTotal", "currentCount", "currntsalesPrice", "currentTotal", "beizhu" };
                    controlsPreName = "tbGoods";
                    indexCount = 17;
                    mainID = tbGoods1.Text;

                    canPrint = false;
                    DiscardCheckBox.Visible = false;

                    if (ItemId.Equals("-1"))
                    {
                        // 自动生成ID
                        tbGoods1.Text = DatabaseConnections.Connector.LocalAutoincreaseID(table, baseName);
                        ItemId = tbGoods1.Text;
                    }
                    else
                    {
                        // tbGoods11.ReadOnly = true; 初期存量
                        try
                        {
                            FormBasicFeatrues.GetInstence().SetControlsVaule(controlsPreName, detailedPanel, DatabaseConnections.Connector.LocalGetOneRowDataById(table, queryArray, baseName, ItemId));
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
                    checkValidateControls = new List<Control>() { tbDz1, tbDz2, tbDz3 };
                    detailedPanel = DetailedDanziPanel;

                    controlsPreName = "tbDz";
                    detailedPanel = DetailedDanziPanel;
                    indexCount = 6;
                    danziComboBox.Visible = false;
                    DiscardCheckBox.Visible = true;

                    // 添加客户编号
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("clients", "clientID", " ORDER BY id ASC "), tbDz1);
                    if (tbDz1.Items.Count == 0)
                    {
                        MessageBox.Show("请先输入客户信息!", "提示");
                        break;
                    }
                    tbDz1.Items.Insert(0, "");
                    tbDz1.Items.Insert(1, "使用选择器...");

                    // 添加商品编号
                    JCDcbA.Items.Add("");
                    JCDcbB.Items.Add("");
                    JCDcbC.Items.Add("");
                    JCDcbD.Items.Add("");
                    JCDcbE.Items.Add("");
                    JCDcbA.Items.Insert(1, "使用选择器...");
                    JCDcbB.Items.Insert(1, "使用选择器...");
                    JCDcbC.Items.Insert(1, "使用选择器...");
                    JCDcbD.Items.Insert(1, "使用选择器...");
                    JCDcbE.Items.Insert(1, "使用选择器...");
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), JCDcbA);
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), JCDcbB);
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), JCDcbC);
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), JCDcbD);
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), JCDcbE);

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
                        enableDetailedUnderCaigou(false);
                        danziComboBox.Items.Clear();
                        danziComboBox.Items.Add("采购单");
                        danziComboBox.Items.Add("销售单");
                        danziComboBox.SelectedIndex = 0;
                        // moreDetaildpPanel.Visible = true;
                        // FormBasicFeatrues.GetInstence().moveParentPanel(moreDetaildpPanel, detailedPanel);
                    }
                    else
                    {
                        lbDzTitle.Text = "商品（货物）销售单";
                        table = "xsdList";
                        baseName = "xsdID";
                        makeControlsInvisibleForJCCD(true);
                        enableDetailedUnderCaigou(true);
                        danziComboBox.Items.Clear();
                        danziComboBox.Items.Add("采购单");
                        danziComboBox.Items.Add("销售单");
                        danziComboBox.SelectedIndex = 1;
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
                        //tbDz1.SelectedIndex = 0;
                        DiscardCheckBox.Visible = false;

                        // 自动生成ID
                        tbDz2.Text = DatabaseConnections.Connector.LocalAutoincreaseID(table, baseName);
                    }
                    else
                    {
                        try
                        {
                            List<String> queryList = queryArray.ToList();
                            queryList.Remove("companyName");
                            queryList.Remove("jsonData");
                            queryList.Remove("goodsName");
                            queryList.Remove("addtime");
                            queryList.Remove("modifyTime");
                            queryList.Remove("discardFlag");
                            if ((MainWindow.CURRENT_LIST_BUTTON.Text.Equals("进仓单列表")) || (MainWindow.CURRENT_LIST_BUTTON.Text.Equals("出仓单列表")))
                            {
                                queryList.Remove("kxQq");
                                queryList.Remove("kxXq");
                                queryList.Remove("kxJf");
                                queryList.Remove("kxSq");
                                queryList.Remove("kxDay");
                                SaveButton.Visible = false;
                                DiscardCheckBox.Visible = false;
                            }
                            queryArray = queryList.ToArray();

                            FormBasicFeatrues.GetInstence().SetControlsVaule(controlsPreName, detailedPanel, DatabaseConnections.Connector.LocalGetOneRowDataById(table, queryArray, baseName, ItemId));

                            String[] data = DatabaseConnections.Connector.LocalGetOneRowDataById(table, new String[] { "modifyTime", "jsonData", "discardFlag" }, baseName, ItemId);
                            DzDateTextBox.Text = Convert.ToDateTime(data[0]).ToLongDateString();
                            if (int.Parse(data[2]) == 0)
                            {
                                DiscardCheckBox.Checked = false;
                                DiscardCheckBox.Visible = true;
                                DiscardLabel.Visible = false;
                                SaveButton.Visible = true;
                            }
                            else
                            {
                                DiscardCheckBox.Checked = true;
                                DiscardCheckBox.Visible = false;
                                DiscardLabel.Visible = true;
                                SaveButton.Visible = false;
                            }
                            //DiscardCheckBox.Checked = (int.Parse(data[2]) == 0) ? false : true;
                            //DiscardLabel.Visible = (int.Parse(data[2]) == 0) ? false : true;
                            data[1] = data[1].Replace("\n", "");
                            data[1] = data[1].Replace(" ", "");
                            JSONObject json = JSONConvert.DeserializeObject(data[1]);//执行反序列化
                            if (json != null)
                            {
                                if ((JSONObject)json["1"] != null)
                                {
                                    JCDcbA.Text = ((JSONObject)json["1"])["goodsID"].ToString().Equals("") ? "" : ((JSONObject)json["1"])["goodsID"].ToString();
                                    AJCDtb4.Text = ((JSONObject)json["1"])["price"].ToString().Equals("") ? "" : ((JSONObject)json["1"])["price"].ToString();
                                    AJCDtb5.Text = ((JSONObject)json["1"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["1"])["goodsAmount"].ToString();
                                }
                                if ((JSONObject)json["2"] != null)
                                {
                                    JCDcbB.Text = ((JSONObject)json["2"])["goodsID"].Equals("") ? "" : ((JSONObject)json["2"])["goodsID"].ToString();
                                    BJCDtb4.Text = ((JSONObject)json["2"])["price"].Equals("无") ? "" : ((JSONObject)json["2"])["price"].ToString();
                                    BJCDtb5.Text = ((JSONObject)json["2"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["2"])["goodsAmount"].ToString();
                                }
                                if ((JSONObject)json["3"] != null)
                                {
                                    JCDcbC.Text = ((JSONObject)json["3"])["goodsID"].Equals("") ? "" : ((JSONObject)json["3"])["goodsID"].ToString();
                                    CJCDtb4.Text = ((JSONObject)json["3"])["price"].Equals("无") ? "" : ((JSONObject)json["3"])["price"].ToString();
                                    CJCDtb5.Text = ((JSONObject)json["3"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["3"])["goodsAmount"].ToString();
                                }
                                if ((JSONObject)json["4"] != null)
                                {
                                    JCDcbD.Text = ((JSONObject)json["4"])["goodsID"].Equals("") ? "" : ((JSONObject)json["4"])["goodsID"].ToString();
                                    DJCDtb4.Text = ((JSONObject)json["4"])["price"].Equals("无") ? "" : ((JSONObject)json["4"])["price"].ToString();
                                    DJCDtb5.Text = ((JSONObject)json["4"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["4"])["goodsAmount"].ToString();
                                }
                                if ((JSONObject)json["5"] != null)
                                {
                                    JCDcbE.Text = ((JSONObject)json["5"])["goodsID"].Equals("") ? "" : ((JSONObject)json["5"])["goodsID"].ToString();
                                    EJCDtb4.Text = ((JSONObject)json["5"])["price"].Equals("无") ? "" : ((JSONObject)json["5"])["price"].ToString();
                                    EJCDtb5.Text = ((JSONObject)json["5"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["5"])["goodsAmount"].ToString();
                                }
                            }
                            DetailedDanziPanel.Enabled = false;
                            DiscardCheckBox.Enabled = true;
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
                    checkValidateControls = new List<Control>() { tbPz1, tbPz2, SumtbPz };
                    detailedPanel = DetailedPZPanel;
                    detailedHeightDis = 250;

                    table = "pzList";
                    baseName = "pzID";
                    queryArray = new string[] { "clientID", "pzID", "leixing", "companyName", "jsonData", "operateMoney", "remaintingMoney", "beizhu", "discardFlag", "addtime", "modifyTime" };
                    controlsPreName = "tbPz";
                    indexCount = 11;
                    mainID = tbPz2.Text;

                    // 添加客户编号
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("clients", "clientID", " ORDER BY id ASC "), tbPz1);
                    if (tbPz1.Items.Count < 1)
                    {
                        MessageBox.Show("请先输入客户信息!", "提示");
                        break;
                    }

                    tbPz1.Items.Insert(0, "");
                    tbPz1.Items.Insert(1, "使用选择器...");

                    canPrint = true;
                    if (ItemId.Equals("-1"))
                    {
                        pzComboBox.Visible = true;
                        pzComboBox.Items.Clear();
                        pzComboBox.Items.Add("收款凭证");
                        pzComboBox.Items.Add("付款凭证");
                        pzComboBox.Items.Add("领款凭证");
                        pzComboBox.Items.Add("还款凭证");
                        pzComboBox.SelectedIndex = 0;

                        PzDateTextBox.Text = DateTime.Now.ToLongDateString();
                        //tbPz1.SelectedIndex = 0;
                        DiscardCheckBox.Visible = false;

                        // 自动生成ID
                        tbPz2.Text = DatabaseConnections.Connector.LocalAutoincreaseID(table, baseName);
                    }
                    else
                    {
                        pzComboBox.Visible = false;
                        pzComboBox.Items.Clear();
                        pzComboBox.Items.Add("收款凭证");
                        pzComboBox.Items.Add("付款凭证");
                        pzComboBox.Items.Add("领款凭证");
                        pzComboBox.Items.Add("还款凭证");

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

                            FormBasicFeatrues.GetInstence().SetControlsVaule(controlsPreName, detailedPanel, DatabaseConnections.Connector.LocalGetOneRowDataById(table, queryArray, baseName, ItemId));

                            String[] data = DatabaseConnections.Connector.LocalGetOneRowDataById(table, new String[] { "cast(modifyTime as VARCHAR)", "jsonData", "discardFlag", "leixing" }, baseName, ItemId);
                            PzDateTextBox.Text = Convert.ToDateTime(data[0]).ToLongDateString();
                            if (int.Parse(data[2]) == 0)
                            {
                                DiscardCheckBox.Checked = false;
                                DiscardCheckBox.Visible = true;
                                DiscardLabel.Visible = false;
                                SaveButton.Visible = true;
                            }
                            else
                            {
                                DiscardCheckBox.Checked = true;
                                DiscardCheckBox.Visible = false;
                                DiscardLabel.Visible = true;
                                SaveButton.Visible = false;
                            }
                            pzComboBox.SelectedIndex = int.Parse(data[3].ToString());
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
                            // 不可编辑控件
                            DetailedPZPanel.Enabled = false;
                            DiscardCheckBox.Enabled = true;
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
                    checkValidateControls = new List<Control>() { HTtbID, HTtbLocation, tbHTxsfPresenter, tbHTghfPresenter, SumHtTextbox };
                    //checkValidateControls = new List<Control>() { HTtbID, HTtbLocation, tbHTxsfPresenter, tbHTghfPresenter, SumHtTextbox, HTcbChoose2, HTcbChoose3, HTcbChoose4, HTcbChoose5, HTcbChoose6, HTcbChoose7 };
                    //checkValidateControls = new List<Control>() { HTtbID };
                    detailedPanel = DetailedHTPanel;
                    detailedLocationY = 80;
                    detailedHeightDis = 60;

                    //tbHTxsfID.Items.Insert(0, "使用选择器...");
                    //tbHTghfID.Items.Insert(0, "使用选择器...");

                    // 添加商品编号
                    cbHTGoodsNameA.Items.Add("");
                    cbHTGoodsNameB.Items.Add("");
                    cbHTGoodsNameC.Items.Add("");
                    cbHTGoodsNameD.Items.Add("");
                    cbHTGoodsNameE.Items.Add("");
                    cbHTGoodsNameA.Items.Insert(1, "使用选择器...");
                    cbHTGoodsNameB.Items.Insert(1, "使用选择器...");
                    cbHTGoodsNameC.Items.Insert(1, "使用选择器...");
                    cbHTGoodsNameD.Items.Insert(1, "使用选择器...");
                    cbHTGoodsNameE.Items.Insert(1, "使用选择器...");
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), cbHTGoodsNameA);
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), cbHTGoodsNameB);
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), cbHTGoodsNameC);
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), cbHTGoodsNameD);
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("goods", "goodID", " ORDER BY id ASC "), cbHTGoodsNameE);

                    table = "htList";
                    baseName = "htID";
                    queryArray = new string[] { "htID", "leixing", "htDate", "clientID", "companyName", "jsonData", "sum", "discardFlag", "addtime", "modifyTime" };
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

                        HTtbDate.Text = DateTime.Now.ToLongDateString();

                        // 自动生成ID
                        HTtbID.Text = DatabaseConnections.Connector.LocalAutoincreaseID(table, baseName);
                    }
                    else
                    {
                        HTcbName.Enabled = false;
                        try
                        {
                            String[] data = DatabaseConnections.Connector.LocalGetOneRowDataById(table, new String[] { "modifyTime", "jsonData", "leixing", "htID", "htDate", "clientID", "discardFlag", "option", "htLocation" }, baseName, ItemId);
                            DzDateTextBox.Text = Convert.ToDateTime(data[0]).ToLongDateString();
                            HTcbName.SelectedIndex = int.Parse(data[2].ToString());
                            HTtbID.Text = data[3].ToString();
                            HTtbLocation.Text = data[8].ToString();
                            HTtbDate.Text = Convert.ToDateTime(data[4]).ToLongDateString();
                            if (HTcbName.SelectedIndex == 1) //购买合同
                            {
                                tbHTghfID.Text = data[5].ToString();
                                // 自己公司
                                tbHTxsfAddress.Text = MainWindow.ADDRESS;
                                tbHTxsfPresenter.Text = MainWindow.COMPANY_OWNER;
                                tbHTxsfFax.Text = MainWindow.FAX;
                                tbHTxsfPhone.Text = MainWindow.PHONE;
                                tbHTxsfEmail.Text = MainWindow.EMAIL;
                                tbHTxsfBankName.Text = MainWindow.BANK_NAME;
                                tbHTxsfBankNumber.Text = MainWindow.BANK_CARD;
                                tbHTghfID.Enabled = true;
                                tbHTxsfID.Enabled = false;
                                tbHTxsfID.Items.Clear();
                            }
                            else
                            {
                                tbHTxsfID.Text = data[5].ToString();
                                // 自己公司
                                tbHTghfName.Text = MainWindow.COMPANY_NAME;
                                tbHTghfAddress.Text = MainWindow.ADDRESS;
                                tbHTghfPresenter.Text = MainWindow.COMPANY_OWNER;
                                tbHTghfFax.Text = MainWindow.FAX;
                                tbHTghfPhone.Text = MainWindow.PHONE;
                                tbHTghfEmail.Text = MainWindow.EMAIL;
                                tbHTghfBankName.Text = MainWindow.BANK_NAME;
                                tbHTghfBankNumber.Text = MainWindow.BANK_CARD;
                                tbHTghfID.Enabled = false;
                                tbHTxsfID.Enabled = true;
                                tbHTghfID.Items.Clear();
                            }

                            DiscardCheckBox.Checked = data[6].Equals("0") ? false : true;

                            // 选项
                            String[] optionArray = data[7].Split(',');
                            HTcbChoose2.SelectedIndex = int.Parse(optionArray[0]);
                            HTcbChoose3.SelectedIndex = int.Parse(optionArray[1]);
                            HTcbChoose4.SelectedIndex = int.Parse(optionArray[2]);
                            HTcbChoose5.SelectedIndex = int.Parse(optionArray[3]);
                            HTcbChoose6.SelectedIndex = int.Parse(optionArray[4]);
                            HTcbChoose7.SelectedIndex = int.Parse(optionArray[5]);

                            data[1] = data[1].Replace("\n", "");
                            data[1] = data[1].Replace(" ", "");
                            JSONObject json = JSONConvert.DeserializeObject(data[1]);//执行反序列化 
                            if (json != null)
                            {
                                if ((JSONObject)json["1"] != null)
                                {
                                    cbHTGoodsNameA.Text = ((JSONObject)json["1"])["goodsID"].ToString().Equals("") ? "" : ((JSONObject)json["1"])["goodsID"].ToString();
                                    cbHTGoodsA5.Text = ((JSONObject)json["1"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["1"])["goodsAmount"].ToString();
                                    cbHTGoodsA7.Text = ((JSONObject)json["1"])["goodsLocation"].Equals("无") ? "" : ((JSONObject)json["1"])["goodsLocation"].ToString();
                                }
                                if ((JSONObject)json["2"] != null)
                                {
                                    cbHTGoodsNameB.Text = ((JSONObject)json["2"])["goodsID"].ToString().Equals("") ? "" : ((JSONObject)json["2"])["goodsID"].ToString();
                                    cbHTGoodsB5.Text = ((JSONObject)json["2"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["2"])["goodsAmount"].ToString();
                                    cbHTGoodsB7.Text = ((JSONObject)json["2"])["goodsLocation"].Equals("无") ? "" : ((JSONObject)json["2"])["goodsLocation"].ToString();
                                }
                                if ((JSONObject)json["3"] != null)
                                {
                                    cbHTGoodsNameC.Text = ((JSONObject)json["3"])["goodsID"].ToString().Equals("") ? "" : ((JSONObject)json["3"])["goodsID"].ToString();
                                    cbHTGoodsC5.Text = ((JSONObject)json["3"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["3"])["goodsAmount"].ToString();
                                    cbHTGoodsC7.Text = ((JSONObject)json["3"])["goodsLocation"].Equals("无") ? "" : ((JSONObject)json["3"])["goodsLocation"].ToString();
                                }
                                if ((JSONObject)json["4"] != null)
                                {
                                    cbHTGoodsNameD.Text = ((JSONObject)json["4"])["goodsID"].ToString().Equals("") ? "" : ((JSONObject)json["4"])["goodsID"].ToString();
                                    cbHTGoodsD5.Text = ((JSONObject)json["4"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["4"])["goodsAmount"].ToString();
                                    cbHTGoodsD7.Text = ((JSONObject)json["4"])["goodsLocation"].Equals("无") ? "" : ((JSONObject)json["4"])["goodsLocation"].ToString();
                                }
                                if ((JSONObject)json["5"] != null)
                                {
                                    cbHTGoodsNameE.Text = ((JSONObject)json["5"])["goodsID"].ToString().Equals("") ? "" : ((JSONObject)json["5"])["goodsID"].ToString();
                                    cbHTGoodsE5.Text = ((JSONObject)json["5"])["goodsAmount"].Equals("无") ? "" : ((JSONObject)json["5"])["goodsAmount"].ToString();
                                    cbHTGoodsE7.Text = ((JSONObject)json["5"])["goodsLocation"].Equals("无") ? "" : ((JSONObject)json["5"])["goodsLocation"].ToString();
                                }
                            }
                            // 不可编辑控件
                            DetailedHTPanel.Enabled = false;
                            DiscardCheckBox.Enabled = true;
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
            DiscardLabel.Location = new Point(DiscardLabel.Location.X, DiscardLabel.Location.Y - detailedHeightDis);
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
            tbDz8.Visible = isVisable;
            tbDz9.Visible = isVisable;
            tbDz10.Visible = isVisable;
            tbDz11.Visible = isVisable;
            tbDz12.Visible = isVisable;
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

        // 保存按钮
        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormBasicFeatrues.GetInstence().isPassValidateControls(checkValidateControls))
                {
                    if ((MainWindow.CURRENT_TAB == 3) || (MainWindow.CURRENT_TAB == 4)) // 进仓单 出仓单
                    {
                        String jsonData = ControlValueTransitToJson(
                            new List<String>() { "goodsID", "price", "goodsAmount" },
                            new List<List<Control>>() {
                                new List<Control> (){JCDcbA, AJCDtb4,AJCDtb5 } ,
                                new List<Control> (){JCDcbB, BJCDtb4,BJCDtb5 } ,
                                new List<Control> (){JCDcbC, CJCDtb4,CJCDtb5 } ,
                                new List<Control> (){JCDcbD, DJCDtb4,DJCDtb5 } ,
                                new List<Control> (){JCDcbE, EJCDtb4,EJCDtb5 } 
                                }
                            );

                        // 
                        String[] queryStringArray;
                        String[] resultStringArray;
                        if (ItemId.Equals("-1")) // 新建
                        {
                            //if ((MainWindow.CURRENT_LIST_BUTTON.Text.Equals("进仓单列表")) || (MainWindow.CURRENT_LIST_BUTTON.Text.Equals("出仓单列表")))
                            //{
                            //    queryStringArray = new String[] { "clientID", baseName, "companyName", "goodsName", "jsonData", "sum", "beizhu", "fpPu", "fpZeng", "fpCount", "discardFlag", "addtime", "modifyTime" };
                            //    resultStringArray = new String[] { 
                            //    tbDz1.Text,tbDz2.Text, dzCompany.Text, 
                            //    AJCDtb0.Text+","+BJCDtb0.Text+","+CJCDtb0.Text+","+DJCDtb0.Text+","+EJCDtb0.Text,
                            //    jsonData, 
                            //    tbDz3.Text.Split('￥')[1], tbDz4.Text,tbDz5.Text, tbDz6.Text, tbDz7.Text,(DiscardCheckBox.Checked?"1":"0"),DateTime.Now.ToString(),   DateTime.Now.ToString()};
                            //}
                            //else
                            //{

                            #region 保存到进出仓单
                            String tableDZ = "";
                            String baseNameDZ = "";
                            String operatorDZ = "";

                            if (danziComboBox.SelectedIndex == 0) // 采购单 - 进仓单
                            {
                                tableDZ = "jcdList";
                                baseNameDZ = "jcdID";
                                operatorDZ = "+";
                            }
                            else
                            { // 销售单 - 出仓单
                                tableDZ = "ccdList";
                                baseNameDZ = "ccdID";
                                operatorDZ = "-";
                                if (int.Parse(AJCDtb5.Text.Equals("") ? "0" : AJCDtb5.Text) > int.Parse((AJCDtb5.EmptyTextTip.Equals("") ? "0:0" : AJCDtb5.EmptyTextTip).Split(':')[1])
                                    || int.Parse(BJCDtb5.Text.Equals("") ? "0" : BJCDtb5.Text) > int.Parse((BJCDtb5.EmptyTextTip.Equals("") ? "0:0" : BJCDtb5.EmptyTextTip).Split(':')[1])
                                    || int.Parse(CJCDtb5.Text.Equals("") ? "0" : CJCDtb5.Text) > int.Parse((CJCDtb5.EmptyTextTip.Equals("") ? "0:0" : CJCDtb5.EmptyTextTip).Split(':')[1])
                                    || int.Parse(DJCDtb5.Text.Equals("") ? "0" : DJCDtb5.Text) > int.Parse((DJCDtb5.EmptyTextTip.Equals("") ? "0:0" : DJCDtb5.EmptyTextTip).Split(':')[1])
                                    || int.Parse(EJCDtb5.Text.Equals("") ? "0" : EJCDtb5.Text) > int.Parse((EJCDtb5.EmptyTextTip.Equals("") ? "0:0" : EJCDtb5.EmptyTextTip).Split(':')[1]))
                                {
                                    MessageBox.Show("此商品库存数量不足！请修改数量！", "错误");
                                    return;
                                }
                            }

                            String[] queryStringArrayDZ = new String[] { "clientID", baseNameDZ, "cgxsID", "companyName", "goodsName", "jsonData", "sum", "beizhu", "fpPu", "fpZeng", "fpCount", "discardFlag", "addtime", "modifyTime" };
                            String[] resultStringArrayDZ = new String[] { 
                                tbDz1.Text, DatabaseConnections.Connector.LocalAutoincreaseID(tableDZ, baseNameDZ), tbDz2.Text, dzCompany.Text, 
                                AJCDtb0.Text+" "+BJCDtb0.Text+" "+CJCDtb0.Text+" "+DJCDtb0.Text+" "+EJCDtb0.Text,
                                jsonData, 
                                tbDz3.Text.Split('￥')[1], tbDz4.Text,tbDz5.Text, tbDz6.Text, tbDz7.Text,(DiscardCheckBox.Checked?"1":"0"),DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),   DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")};

                            DatabaseConnections.Connector.LocalReplaceIntoData(tableDZ, queryStringArrayDZ, resultStringArrayDZ, mainID);
                            #endregion

                            #region 更新到库存信息
                            if (!JCDcbA.Text.Equals("") && !AJCDtb5.Text.Equals(""))
                            {
                                DatabaseConnections.Connector.LocalUpdateData("goods", new String[] { "currentCount" }, new String[] { "currentCount" + operatorDZ + AJCDtb5.Text }, false, "goodID", JCDcbA.Text);
                            }
                            if (!JCDcbB.Text.Equals("") && !BJCDtb5.Text.Equals(""))
                            {
                                DatabaseConnections.Connector.LocalUpdateData("goods", new String[] { "currentCount" }, new String[] { "currentCount" + operatorDZ + BJCDtb5.Text }, false, "goodID", JCDcbB.Text);
                            }
                            if (!JCDcbC.Text.Equals("") && !CJCDtb5.Text.Equals(""))
                            {
                                DatabaseConnections.Connector.LocalUpdateData("goods", new String[] { "currentCount" }, new String[] { "currentCount" + operatorDZ + CJCDtb5.Text }, false, "goodID", JCDcbC.Text);
                            }
                            if (!JCDcbD.Text.Equals("") && !DJCDtb5.Text.Equals(""))
                            {
                                DatabaseConnections.Connector.LocalUpdateData("goods", new String[] { "currentCount" }, new String[] { "currentCount" + operatorDZ + DJCDtb5.Text }, false, "goodID", JCDcbD.Text);
                            }
                            if (!JCDcbE.Text.Equals("") && !EJCDtb5.Text.Equals(""))
                            {
                                DatabaseConnections.Connector.LocalUpdateData("goods", new String[] { "currentCount" }, new String[] { "currentCount" + operatorDZ + EJCDtb5.Text }, false, "goodID", JCDcbE.Text);
                            }
                            #endregion

                            #region 更新客户应收应付
                            if (danziComboBox.SelectedIndex == 0) //采购
                            {
                                DatabaseConnections.Connector.LocalUpdateData("clients", new String[] { "shouldPay" }, new String[] { "shouldPay+" + tbDz3.Text.Split('￥')[1] }, false, "clientID", tbDz1.Text);
                            }
                            else if (danziComboBox.SelectedIndex == 1) //销售
                            {
                                DatabaseConnections.Connector.LocalUpdateData("clients", new String[] { "shouldReceive" }, new String[] { "shouldReceive+" + tbDz3.Text.Split('￥')[1] }, false, "clientID", tbDz1.Text);
                            }
                            #endregion

                            queryStringArray = new String[] { "clientID", baseName, "companyName", "goodsName", "jsonData", "sum", "beizhu", "fpPu", "fpZeng", "fpCount", "discardFlag", "addtime", "modifyTime", "kxQq", "kxXq", "kxJf", "kxSq", "kxDay" };
                            resultStringArray = new String[] { 
                                tbDz1.Text,tbDz2.Text, dzCompany.Text, 
                                AJCDtb0.Text+" "+BJCDtb0.Text+" "+CJCDtb0.Text+" "+DJCDtb0.Text+" "+EJCDtb0.Text,
                                jsonData, 
                                tbDz3.Text.Split('￥')[1], tbDz4.Text,tbDz5.Text, tbDz6.Text, tbDz7.Text,(DiscardCheckBox.Checked?"1":"0"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), tbDz8.Text,tbDz9.Text, tbDz10.Text,tbDz11.Text,tbDz12.Text};
                            //}
                        }
                        else
                        {
                            if ((MainWindow.CURRENT_LIST_BUTTON.Text.Equals("进仓单列表")) || (MainWindow.CURRENT_LIST_BUTTON.Text.Equals("出仓单列表")))
                            {
                                queryStringArray = new String[] { "clientID", baseName, "companyName", "goodsName", "jsonData", "sum", "beizhu", "fpPu", "fpZeng", "fpCount", "discardFlag", "modifyTime" };
                                resultStringArray = new String[] { 
                                tbDz1.Text,tbDz2.Text, dzCompany.Text, 
                                AJCDtb0.Text+","+BJCDtb0.Text+","+CJCDtb0.Text+","+DJCDtb0.Text+","+EJCDtb0.Text,
                                jsonData, 
                                tbDz3.Text.Split('￥')[1], tbDz4.Text,tbDz5.Text, tbDz6.Text, tbDz7.Text,(DiscardCheckBox.Checked?"1":"0"),   DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")};
                                //queryStringArray = new String[] { baseName, "discardFlag", "modifyTime" };
                                //resultStringArray = new String[] { tbDz2.Text, (DiscardCheckBox.Checked ? "1" : "0"), DateTime.Now.ToString() };
                            }
                            else
                            { // 采购销售单
                                queryStringArray = new String[] { "clientID", baseName, "companyName", "goodsName", "jsonData", "sum", "beizhu", "fpPu", "fpZeng", "fpCount", "discardFlag", "modifyTime", "kxQq", "kxXq", "kxJf", "kxSq", "kxDay" };
                                resultStringArray = new String[] { 
                               tbDz1.Text,tbDz2.Text, dzCompany.Text, 
                               AJCDtb0.Text+","+BJCDtb0.Text+","+CJCDtb0.Text+","+DJCDtb0.Text+","+EJCDtb0.Text,
                               jsonData, 
                               tbDz3.Text.Split('￥')[1], tbDz4.Text,tbDz5.Text, tbDz6.Text, tbDz7.Text,(DiscardCheckBox.Checked?"1":"0"),   DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), tbDz8.Text,tbDz9.Text, tbDz10.Text,tbDz11.Text,tbDz12.Text};
                                //queryStringArray = new String[] { baseName, "discardFlag", "modifyTime" };
                                //resultStringArray = new String[] { tbDz2.Text, (DiscardCheckBox.Checked ? "1" : "0"), DateTime.Now.ToString() };
                            }

                            if (DiscardCheckBox.Checked)
                            {
                                #region 作废标记 保存到进出仓单
                                String tableDZModi = "";
                                String baseNameDZModi = "cgxsID";
                                String operatorDZModi = "";

                                if (danziComboBox.SelectedIndex == 0) // 采购单 - 进仓单
                                {
                                    tableDZModi = "jcdList";
                                    operatorDZModi = "-";
                                }
                                else
                                { // tableDZModi - 出仓单
                                    tableDZModi = "ccdList";
                                    operatorDZModi = "+";
                                }

                                String[] queryStringArrayDZModi = new String[] { "discardFlag", "modifyTime" };
                                String[] resultStringArrayDZModi = new String[] { (DiscardCheckBox.Checked ? "1" : "0"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };

                                DatabaseConnections.Connector.LocalUpdateData(tableDZModi, queryStringArrayDZModi, resultStringArrayDZModi, true, baseNameDZModi, tbDz2.Text);
                                #endregion

                                #region 作废标记 更新到库存信息
                                if (!JCDcbA.Text.Equals("") && !AJCDtb5.Text.Equals(""))
                                {
                                    DatabaseConnections.Connector.LocalUpdateData("goods", new String[] { "currentCount" }, new String[] { "currentCount" + operatorDZModi + AJCDtb5.Text }, false, "goodID", JCDcbA.Text);
                                }
                                if (!JCDcbB.Text.Equals("") && !BJCDtb5.Text.Equals(""))
                                {
                                    DatabaseConnections.Connector.LocalUpdateData("goods", new String[] { "currentCount" }, new String[] { "currentCount" + operatorDZModi + BJCDtb5.Text }, false, "goodID", JCDcbB.Text);
                                }
                                if (!JCDcbC.Text.Equals("") && !CJCDtb5.Text.Equals(""))
                                {
                                    DatabaseConnections.Connector.LocalUpdateData("goods", new String[] { "currentCount" }, new String[] { "currentCount" + operatorDZModi + CJCDtb5.Text }, false, "goodID", JCDcbC.Text);
                                }
                                if (!JCDcbD.Text.Equals("") && !DJCDtb5.Text.Equals(""))
                                {
                                    DatabaseConnections.Connector.LocalUpdateData("goods", new String[] { "currentCount" }, new String[] { "currentCount" + operatorDZModi + DJCDtb5.Text }, false, "goodID", JCDcbD.Text);
                                }
                                if (!JCDcbE.Text.Equals("") && !EJCDtb5.Text.Equals(""))
                                {
                                    DatabaseConnections.Connector.LocalUpdateData("goods", new String[] { "currentCount" }, new String[] { "currentCount" + operatorDZModi + EJCDtb5.Text }, false, "goodID", JCDcbE.Text);
                                }
                                #endregion
                            }
                        }

                        DatabaseConnections.Connector.LocalReplaceIntoData(table, queryStringArray, resultStringArray, mainID);
                    }
                    else if (MainWindow.CURRENT_TAB == 5)  // 凭证
                    {
                        /*
                         收款凭证
                         付款凭证
                         领款凭证
                         还款凭证
                         */

                        String jsonData = ControlValueTransitToJson(
                                                   new List<String>() { "zhaiyao", "operateMoney", "payWay", "payNumber", "payCount" },
                                                   new List<List<Control>>() {
                                new List<Control> (){PzcbA,APztb0,APztb1,APztb2,APztb3 } ,
                                new List<Control> (){PzcbB,BPztb0,BPztb1,BPztb2,BPztb3 } ,
                                new List<Control> (){PzcbC,CPztb0,CPztb1,CPztb2,CPztb3 } ,
                                new List<Control> (){PzcbD,DPztb0,DPztb1,DPztb2,DPztb3 } ,
                                new List<Control> (){PzcbE,EPztb0,EPztb1,EPztb2,EPztb3 } 
                                });
                        // "pzID", "leixing", "clientID", "companyName", "jsonData", "operateMoney", "remaintingMoney", "discardFlag", "addtime", "modifyTime"
                        String[] queryStringArray;
                        String[] resultStringArray;
                        if (ItemId.Equals("-1"))
                        {
                            queryStringArray = new string[] { "clientID", "pzID", "zhaiyao", "leixing", "companyName", "jsonData", "operateMoney", "remaintingMoney", "beizhu", "discardFlag", "addtime", "modifyTime" };
                            resultStringArray = new String[] {  
                                tbPz1.Text, 
                                tbPz2.Text, 
                                PzcbA.Text+" "+PzcbB.Text+" "+PzcbC.Text+" "+PzcbD.Text+" "+PzcbE.Text,
                                pzComboBox.SelectedIndex.ToString(),  
                                pzCompany.Text,
                                jsonData, SumtbPz.Text.Split('￥')[1],
                                "",  tbPz3.Text,  (DiscardCheckBox.Checked?"1":"0"),DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")};

                            if (pzComboBox.SelectedIndex == 0) //收款
                            {
                                DatabaseConnections.Connector.LocalUpdateData("clients", new String[] { "shouldReceive" }, new String[] { "shouldReceive-" + SumtbPz.Text.Split('￥')[1] }, false, "clientID", tbPz1.Text);
                            }
                            else if (pzComboBox.SelectedIndex == 1) //付款
                            {
                                DatabaseConnections.Connector.LocalUpdateData("clients", new String[] { "shouldPay" }, new String[] { "shouldPay-" + SumtbPz.Text.Split('￥')[1] }, false, "clientID", tbPz1.Text);
                            }
                        }
                        else
                        {
                            queryStringArray = new string[] { "clientID", "pzID", "zhaiyao", "leixing", "companyName", "jsonData", "operateMoney", "remaintingMoney", "beizhu", "discardFlag", "modifyTime" };
                            resultStringArray = new String[] {  
                                tbPz1.Text, 
                                tbPz2.Text, 
                                PzcbA.Text+" "+PzcbB.Text+" "+PzcbC.Text+" "+PzcbD.Text+" "+PzcbE.Text,
                                pzComboBox.SelectedIndex.ToString(),  
                                pzCompany.Text,
                                jsonData, SumtbPz.Text.Split('￥')[1],
                                "",  tbPz3.Text,  (DiscardCheckBox.Checked?"1":"0"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")};
                            //queryStringArray = new String[] { "pzID", "discardFlag", "modifyTime" };
                            //resultStringArray = new String[] { tbPz2.Text, (DiscardCheckBox.Checked ? "1" : "0"), DateTime.Now.ToString() };
                        }
                        DatabaseConnections.Connector.LocalReplaceIntoData(table, queryStringArray, resultStringArray, tbPz2.Text);
                    }
                    else if (MainWindow.CURRENT_TAB == 6)  // 合同
                    {
                        String jsonData = ControlValueTransitToJson(
                                                       new List<String>() { "goodsID", "goodsAmount", "goodsLocation" },
                                                       new List<List<Control>>() {
                                new List<Control> (){cbHTGoodsNameA,cbHTGoodsA5,cbHTGoodsA7 } ,
                                new List<Control> (){cbHTGoodsNameB,cbHTGoodsB5,cbHTGoodsB7 } ,
                                new List<Control> (){cbHTGoodsNameC,cbHTGoodsC5,cbHTGoodsC7 } ,
                                new List<Control> (){cbHTGoodsNameD,cbHTGoodsD5,cbHTGoodsD7 } ,
                                new List<Control> (){cbHTGoodsNameE,cbHTGoodsE5,cbHTGoodsE7 } 
                                }
                                                       );
                        // "htID", "leixing", "htDate", "companyName", "jsonData", "sum", "discardFlag", "addtime", "modtifyTime"

                        String[] queryStringArray;
                        String[] resultStringArray;
                        if (ItemId.Equals("-1"))
                        {
                            queryStringArray = new string[] { "htID", "leixing", "htDate", "htLocation", "clientID", "companyName", "jsonData", "sum", "discardFlag", "addtime", "modifyTime", "option" };
                            resultStringArray = new String[] {  
                                HTtbID.Text, 
                                HTcbName.SelectedIndex.ToString(),  
                                HTtbDate.Text, 
                                HTtbLocation.Text,
                               (HTcbName.SelectedIndex == 0) ?tbHTxsfID.Text:tbHTghfID.Text,  
                               (HTcbName.SelectedIndex == 0) ?tbHTxsfName.Text:tbHTghfName.Text,
                                jsonData,
                                SumHtTextbox.Text.Split('￥')[1], 
                                (DiscardCheckBox.Checked?"1":"0"), 
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),  
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                HTcbChoose2.SelectedIndex+","+HTcbChoose3.SelectedIndex+","+HTcbChoose4.SelectedIndex+","+HTcbChoose5.SelectedIndex+","+HTcbChoose6.SelectedIndex+","+HTcbChoose7.SelectedIndex};
                            // 保存法人信息
                            if (HTcbName.SelectedIndex == 0)
                            {
                                DatabaseConnections.Connector.LocalUpdateData("clients", new String[] { "companyOwner" }, new String[] { tbHTghfPresenter.Text }, true, "clientID", tbHTghfID.Text);
                            }
                            else
                            {
                                DatabaseConnections.Connector.LocalUpdateData("clients", new String[] { "companyOwner" }, new String[] { tbHTxsfPresenter.Text }, true, "clientID", tbHTxsfID.Text);
                            }
                        }
                        else
                        {
                            queryStringArray = new string[] { "htID", "leixing", "htDate", "clientID", "companyName", "jsonData", "sum", "discardFlag", "modifyTime", "option" };
                            resultStringArray = new String[] {  HTtbID.Text, HTcbName.SelectedIndex.ToString(),  HTtbDate.Text, 
                                (HTcbName.SelectedIndex == 0) ?tbHTghfID.Text:tbHTxsfID.Text,  (HTcbName.SelectedIndex == 0) ?tbHTghfName.Text:tbHTxsfName.Text,
                                jsonData,
                                SumHtTextbox.Text.Split('￥')[1], (DiscardCheckBox.Checked?"1":"0"),  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),HTcbChoose2.SelectedIndex+","+HTcbChoose3.SelectedIndex+","+HTcbChoose4.SelectedIndex+","+HTcbChoose5.SelectedIndex+","+HTcbChoose6.SelectedIndex+","+HTcbChoose7.SelectedIndex};
                            //queryStringArray = new String[] { "htID", "discardFlag", "modifyTime" };
                            //resultStringArray = new String[] { HTtbID.Text, (DiscardCheckBox.Checked ? "1" : "0"), DateTime.Now.ToString() };
                        }
                        DatabaseConnections.Connector.LocalReplaceIntoData(table, queryStringArray, resultStringArray, mainID);
                    }
                    else
                    {
                        //DatabaseConnections.GetInstence().LocalReplaceIntoData(table, queryArray, FormBasicFeatrues.GetInstence().GetControlsVaule(controlsPreName, detailedPanel, indexCount), mainID);
                        if (MainWindow.CURRENT_TAB == 1)
                        {
                            queryArray = new string[] { baseName, "gzbID", "type", "company", "companyOwner", "address", "phone", "fax", "QQ", "taxNumber", "email", "bankName", "bankCard", "PrivateAccount", "shouldReceive", "shouldPay", "beizhu" };
                        }
                        else if (MainWindow.CURRENT_TAB == 2)
                        {
                            queryArray = new string[] { baseName, "dengji", "name", "guige", "unit", "storageName", "storageManager", "storageManagerPhone", "storageLocation", "storageAddress", "initalCount", "purchasePrice", "purchaseTotal", "currentCount", "currntsalesPrice", "currentTotal", "beizhu" };
                        }
                        List<String> queryArrayList = queryArray.ToList();
                        List<String> resultListString = FormBasicFeatrues.GetInstence().GetControlsVaule(controlsPreName, detailedPanel, indexCount).ToList(); ;
                        if (isNewWindow)
                        {
                            queryArrayList.Add("addtime");
                            queryArrayList.Add("modifyTime");

                            resultListString.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            resultListString.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            queryArrayList.Add("addtime");
                            resultListString.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        DatabaseConnections.Connector.LocalReplaceIntoData(table, queryArrayList.ToArray(), resultListString.ToArray(), mainID);
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

        // 打印预览按钮
        private void PreviewPrintButton_Click(object sender, EventArgs e)
        {
            int printFlag = 0;
            int pageHeight = 1200;
            switch (MainWindow.CURRENT_TAB)
            {
                default:
                    break;
                case 1: // 客户管理 无打印
                    break;
                case 2: // 商品管理 无打印
                    break;
                case 3: // 存储管理 进仓单 出仓单
                    printFlag = 1;
                    pageHeight = 560;
                    break;
                case 4: // 业务管理 采购单 销售单 客户对账单
                    printFlag = 2;
                    pageHeight = 560;
                    break;
                case 5:  // 财务管理 凭证 收付汇总表
                    printFlag = 5;
                    pageHeight = 560;
                    break;
                case 6: // 合同
                    printFlag = 7;
                    pageHeight = 1200;
                    break;
            }
            SetPrintPreview(printFlag, pageHeight);
            //SetPrintPreview(MainWindow.CURRENT_TAB);
        }

        //为生成新行添加值
        private void DetailedDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {

        }

        // 隐藏采购单下面内容
        private void enableDetailedUnderCaigou(Boolean isEnabled)
        {
            label46.Enabled = isEnabled;
            label47.Enabled = isEnabled;
            label48.Enabled = isEnabled;
            label49.Enabled = isEnabled;
            label50.Enabled = isEnabled;
            tbDz8.Enabled = isEnabled;
            tbDz9.Enabled = isEnabled;
            tbDz10.Enabled = isEnabled;
            tbDz11.Enabled = isEnabled;
            tbDz12.Enabled = isEnabled;
            if (!isEnabled)
            {
                tbDz8.Text = "";
                tbDz9.Text = "";
                tbDz10.Text = "";
                tbDz11.Text = "";
            }
        }

        // 进仓单 出仓单 采购单 销售单
        private void danziComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormBasicFeatrues.GetInstence().reTriggleCombox(JCDcbA);
            FormBasicFeatrues.GetInstence().reTriggleCombox(JCDcbB);
            FormBasicFeatrues.GetInstence().reTriggleCombox(JCDcbC);
            FormBasicFeatrues.GetInstence().reTriggleCombox(JCDcbD);
            FormBasicFeatrues.GetInstence().reTriggleCombox(JCDcbE);

            FormBasicFeatrues.GetInstence().reTriggleCombox(tbDz1);

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
                        tbDz2.Text = DatabaseConnections.Connector.LocalAutoincreaseID(table, baseName);
                        enableDetailedUnderCaigou(false);
                        tbDz12.Text = "";
                        tbDz10.Text = "";
                        break;
                    case 1://销售单
                        lbDzTitle.Text = "商品（货物）销售单";
                        table = "xsdList";
                        baseName = "xsdID";
                        queryArray = new string[] { "clientID", baseName, "companyName", "goodsName", "jsonData", "sum", "beizhu", "fpPu", "fpZeng", "fpCount", "discardFlag", "addtime", "modifyTime", "kxQq", "kxXq", "kxJf", "kxSq", "kxDay" };
                        // 自动生成ID
                        tbDz2.Text = DatabaseConnections.Connector.LocalAutoincreaseID(table, baseName);
                        enableDetailedUnderCaigou(true);
                        tbDz12.Text = "7";
                        tbDz10.Text = "0";
                        break;
                }
            }
        }

        // 凭证管理
        private void pzComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            table = "pzList";
            baseName = "pzID";
            queryArray = new string[] { "clientID", "pzID", "leixing", "companyName", "jsonData", "operateMoney", "remaintingMoney", "beizhu", "discardFlag", "addtime", "modifyTime" };
            controlsPreName = "tbPz";
            if (ItemId.Equals("-1"))
            {
                // 自动生成ID
                tbPz2.Text = DatabaseConnections.Connector.LocalAutoincreaseID(table, baseName);
            }
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
            }
        }

        // 合同
        private void HTcbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            table = "htList";
            baseName = "htID";
            queryArray = new string[] { "htID", "leixing", "htDate", "companyName", "jsonData", "sum", "discardFlag", "addtime", "modtifyTime" };
            controlsPreName = "HTtbID";

            FormBasicFeatrues.GetInstence().reTriggleCombox(cbHTGoodsNameA);
            FormBasicFeatrues.GetInstence().reTriggleCombox(cbHTGoodsNameB);
            FormBasicFeatrues.GetInstence().reTriggleCombox(cbHTGoodsNameC);
            FormBasicFeatrues.GetInstence().reTriggleCombox(cbHTGoodsNameD);
            FormBasicFeatrues.GetInstence().reTriggleCombox(cbHTGoodsNameE);

            // 自动生成ID
            HTtbID.Text = DatabaseConnections.Connector.LocalAutoincreaseID(table, baseName);
            switch (HTcbName.SelectedIndex)
            {
                default:
                    break;
                case 0:// 购买
                    tbHTghfName.Text = MainWindow.COMPANY_NAME;
                    tbHTghfAddress.Text = MainWindow.ADDRESS;
                    tbHTghfPresenter.Text = MainWindow.COMPANY_OWNER;
                    tbHTghfFax.Text = MainWindow.FAX;
                    tbHTghfPhone.Text = MainWindow.PHONE;
                    tbHTghfEmail.Text = MainWindow.EMAIL;
                    tbHTghfBankName.Text = MainWindow.BANK_NAME;
                    tbHTghfBankNumber.Text = MainWindow.BANK_CARD;
                    tbHTghfID.Enabled = false;
                    tbHTxsfID.Enabled = true;
                    tbHTghfID.Items.Clear();

                    // 添加客户编号
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("clients", "clientID", " ORDER BY id ASC "), tbHTxsfID);
                    tbHTxsfID.Items.Insert(0, "");
                    tbHTxsfID.Items.Insert(1, "使用选择器...");
                    //tbHTxsfID.SelectedIndex = 0;
                    tbHTxsfName.Text = "";
                    tbHTxsfAddress.Text = "";
                    tbHTxsfPresenter.Text = "";
                    tbHTxsfFax.Text = "";
                    tbHTxsfPhone.Text = "";
                    tbHTxsfEmail.Text = "";
                    tbHTxsfBankName.Text = "";
                    tbHTxsfBankNumber.Text = "";
                    break;
                case 1:// 销售
                    tbHTxsfName.Text = MainWindow.COMPANY_NAME;
                    tbHTxsfAddress.Text = MainWindow.ADDRESS;
                    tbHTxsfPresenter.Text = MainWindow.COMPANY_OWNER;
                    tbHTxsfFax.Text = MainWindow.FAX;
                    tbHTxsfPhone.Text = MainWindow.PHONE;
                    tbHTxsfEmail.Text = MainWindow.EMAIL;
                    tbHTxsfBankName.Text = MainWindow.BANK_NAME;
                    tbHTxsfBankNumber.Text = MainWindow.BANK_CARD;
                    tbHTghfID.Enabled = true;
                    tbHTxsfID.Enabled = false;
                    tbHTxsfID.Items.Clear();

                    // 添加客户编号
                    addItemsToCombox(DatabaseConnections.Connector.LocalGetIdsOfTable("clients", "clientID", " ORDER BY id ASC "), tbHTghfID);
                    tbHTghfID.Items.Insert(0, "");
                    tbHTghfID.Items.Insert(1, "使用选择器...");
                    //tbHTghfID.SelectedIndex = 0;
                    tbHTghfName.Text = "";
                    tbHTghfAddress.Text = "";
                    tbHTghfPresenter.Text = "";
                    tbHTghfFax.Text = "";
                    tbHTghfPhone.Text = "";
                    tbHTghfEmail.Text = "";
                    tbHTghfBankName.Text = "";
                    tbHTghfBankNumber.Text = "";
                    break;
            }
        }

        private void tbHTxsfID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbHTxsfID.SelectedIndex != -1)
            {
                if (tbHTxsfID.Text.Equals(""))
                {
                    tbHTxsfName.Text = "";
                    tbHTxsfAddress.Text = "";
                    tbHTxsfPresenter.Text = "";
                    tbHTxsfFax.Text = "";
                    tbHTxsfPhone.Text = "";
                    tbHTxsfEmail.Text = "";
                    tbHTxsfBankName.Text = "";
                    tbHTxsfBankNumber.Text = "";
                }
                if (tbHTxsfID.SelectedIndex == 1)
                {
                    InitPicker(tbHTxsfID, true);
                }
                else if (tbHTxsfID.SelectedIndex > 1)
                {
                    FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(new List<Control>() { 
                    tbHTxsfName, tbHTxsfAddress, tbHTxsfPresenter, tbHTxsfFax, tbHTxsfPhone, tbHTxsfEmail, tbHTxsfBankName, tbHTxsfBankNumber },
                            DatabaseConnections.Connector.LocalGetOneRowDataById(
                            "clients",
                            new String[] { "company", "address", "companyOwner", "fax", "phone", "email", "bankName", "bankCard" },
                            "clientID", tbHTxsfID.Text).ToList<String>());
                }
            }
        }

        private void tbHTghfID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbHTghfID.SelectedIndex != -1)
            {
                if (tbHTghfID.Text.Equals(""))
                {
                    tbHTghfName.Text = "";
                    tbHTghfAddress.Text = "";
                    tbHTghfPresenter.Text = "";
                    tbHTghfFax.Text = "";
                    tbHTghfPhone.Text = "";
                    tbHTghfEmail.Text = "";
                    tbHTghfBankName.Text = "";
                    tbHTghfBankNumber.Text = "";
                }
                if (tbHTghfID.SelectedIndex == 1)
                {
                    InitPicker(tbHTghfID, true);
                }
                else if (tbHTghfID.SelectedIndex > 1)
                {
                    FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(new List<Control>() { 
                    tbHTghfName, tbHTghfAddress, tbHTghfPresenter, tbHTghfFax, tbHTghfPhone, tbHTghfEmail, tbHTghfBankName, tbHTghfBankNumber },
                             DatabaseConnections.Connector.LocalGetOneRowDataById(
                             "clients",
                             new String[] { "company", "address", "companyOwner", "fax", "phone", "email", "bankName", "bankCard" },
                             "clientID", tbHTghfID.Text).ToList<String>());
                }
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
        private void JCCDSetControlsValue(List<Control> lcs, QQTextBox hintControl, Control byIdControl)
        {
            if (danziComboBox.SelectedIndex == 0)
            { // 进仓单
                FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(lcs, DatabaseConnections.Connector.LocalGetOneRowDataById("goods", new String[] { "name", "guige", "dengji", "unit", "purchasePrice" }, "goodId", byIdControl.Text).ToList<String>());
                //hintControl.EmptyTextTip = "最大:" + DatabaseConnections.GetInstence().LocalGetOneRowDataById("goods", new String[] { "currentCount" }, "goodId", byIdControl.Text)[0].ToString();
            }
            else
            { // 出仓单
                FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(lcs, DatabaseConnections.Connector.LocalGetOneRowDataById("goods", new String[] { "name", "guige", "dengji", "unit", "currntsalesPrice" }, "goodId", byIdControl.Text).ToList<String>());
                String remainCount = DatabaseConnections.Connector.LocalGetOneRowDataById("goods", new String[] { "currentCount" }, "goodId", byIdControl.Text)[0].ToString();
                hintControl.EmptyTextTip = "库存:" + (remainCount.Equals("") ? "0" : remainCount);
            }
        }

        // 合同
        private void HtSetControlsValue(List<Control> lcs, Control byIdControl)
        {
            if (HTcbName.SelectedIndex == 0)
            { // 购买
                FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(lcs, DatabaseConnections.Connector.LocalGetOneRowDataById("goods", new String[] { "name", "guige", "dengji", "unit", "purchasePrice" }, "goodId", byIdControl.Text).ToList<String>());
            }
            else
            { // 销售
                FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(lcs, DatabaseConnections.Connector.LocalGetOneRowDataById("goods", new String[] { "name", "guige", "dengji", "unit", "currntsalesPrice" }, "goodId", byIdControl.Text).ToList<String>());
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
                    AJCDtb4.ReadOnly = true;
                    AJCDtb5.ReadOnly = true;
                    AJCDtb5.BackColor = SystemColors.Control;
                    clearControlValueByList(new List<Control>() { AJCDtb0, AJCDtb1, AJCDtb2, AJCDtb3, AJCDtb4, AJCDtb5, AJCDtb6 });
                }
                else if (JCDcbA.SelectedIndex == 1)
                {
                    InitPicker(JCDcbA, false);
                }
                else
                {
                    AJCDtb4.ReadOnly = false;
                    AJCDtb5.ReadOnly = false;
                    AJCDtb5.BackColor = SystemColors.Window;
                    JCCDSetControlsValue(new List<Control>() { AJCDtb0, AJCDtb1, AJCDtb2, AJCDtb3, AJCDtb4 }, AJCDtb5, JCDcbA);
                }
            }
        }

        private void JCDcbB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JCDcbB.SelectedIndex != -1)
            {
                if (JCDcbB.Text.Equals(""))
                {
                    BJCDtb4.ReadOnly = true;
                    BJCDtb5.ReadOnly = true;
                    BJCDtb5.BackColor = SystemColors.Control;
                    clearControlValueByList(new List<Control>() { BJCDtb0, BJCDtb1, BJCDtb2, BJCDtb3, BJCDtb4, BJCDtb5, BJCDtb6 });
                }
                else if (JCDcbB.SelectedIndex == 1)
                {
                    InitPicker(JCDcbB, false);
                }
                else
                {
                    BJCDtb4.ReadOnly = false;
                    BJCDtb5.ReadOnly = false;
                    BJCDtb5.BackColor = SystemColors.Window;
                    JCCDSetControlsValue(new List<Control>() { BJCDtb0, BJCDtb1, BJCDtb2, BJCDtb3, BJCDtb4 }, BJCDtb5, JCDcbB);
                }
            }
        }

        private void JCDcbC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JCDcbC.SelectedIndex != -1)
            {
                if (JCDcbC.Text.Equals(""))
                {
                    CJCDtb4.ReadOnly = true;
                    CJCDtb5.ReadOnly = true;
                    CJCDtb5.BackColor = SystemColors.Control;
                    clearControlValueByList(new List<Control>() { CJCDtb0, CJCDtb1, CJCDtb2, CJCDtb3, CJCDtb4, CJCDtb5, CJCDtb6 });
                }
                else if (JCDcbC.SelectedIndex == 1)
                {
                    InitPicker(JCDcbC, false);
                }
                else
                {
                    CJCDtb4.ReadOnly = false;
                    CJCDtb5.ReadOnly = false;
                    CJCDtb5.BackColor = SystemColors.Window;
                    JCCDSetControlsValue(new List<Control>() { CJCDtb0, CJCDtb1, CJCDtb2, CJCDtb3, CJCDtb4 }, CJCDtb5, JCDcbC);
                }
            }
        }

        private void JCDcbD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JCDcbD.SelectedIndex != -1)
            {
                if (JCDcbD.Text.Equals(""))
                {
                    DJCDtb4.ReadOnly = true;
                    DJCDtb5.ReadOnly = true;
                    DJCDtb5.BackColor = SystemColors.Control;
                    clearControlValueByList(new List<Control>() { DJCDtb0, DJCDtb1, DJCDtb2, DJCDtb3, DJCDtb4, DJCDtb5, DJCDtb6 });
                }
                else if (JCDcbD.SelectedIndex == 1)
                {
                    InitPicker(JCDcbD, false);
                }
                else
                {
                    DJCDtb4.ReadOnly = false;
                    DJCDtb5.ReadOnly = false;
                    DJCDtb5.BackColor = SystemColors.Window;
                    JCCDSetControlsValue(new List<Control>() { DJCDtb0, DJCDtb1, DJCDtb2, DJCDtb3, DJCDtb4 }, DJCDtb5, JCDcbD);
                }
            }
        }

        private void JCDcbE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JCDcbE.SelectedIndex != -1)
            {
                if (JCDcbE.Text.Equals(""))
                {
                    EJCDtb4.ReadOnly = true;
                    EJCDtb5.ReadOnly = true;
                    EJCDtb5.BackColor = SystemColors.Control;
                    clearControlValueByList(new List<Control>() { EJCDtb0, EJCDtb1, EJCDtb2, EJCDtb3, EJCDtb4, EJCDtb5, EJCDtb6 });
                }
                else if (JCDcbE.SelectedIndex == 1)
                {
                    InitPicker(JCDcbE, false);
                }
                else
                {
                    EJCDtb4.ReadOnly = false;
                    EJCDtb5.ReadOnly = false;
                    EJCDtb5.BackColor = SystemColors.Window;
                    JCCDSetControlsValue(new List<Control>() { EJCDtb0, EJCDtb1, EJCDtb2, EJCDtb3, EJCDtb4 }, EJCDtb5, JCDcbE);
                }
            }
        }

        /// <summary>
        /// 启用客户商品选择器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void InitPicker(ComboBox cbPicker, Boolean isClient)
        {
            Picker cp = new Picker();
            cp.isClient = isClient;
            if (cp.ShowDialog() == DialogResult.OK)
            {
                cbPicker.Text = cp.selectedClientID;
            }
            else
            {
                cbPicker.SelectedIndex = 0;
            }
        }

        private void tbDz1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbDz1.SelectedIndex > 1)
            {
                FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(new List<Control>() { dzGZBId, dzContact, dzPhone, dzCompany, dzAddress, tbDz8 }, DatabaseConnections.Connector.LocalGetOneRowDataById("clients", new String[] { "gzbID", "companyOwner", "phone", "company", "address", "shouldPay" }, "clientID", tbDz1.Text).ToList<String>());
            }
            else if (tbDz1.SelectedIndex == 0)
            {
                dzContact.Clear();
                dzPhone.Clear();
                dzCompany.Clear();
                dzAddress.Clear();
            }
            else if (tbDz1.SelectedIndex == 1)
            {
                InitPicker(tbDz1, true);
            }
        }

        private void tbPz1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbPz1.SelectedIndex > 1)
            {
                FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(new List<Control>() { pzContact, pzPhone, pzCompany, pzAddress }, DatabaseConnections.Connector.LocalGetOneRowDataById("clients", new String[] { "companyOwner", "phone", "company", "address" }, "clientID", tbPz1.Text).ToList<String>());
            }
            else if (tbPz1.SelectedIndex == 0)
            {
                pzContact.Clear();
                pzPhone.Clear();
                pzCompany.Clear();
                pzAddress.Clear();
            }
            else if (tbPz1.SelectedIndex == 1)
            {
                InitPicker(tbPz1, true);
            }
        }

        private void PzcbA_TextChanged(object sender, EventArgs e)
        {
            if (APztb1.SelectedIndex != 1)
            {
                APztb1.SelectedIndex = 1;
            }
        }

        private void PzcbB_TextChanged(object sender, EventArgs e)
        {
            if (BPztb1.SelectedIndex != 1)
            {
                BPztb1.SelectedIndex = 1;
            }
        }

        private void PzcbC_TextChanged(object sender, EventArgs e)
        {
            if (CPztb1.SelectedIndex != 1)
            {
                CPztb1.SelectedIndex = 1;
            }
        }

        private void PzcbD_TextChanged(object sender, EventArgs e)
        {
            if (DPztb1.SelectedIndex != 1)
            {
                DPztb1.SelectedIndex = 1;
            }
        }

        private void PzcbE_TextChanged(object sender, EventArgs e)
        {
            if (EPztb1.SelectedIndex != 1)
            {
                EPztb1.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// 计算金钱价格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        private void calculateSmallSum(Control left, Control right, Control result)
        {
            float test1, test2;
            if (left.Text.Equals("") || right.Text.Equals("") || !float.TryParse(left.Text, out test1) || !float.TryParse(right.Text, out test2))
            {
            }
            else
            {
                result.Text = (float.Parse(left.Text) * float.Parse(right.Text)).ToString();
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
            float sum = 0;
            foreach (Control con in conList)
            {
                if (!con.Text.Equals(""))
                {
                    sum += float.Parse(con.Text);
                }
            }
            resultControl.Text = FormBasicFeatrues.GetInstence().MoneyToUpper(sum.ToString()) + "￥" + sum.ToString();
        }

        private void calculateSumForDz(object sender, EventArgs e)
        {
            SetTotalSum(new List<Control>() { AJCDtb6, BJCDtb6, CJCDtb6, DJCDtb6, EJCDtb6 }, tbDz3);
        }

        private void calculateSumForPz(object sender, EventArgs e)
        {
            SetTotalSum(new List<Control>() { APztb0, BPztb0, CPztb0, DPztb0, EPztb0 }, SumtbPz);
        }

        private void calculateSumForHt(object sender, EventArgs e)
        {
            SetTotalSum(new List<Control>() { cbHTGoodsA6, cbHTGoodsB6, cbHTGoodsC6, cbHTGoodsD6, cbHTGoodsE6 }, SumHtTextbox);
        }

        /// <summary>
        /// 只能输入字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberInputOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length >= 0)) return;   //处理负数
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
                APztb2.Text = "";
                APztb3.Text = "";
                APztb2.ReadOnly = true;
                APztb3.ReadOnly = true;
            }
            else if (APztb1.SelectedIndex == 1)
            {
                APztb2.ReadOnly = true;
                APztb3.ReadOnly = true;
            }
            else if (APztb1.SelectedIndex > 1)
            {
                APztb2.ReadOnly = false;
                APztb3.ReadOnly = false;
            }
        }

        private void BPztb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BPztb1.SelectedIndex == 0)
            {
                BPztb2.Text = "";
                BPztb3.Text = "";
                BPztb2.ReadOnly = true;
                BPztb3.ReadOnly = true;
            }
            else if (BPztb1.SelectedIndex == 1)
            {
                BPztb2.ReadOnly = true;
                BPztb3.ReadOnly = true;
            }
            else if (BPztb1.SelectedIndex > 1)
            {
                BPztb2.ReadOnly = false;
                BPztb3.ReadOnly = false;
            }
        }

        private void CPztb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CPztb1.SelectedIndex == 0)
            {
                CPztb2.Text = "";
                CPztb3.Text = "";
                CPztb2.ReadOnly = true;
                CPztb3.ReadOnly = true;
            }
            else if (CPztb1.SelectedIndex == 1)
            {
                CPztb2.ReadOnly = true;
                CPztb3.ReadOnly = true;
            }
            else if (CPztb1.SelectedIndex > 1)
            {
                CPztb2.ReadOnly = false;
                CPztb3.ReadOnly = false;
            }
        }

        private void DPztb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DPztb1.SelectedIndex == 0)
            {
                DPztb2.Text = "";
                DPztb3.Text = "";
                DPztb2.ReadOnly = true;
                DPztb3.ReadOnly = true;
            }
            else if (DPztb1.SelectedIndex == 1)
            {
                DPztb2.ReadOnly = true;
                DPztb3.ReadOnly = true;
            }
            else if (DPztb1.SelectedIndex > 1)
            {
                DPztb2.ReadOnly = false;
                DPztb3.ReadOnly = false;
            }
        }

        private void EPztb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EPztb1.SelectedIndex == 0)
            {
                EPztb2.Text = "";
                EPztb3.Text = "";
                EPztb2.ReadOnly = true;
                EPztb3.ReadOnly = true;
            }
            else if (EPztb1.SelectedIndex == 1)
            {
                EPztb2.ReadOnly = true;
                EPztb3.ReadOnly = true;
            }
            else if (EPztb1.SelectedIndex > 1)
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
                if (DatabaseConnections.Connector.LocalCheckIfDuplicate(table, baseName, (sender as TextBox).Text))
                {
                    MessageBox.Show("您设定的编号已经被占用, 请再次输入", "错误");
                    (sender as TextBox).Text = ItemId.Equals("-1") ? "" : ItemId;
                    (sender as TextBox).Focus();
                }
            }
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
                    // e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", 850, 40);
                    //this.printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", this.printDocument1.DefaultPageSettings.PaperSize.Width, 600);

                    PrintDZ(0, 30, true, e);
                    break;
                case 2: // 采购销售单
                    //PrintCGXSD(0, 0, yewuPanel, e);
                    PrintDZ(0, 30, false, e);
                    break;
                case 3: // 采购销售列表
                    //PrintWithDGV(0, 0, panel15, dgvCGXS, 30, e);
                    break;
                case 4: // 对账单
                    //PrintWithDGV(0, 0, panel9, dgvYWDZ, 30, e);
                    break;
                case 5: // 凭证
                    PrintPZ(0, 30, e);
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
        private void SetPrintPreview(int flag, int pageHeight)
        {
            if (FormBasicFeatrues.GetInstence().isPassValidateControls(checkValidateControls))
            {
                printFlag = flag;

                this.printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", this.printDocument1.DefaultPageSettings.PaperSize.Width, pageHeight);

                pageWidth = this.printDocument1.DefaultPageSettings.PaperSize.Width;
                pageHeight = this.printDocument1.DefaultPageSettings.PaperSize.Height;

                ////注意指定其Document(获取或设置要预览的文档)属性
                //this.printPreviewDialog1.Document = this.printDocument1;
                ////ShowDialog方法：将窗体显示为模式对话框，并将当前活动窗口设置为它的所有者
                //this.printPreviewDialog1.WindowState = FormWindowState.Maximized;
                //this.printPreviewDialog1.ShowDialog();

                using (var dlg = new CoolPrintPreviewDialog())
                {
                    dlg.Document = this.printDocument1;
                    //dlg.WindowState = FormWindowState.Maximized;
                    dlg.Size = new Size(800, 600);
                    if (!dzGZBId.Text.Equals("") && (printFlag == 2) && (danziComboBox.SelectedIndex == 1) && (MainWindow.DEGREE > 0))
                    {
                        dlg.sendRemoteSignToolStripButton.Enabled = true;
                        dlg.gzbIDString = dzGZBId.Text;
                        dlg.companyNickNameStirng = dzCompany.Text;
                    }
                    if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        SaveButton.PerformClick();
                    }
                }
            }
            else
            {
                MessageBox.Show("请先填入必填项目再打开预览!", "提示");
            }
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
            }
        }

        // 打印单子 进仓单 出仓单 采购单 销售单
        private void PrintDZ(int x, int y, Boolean isJCCD, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;   //先建立画布
            SizeF fontSize;

            Font f1 = new Font("黑体", 20, FontStyle.Bold);
            Font f2 = new Font("微软雅黑", 9);
            Font f3 = new Font("微软雅黑", 11);
            Font f4 = new Font("微软雅黑", 8);
            Font f5 = new Font("微软雅黑", 10.5f);
            Font f6 = new Font("微软雅黑", 7);
            Font f7 = new Font("微软雅黑", 6);

            int fontDisX = 3;
            int fontDisY = 3;
            int tableX = 50;
            int tableY = 185;

            //
            fontSize = g.MeasureString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", MainWindow.COMPANY_NAME), f3);//桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司
            g.DrawString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", MainWindow.COMPANY_NAME), f3, new SolidBrush(Color.Blue), pageWidth / 2 - fontSize.Width / 2 + x, y);
            //fontSize = g.MeasureString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", "桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司"), f3);//桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司
            //g.DrawString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", "桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司"), f3, new SolidBrush(Color.Blue), pageWidth / 2 - fontSize.Width / 2 + x, y);

            fontSize = g.MeasureString(lbDzTitle.Text, f1);
            g.DrawString(lbDzTitle.Text, f1, new SolidBrush(Color.Black), pageWidth / 2 - fontSize.Width / 2 + x, y + 40 - 15);

            g.DrawString("TAL：" + MainWindow.PHONE, f3, new SolidBrush(dzContact.ForeColor), tableX + x, 50 + y + fontDisY - 20);

            g.DrawString("FAX：" + MainWindow.FAX, f5, new SolidBrush(dzContact.ForeColor), tableX + x, 80 + y + fontDisY - 20);

            //   以质为根   以诚为本
            fontSize = g.MeasureString("以质为根   以诚为本", f5);
            g.DrawString("以质为根   以诚为本", f5, new SolidBrush(dzContact.ForeColor), tableX + x, tableY - 26 * 3 + y + fontDisY - 20);
            //g.DrawRectangle(new Pen(Color.Black), tableX + x + 733 - tbDz2.Size.Width, 80 + y, tbDz2.Size.Width, tbDz2.Height);
            g.DrawString("", f5, new SolidBrush(dzContact.ForeColor), tableX + x, tableY - 26 * 3 + y + fontDisY - 20);

            fontSize = g.MeasureString("客户编号：", f5);
            g.DrawString("客户编号：", f5, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - fontSize.Width - tbDz1.Size.Width - 5, 50 + y + fontDisY - 20);
            g.DrawString(tbDz1.Text, f5, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - tbDz2.Size.Width + fontDisX, 50 + y + fontDisY - 20);

            fontSize = g.MeasureString("凭证号码：", f5);
            g.DrawString("凭证号码：", f5, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - fontSize.Width - tbDz2.Size.Width - 5, 80 + y + fontDisY - 20);
            g.DrawString(tbDz2.Text, f5, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - tbDz2.Size.Width + fontDisX, 80 + y + fontDisY - 20);


            fontSize = g.MeasureString("业务联/白色； 财务联/黄色； 仓库联/蓝色； 客户联/红色", f2);
            g.DrawString("业务联/白色； 财务联/黄色； 仓库联/蓝色； 客户联/红色", f2, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX + 733 - fontSize.Width, tableY - 26 * 3 + y + fontDisY - 20);

            g.DrawRectangle(new Pen(Color.Black), tableX + x, tableY - 26 * 2 + y - 20, 116, dzContact.Height);
            g.DrawString("对方单位：", dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, tableY - 26 * 2 + y + fontDisY - 20);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, tableY - 26 * 2 + y - 20, 268, dzContact.Height);
            g.DrawString(dzContact.Text, dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 116 + fontDisX, tableY - 26 * 2 + y + fontDisY - 20);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 384, tableY - 26 * 2 + y - 20, 111, dzContact.Height);
            g.DrawString("日期：", dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 384 + fontDisX, tableY - 26 * 2 + y + fontDisY - 20);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 111 + 384, tableY - 26 * 2 + y - 20, 238, dzContact.Height);
            g.DrawString(DzDateTextBox.Text, dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 111 + 384 + fontDisX, tableY - 26 * 2 + y + fontDisY - 20);

            g.DrawRectangle(new Pen(Color.Black), tableX + x, tableY - 26 + y - 20, 116, dzContact.Height);
            g.DrawString("联系地址：", dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, tableY - 26 + y + fontDisY - 20);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, tableY - 26 + y - 20, 268, dzAddress.Height);
            // g.DrawString(dzAddress.Text, dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 116 + fontDisX, tableY - 26 + y + fontDisY - 20);
            Font fontTemp = FormBasicFeatrues.GetInstence().setStringFontByLongestSize(268, dzAddress.Font, dzAddress.Text, g);
            g.DrawString(dzAddress.Text, fontTemp, new SolidBrush(dzContact.ForeColor), tableX + x + 116 + fontDisX, tableY - 26 + y - 20 + dzAddress.Height / 2 - fontTemp.Height / 2);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 384, tableY - 26 + y - 20, 111, dzContact.Height);
            g.DrawString("联系电话：", dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 384 + fontDisX, tableY - 26 + y + fontDisY - 20);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 111 + 384, tableY - 26 + y - 20, 238, dzPhone.Height);
            g.DrawString(dzPhone.Text, dzContact.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 111 + 384 + fontDisX, tableY - 26 + y + fontDisY - 20);

            // 表格之后
            g.DrawRectangle(new Pen(Color.Black), tableX + x, 156 + y + tableY - 20, 116, tbDz3.Height);
            g.DrawString("总金额：", tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, 156 + y + fontDisY + tableY - 20);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, 156 + y + tableY - 20, 617, tbDz3.Height);
            g.DrawString(tbDz3.Text.Equals("") ? "" : tbDz3.Text, tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 116 + fontDisX, 156 + y + fontDisY + tableY - 20);

            if (isJCCD)
            {
                g.DrawRectangle(new Pen(Color.Black), tableX + x, 182 + y + tableY - 20, 452, 104);
                g.DrawString("备注：\n" + tbDz4.Text, tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, 182 + y + fontDisY + tableY - 20);
            }
            else
            {
                if (danziComboBox.SelectedIndex == 0)
                {
                    g.DrawRectangle(new Pen(Color.Black), tableX + x, 182 + y + tableY - 20, 560, 104);
                    g.DrawString("注：\n（1）本期采购单具有替代\"进仓通知单\"的作用，自动导入关联数据并生成进仓单。" + tbDz4.Text, tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, 182 + y + fontDisY + tableY - 20);
                    g.DrawRectangle(new Pen(Color.Black), tableX + x + 560, 182 + y + tableY - 20, 173, 78);
                    g.DrawString("发票号码：", tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 560 + fontDisX, 182 + y + fontDisY + tableY - 20);
                    g.DrawString("增：" + tbDz5.Text, tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 560 + fontDisX, 208 + y + fontDisY + tableY - 20);
                    g.DrawString("普：" + tbDz6.Text, tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 560 + fontDisX, 234 + y + fontDisY + tableY - 20);

                    g.DrawRectangle(new Pen(Color.Black), tableX + x + 560, 260 + y + tableY - 20, 173, 26);
                    g.DrawString("附件凭证： " + tbDz7.Text, tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 560 + fontDisX, 260 + y + fontDisY + tableY - 20);
                    g.DrawString("张", tbDz3.Font, new SolidBrush(dzContact.ForeColor), tableX + x + 700 + fontDisX, 260 + y + fontDisY + tableY - 20);
                }
                else
                {

                    g.DrawRectangle(new Pen(Color.Black), tableX + x, 182 + y + tableY - 20, 183, tbDz8.Height);
                    g.DrawString("原前帐，购货方尚欠销货方货款(元)：￥" + tbDz8.Text, f7, new SolidBrush(dzContact.ForeColor), tableX + x + 3, 182 + y + tableY + 6 - 20);

                    g.DrawRectangle(new Pen(Color.Black), tableX + x + 183, 182 + y + tableY - 20, 183, tbDz9.Height);
                    g.DrawString("今日(本销货单)新增欠款(元)：￥" + tbDz9.Text, f7, new SolidBrush(dzContact.ForeColor), tableX + x + 186, 182 + y + tableY + 6 - 20);

                    g.DrawRectangle(new Pen(Color.Black), tableX + x + 366, 182 + y + tableY - 20, 168, tbDz10.Height);
                    g.DrawString("购货方今日支付销货方货款(元)：￥" + tbDz10.Text, f7, new SolidBrush(dzContact.ForeColor), tableX + x + 369, 182 + y + tableY + 6 - 20);

                    g.DrawRectangle(new Pen(Color.Black), tableX + x + 535, 182 + y + tableY - 20, 198, tbDz11.Height);
                    g.DrawString("至今日止购货方尚欠销货方货款(元)：￥" + tbDz11.Text, f7, new SolidBrush(dzContact.ForeColor), tableX + x + 537, 182 + y + tableY + 6 - 20);

                    //g.DrawRectangle(new Pen(Color.Black), tableX + x + 230, 182 + y + tableY + tbDz9.Height + tbDz11.Height - 20, 230, tbDz12.Height);
                    //g.DrawString("赊欠期限(天): " + tbDz12.Text, f6, new SolidBrush(dzContact.ForeColor), tableX + x + 226, 182 + y + tableY + tbDz9.Height + tbDz11.Height + 6 - 20);

                    g.DrawRectangle(new Pen(Color.Black), tableX + x, 208 + y + tableY - 20, 733, 85);
                    String beizhu = "注：" + tbDz4.Text + "\n"
                                         + "一、上述货物经购货方验收合格并已收妥，除双方另有书面约定外，双方承认本销售单具有替代购销合同的作用和法律效力。\n"
                                         + "二、在购货方赊欠或拖欠销货方货款时，所欠货款则自动转为购货方向销货方借款。本销售单替代并与欠（借）款凭据具有同等的作用和法律效力。\n"
                                         + "三、货款结算期限，除双方麟游书面约定外，自销货方交货之日起 " + tbDz12.Text + " 天内付清。购货方若逾期支付，则依合同法承担违约责任。\n"
                                         + "四、遇有争议，应友好协商，若协商不成，则由销货方所在地人民法院处理。\n"
                                         + "五、本销售单经双方盖章或经办人签字（含电子签名）后即为生效。\n";
                    g.DrawString(beizhu, f6, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX + 3, 185 + y + tableY + tbDz9.Height - 20);
                }
            }


            g.DrawString("对方办理人\n（签 字）：", f3, new SolidBrush(Color.Black), tableX + x, 300 + y + fontDisY + tableY - 20);
            g.DrawString("我方经办人\n（签 字）：", f3, new SolidBrush(Color.Black), tableX + x + 733 / 2 - 90, 300 + y + fontDisY + tableY - 20);
            g.DrawString("我方核准人\n（签 字）：", f3, new SolidBrush(Color.Black), tableX + x + 733 - 190, 300 + y + fontDisY + tableY - 20);

            foreach (Control item in PanelDZ.Controls)
            {
                if (item is Label)
                {
                    Control tx = (item as Control);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x + tableX, tx.Top + y + tableY - 20);
                }
                if (item is TextBox)
                {
                    TextBox tx = (item as TextBox);
                    //g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Location.X + x + tableX, tx.Location.Y + y + 3 + tableY);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Location.X + x + tableX + fontDisX, tx.Location.Y + y + tableY + fontDisY - 20);
                    if (tx.BorderStyle == BorderStyle.FixedSingle)
                    {
                        //g.DrawRectangle(new Pen(Color.Black), tx.Left + x + tableX, tx.Top + y + tableY, tx.Width, tx.Height - 4);
                        g.DrawRectangle(new Pen(Color.Black), tx.Left + x + tableX, tx.Top + y + tableY - 20, tx.Width, tx.Height);
                    }
                }
                if (item is ComboBox)
                {
                    ComboBox tx = (item as ComboBox);
                    //g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x + tableX, tx.Top + y + 3 + tableY);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x + tableX + fontDisX, tx.Top + y + tableY + fontDisY - 20);
                    //g.DrawRectangle(new Pen(Color.Black), tx.Left + x + tableX, tx.Top + y + tableY, tx.Width, tx.Height - 6);
                    g.DrawRectangle(new Pen(Color.Black), tx.Left + x + tableX, tx.Top + y + tableY - 20, tx.Width, 26);
                }
            }
        }

        // 打印凭证
        private void PrintPZ(int x, int y, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;   //先建立画布
            SizeF fontSize;

            Font f1 = new Font("黑体", 20, FontStyle.Bold);
            Font f2 = new Font("微软雅黑", 9);
            Font f3 = new Font("微软雅黑", 11);
            Font f4 = new Font("微软雅黑", 8);
            Font f5 = new Font("微软雅黑", 10.5f);

            int fontDisX = 3;
            int fontDisY = -17;
            int tableX = 50;
            int tableY = 165;

            //fontSize = g.MeasureString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", "桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司"), f3);
            fontSize = g.MeasureString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", MainWindow.COMPANY_NAME), f3);//桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司
            g.DrawString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", MainWindow.COMPANY_NAME), f3, new SolidBrush(Color.Blue), pageWidth / 2 - fontSize.Width / 2 + x, y - 10);
            //g.DrawString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", "桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司"), f3, new SolidBrush(Color.Blue), pageWidth / 2 - fontSize.Width / 2 + x, y-10);
            fontSize = g.MeasureString(lbPzTitle.Text, f1);
            g.DrawString(lbPzTitle.Text, f1, new SolidBrush(Color.Black), pageWidth / 2 - fontSize.Width / 2 + x, y + 40 - 20);

            g.DrawString("TAL：" + MainWindow.PHONE, f3, new SolidBrush(dzContact.ForeColor), tableX + x, 50 + y + fontDisY - 20);

            g.DrawString("FAX：" + MainWindow.FAX, f5, new SolidBrush(dzContact.ForeColor), tableX + x, 80 + y + fontDisY - 20);

            //   以质为根   以诚为本
            fontSize = g.MeasureString("以质为根   以诚为本", f5);
            g.DrawString("以质为根   以诚为本", f5, new SolidBrush(dzContact.ForeColor), tableX + x, tableY - 26 * 3 + y + fontDisY);

            fontSize = g.MeasureString("客户编号：", f5);
            g.DrawString("客户编号：", f5, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - fontSize.Width - tbPz2.Size.Width - 5, 50 + y + fontDisY - 20);
            g.DrawString(tbPz1.Text, f5, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - tbPz2.Size.Width + fontDisX, 50 + y + fontDisY - 20);

            fontSize = g.MeasureString("凭证号码：", f5);
            g.DrawString("凭证号码：", f5, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - fontSize.Width - tbPz2.Size.Width - 5, 80 + y + fontDisY - 20);
            g.DrawString(tbPz2.Text, f5, new SolidBrush(dzContact.ForeColor), tableX + x + 733 - tbPz2.Size.Width + fontDisX, 80 + y + fontDisY - 20);

            fontSize = g.MeasureString("业务联/白色； 财务联/黄色； 仓库联/蓝色； 客户联/红色", f2);
            g.DrawString("业务联/白色； 财务联/黄色； 仓库联/蓝色； 客户联/红色", f2, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX + 733 - fontSize.Width, tableY - 26 * 3 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x, tableY - 26 * 2 + y - 20, 116, dzContact.Height);
            g.DrawString("对方单位：", f5, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, tableY - 26 * 2 + y - 20, 268, dzContact.Height);
            g.DrawString(pzCompany.Text, f5, new SolidBrush(dzContact.ForeColor), tableX + x + 116 + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 384, tableY - 26 * 2 + y - 20, 111, dzContact.Height);
            g.DrawString("日期：", f5, new SolidBrush(dzContact.ForeColor), tableX + x + 384 + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 111 + 384, tableY - 26 * 2 + y - 20, 238, dzContact.Height);
            g.DrawString(PzDateTextBox.Text, f5, new SolidBrush(dzContact.ForeColor), tableX + x + 111 + 384 + fontDisX, tableY - 26 * 2 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x, tableY - 26 + y - 20, 116, dzContact.Height);
            g.DrawString("联系地址：", f5, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, tableY - 26 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, tableY - 26 + y - 20, 268, dzAddress.Height);
            //g.DrawString(pzAddress.Text, f5, new SolidBrush(dzContact.ForeColor), tableX + x + 116 + fontDisX, tableY - 26 + y + fontDisY);
            Font fontTemp = FormBasicFeatrues.GetInstence().setStringFontByLongestSize(268, pzAddress.Font, pzAddress.Text, g);
            g.DrawString(pzAddress.Text, fontTemp, new SolidBrush(pzAddress.ForeColor), tableX + x + 116, tableY - 26 + y - 20 + pzAddress.Height / 2 - fontTemp.Height / 2);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 384, tableY - 26 + y - 20, 111, dzContact.Height);
            g.DrawString("联系电话：", f5, new SolidBrush(dzContact.ForeColor), tableX + x + 384 + fontDisX, tableY - 26 + y + fontDisY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 111 + 384, tableY - 26 + y - 20, 238, dzPhone.Height);
            g.DrawString(pzPhone.Text, f5, new SolidBrush(dzContact.ForeColor), tableX + x + 111 + 384 + fontDisX, tableY - 26 + y + fontDisY);

            // 表格之后
            g.DrawRectangle(new Pen(Color.Black), tableX + x, 156 + y + tableY - 20, 116, tbPz3.Height);
            g.DrawString("总金额：", f5, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, 156 + y + fontDisY + tableY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x + 116, 156 + y + tableY - 20, 617, tbPz3.Height);
            g.DrawString(SumtbPz.Text.Equals("") ? "" : SumtbPz.Text, f5, new SolidBrush(dzContact.ForeColor), tableX + x + 116 + fontDisX, 156 + y + fontDisY + tableY);

            g.DrawRectangle(new Pen(Color.Black), tableX + x, 182 + y + tableY - 20, 733, 104);
            g.DrawString("备注：\n" + tbPz3.Text, f5, new SolidBrush(dzContact.ForeColor), tableX + x + fontDisX, 182 + y + fontDisY + tableY);

            g.DrawString("对方办理人\n（签 字）：", f3, new SolidBrush(Color.Black), tableX + x, 300 + y + fontDisY + tableY);
            g.DrawString("我方经办人\n（签 字）：", f3, new SolidBrush(Color.Black), tableX + x + 733 / 2 - 90, 300 + y + fontDisY + tableY);
            g.DrawString("我方核准人\n（签 字）：", f3, new SolidBrush(Color.Black), tableX + x + 733 - 190, 300 + y + fontDisY + tableY);

            foreach (Control item in pzPanel.Controls)
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
                        g.DrawRectangle(new Pen(Color.Black), tx.Left + x + tableX, tx.Top + y + tableY - 20, tx.Width, tx.Height);
                    }
                }
                if (item is ComboBox)
                {
                    ComboBox tx = (item as ComboBox);
                    //g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x + tableX, tx.Top + y + 3 + tableY);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x + tableX + fontDisX, tx.Top + y + tableY + fontDisY);
                    //g.DrawRectangle(new Pen(Color.Black), tx.Left + x + tableX, tx.Top + y + tableY, tx.Width, tx.Height - 6);
                    g.DrawRectangle(new Pen(Color.Black), tx.Left + x + tableX, tx.Top + y + tableY - 20, tx.Width, 26);
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
            Font f4 = new Font("楷体", 9);//华文行楷
            Font f45 = new Font("楷体", 9, FontStyle.Bold);//华文行楷
            Font f5 = new Font("楷体", 9, FontStyle.Underline);//华文行楷
            Font f6 = new Font("楷体", 9);
            Font f7 = new Font("楷体", 10);//华文行楷

            SizeF fontSize;
            //
            fontSize = g.MeasureString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", MainWindow.COMPANY_NAME), f3);//桐 乡 市 瑞 递 曼 尔 工 贸 有 限 公 司
            g.DrawString(FormBasicFeatrues.GetInstence().addCharIntoString("  ", MainWindow.COMPANY_NAME), f3, new SolidBrush(Color.Blue), pageWidth / 2 - fontSize.Width / 2 + x, y);

            //
            fontSize = g.MeasureString(FormBasicFeatrues.GetInstence().addCharIntoString(" ", HTcbName.Text), f1);//"购 销 合 同"
            g.DrawString(FormBasicFeatrues.GetInstence().addCharIntoString(" ", HTcbName.Text), f1, new SolidBrush(Color.Red), pageWidth / 2 - fontSize.Width / 2 + x, 30 + y);

            //
            g.DrawString("购货方：", f4, new SolidBrush(Color.Black), 40 + x, 80 + y);
            fontSize = g.MeasureString("购货方：", f4);
            g.DrawString(tbHTghfName.Text, f5, new SolidBrush(Color.Black), 40 + fontSize.Width + x, 80 + y);

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
            g.DrawString(tbHTxsfName.Text, f5, new SolidBrush(Color.Black), 40 + fontSize.Width + x, 120 + y);

            g.DrawString("签约地点：", f4, new SolidBrush(Color.Black), 150 + pageWidth / 2 + x, 120 + y);
            fontSize = g.MeasureString("签约地点：", f4);
            g.DrawString(HTtbLocation.Text, f5, new SolidBrush(Color.Black), 150 + pageWidth / 2 + fontSize.Width + x, 120 + y);

            // 
            g.DrawString("根据中华人民共和国经济合同法和有关政策规定，经购销双方协商后，一致同意签订本合同，合同内容如下：", f4, new SolidBrush(Color.Black), 40 + x, 160 + y);

            // 
            g.DrawString("1、交易内容（品名、规格、数量、单位、单价、金额、交（提）货日期）：", f4, new SolidBrush(Color.Black), 40 + x, 200 + y);

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

            float LieX1 = 43 + x;
            float LieX2 = 146 + x;
            float LieX3 = 243 + x;
            float LieX4 = 317 + x + 20;
            float LieX5 = 392 + x + 18;
            float LieX6 = 464 + x + 18;
            float LieX7 = 555 + x - 10;

            float HangY1 = 279 + y;
            float HangY2 = 318 + y;
            float HangY3 = 355 + y;
            float HangY4 = 392 + y;
            float HangY5 = 429 + y;

            //第二行
            g.DrawString(cbHTGoodsA0.Text, f4, new SolidBrush(Color.Black), LieX1, HangY1);
            g.DrawString(cbHTGoodsA1.Text, f4, new SolidBrush(Color.Black), LieX2, HangY1);
            g.DrawString(cbHTGoodsA3.Text, f4, new SolidBrush(Color.Black), LieX3, HangY1);
            g.DrawString(cbHTGoodsA4.Text, f4, new SolidBrush(Color.Black), LieX4, HangY1);
            g.DrawString(cbHTGoodsA5.Text, f4, new SolidBrush(Color.Black), LieX5, HangY1);
            g.DrawString(cbHTGoodsA6.Text, f4, new SolidBrush(Color.Black), LieX6, HangY1);
            g.DrawString(cbHTGoodsA7.Text, f4, new SolidBrush(Color.Black), LieX7, HangY1);
            //第三行
            g.DrawString(cbHTGoodsB0.Text, f4, new SolidBrush(Color.Black), LieX1, HangY2);
            g.DrawString(cbHTGoodsB1.Text, f4, new SolidBrush(Color.Black), LieX2, HangY2);
            g.DrawString(cbHTGoodsB3.Text, f4, new SolidBrush(Color.Black), LieX3, HangY2);
            g.DrawString(cbHTGoodsB4.Text, f4, new SolidBrush(Color.Black), LieX4, HangY2);
            g.DrawString(cbHTGoodsB5.Text, f4, new SolidBrush(Color.Black), LieX5, HangY2);
            g.DrawString(cbHTGoodsB6.Text, f4, new SolidBrush(Color.Black), LieX6, HangY2);
            g.DrawString(cbHTGoodsB7.Text, f4, new SolidBrush(Color.Black), LieX7, HangY2);
            //第四行
            g.DrawString(cbHTGoodsC0.Text, f4, new SolidBrush(Color.Black), LieX1, HangY3);
            g.DrawString(cbHTGoodsC1.Text, f4, new SolidBrush(Color.Black), LieX2, HangY3);
            g.DrawString(cbHTGoodsC3.Text, f4, new SolidBrush(Color.Black), LieX3, HangY3);
            g.DrawString(cbHTGoodsC4.Text, f4, new SolidBrush(Color.Black), LieX4, HangY3);
            g.DrawString(cbHTGoodsC5.Text, f4, new SolidBrush(Color.Black), LieX5, HangY3);
            g.DrawString(cbHTGoodsC6.Text, f4, new SolidBrush(Color.Black), LieX6, HangY3);
            g.DrawString(cbHTGoodsC7.Text, f4, new SolidBrush(Color.Black), LieX7, HangY3);
            //第五行
            g.DrawString(cbHTGoodsD0.Text, f4, new SolidBrush(Color.Black), LieX1, HangY4);
            g.DrawString(cbHTGoodsD1.Text, f4, new SolidBrush(Color.Black), LieX2, HangY4);
            g.DrawString(cbHTGoodsD3.Text, f4, new SolidBrush(Color.Black), LieX3, HangY4);
            g.DrawString(cbHTGoodsD4.Text, f4, new SolidBrush(Color.Black), LieX4, HangY4);
            g.DrawString(cbHTGoodsD5.Text, f4, new SolidBrush(Color.Black), LieX5, HangY4);
            g.DrawString(cbHTGoodsD6.Text, f4, new SolidBrush(Color.Black), LieX6, HangY4);
            g.DrawString(cbHTGoodsD7.Text, f4, new SolidBrush(Color.Black), LieX7, HangY4);
            //第六行
            g.DrawString(cbHTGoodsE0.Text, f4, new SolidBrush(Color.Black), LieX1, HangY5);
            g.DrawString(cbHTGoodsE1.Text, f4, new SolidBrush(Color.Black), LieX2, HangY5);
            g.DrawString(cbHTGoodsE3.Text, f4, new SolidBrush(Color.Black), LieX3, HangY5);
            g.DrawString(cbHTGoodsE4.Text, f4, new SolidBrush(Color.Black), LieX4, HangY5);
            g.DrawString(cbHTGoodsE5.Text, f4, new SolidBrush(Color.Black), LieX5, HangY5);
            g.DrawString(cbHTGoodsE6.Text, f4, new SolidBrush(Color.Black), LieX6, HangY5);
            g.DrawString(cbHTGoodsE7.Text, f4, new SolidBrush(Color.Black), LieX7, HangY5);

            //
            //第一列
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y + htHeight, htWidth, htHeight - 2);
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y + 2 * htHeight - 2, htWidth, htHeight + 2);
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y + 3 * htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y + 4 * htHeight, htWidth, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + x, b + y + 5 * htHeight, htWidth, htHeight);
            //g.DrawString(, f4, new SolidBrush(Color.Black), 555 + x + 20, 242 + y);

            //第二列
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y, htWidth - 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y + htHeight, htWidth - 2, htHeight - 2);
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y + 2 * htHeight - 2, htWidth - 2, htHeight + 2);
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y + 3 * htHeight, htWidth - 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y + 4 * htHeight, htWidth - 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + htWidth + x, b + y + 5 * htHeight, htWidth - 2, htHeight);
            //第三列
            g.DrawRectangle(new Pen(Color.Black), a + 2 * htWidth + x - 2, b + y, htWidth - 27, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 2 * htWidth + x - 2, b + y + htHeight, htWidth - 27, htHeight - 2);
            g.DrawRectangle(new Pen(Color.Black), a + 2 * htWidth + x - 2, b + y + 2 * htHeight - 2, htWidth - 27, htHeight + 2);
            g.DrawRectangle(new Pen(Color.Black), a + 2 * htWidth + x - 2, b + y + 3 * htHeight, htWidth - 27, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 2 * htWidth + x - 2, b + y + 4 * htHeight, htWidth - 27, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 2 * htWidth + x - 2, b + y + 5 * htHeight, htWidth - 27, htHeight);
            //第四列
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27 - 2, b + y, htWidth - 27 + 1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27 - 2, b + y + htHeight, htWidth - 27 + 1, htHeight - 2);
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27 - 2, b + y + 2 * htHeight - 2, htWidth - 27 + 1, htHeight + 2);
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27 - 2, b + y + 3 * htHeight, htWidth - 27 + 1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27 - 2, b + y + 4 * htHeight, htWidth - 27 + 1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 3 * htWidth + x - 27 - 2, b + y + 5 * htHeight, htWidth - 27 + 1, htHeight);
            //第五列
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54 - 1, b + y, htWidth - 27 + 1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54 - 1, b + y + htHeight, htWidth - 27 + 1, htHeight - 2);
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54 - 1, b + y + 2 * htHeight - 2, htWidth - 27 + 1, htHeight + 2);
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54 - 1, b + y + 3 * htHeight, htWidth - 27 + 1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54 - 1, b + y + 4 * htHeight, htWidth - 27 + 1, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 4 * htWidth + x - 54 - 1, b + y + 5 * htHeight, htWidth - 27 + 1, htHeight);
            //第六列
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x - 81, b + y, htWidth - 27 + 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x - 81, b + y + htHeight, htWidth - 27 + 2, htHeight - 2);
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x - 81, b + y + 2 * htHeight - 2, htWidth - 27 + 2, htHeight + 2);
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x - 81, b + y + 3 * htHeight, htWidth - 27 + 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x - 81, b + y + 4 * htHeight, htWidth - 27 + 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 5 * htWidth + x - 81, b + y + 5 * htHeight, htWidth - 27 + 2, htHeight);
            //第七列
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x - 108 + 2, b + y, htWidth + 108 - 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x - 108 + 2, b + y + htHeight, htWidth + 108 - 2, htHeight - 2);
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x - 108 + 2, b + y + 2 * htHeight - 2, htWidth + 108 - 2, htHeight + 2);
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x - 108 + 2, b + y + 3 * htHeight, htWidth + 108 - 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x - 108 + 2, b + y + 4 * htHeight, htWidth + 108 - 2, htHeight);
            g.DrawRectangle(new Pen(Color.Black), a + 6 * htWidth + x - 108 + 2, b + y + 5 * htHeight, htWidth + 108 - 2, htHeight);

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
            g.DrawString((tempString.Length > 55) ? tempString.Insert(55, "\n") : tempString, f4, new SolidBrush(Color.Black), 40 + x, 565 + y);
            Console.WriteLine("####" + HTcbChoose7.Text.Length);
            // 
            g.DrawString("8、违约责任：如本合同执行过程中发生纠纷，双方应本着友好合作的态度进行协商解决，遇协商不成时，遵照国家相关法律\n法规及法定程序，任何一方均有权提请销货方所在地的人民法院对有争议的事项依法做出裁决。", f4, new SolidBrush(Color.Black), 40 + x, 600 + y);

            // 
            g.DrawString("9、其它条款：(1)本合同未尽事宜皆按中华人民共和国各项法律之规定处理。（2）本合同如有附件，既与正文具有同等效力。\n（3）本合同一经生效，以前有关本合同（本批贸易）的函电、文件与本合同具有抵触的内容均为无效。（4）本合同一式二份\n（双方各执一份），经双方代理人签字（单位须加盖公章或合同章）后生效。双方必须全面履行本合同，任何一方不得擅自变\n更或解除。", f4, new SolidBrush(Color.Black), 40 + x, 635 + y);

            g.DrawString("10、双方代理人签字（单位必须加盖公章或合同章）：", f4, new SolidBrush(Color.Black), 40 + x, 700 + y);

            // 框
            int recWidth = (pageWidth - 60 * 2) / 2;
            int recHeight = 320;
            g.DrawRectangle(new Pen(Color.Black), 40 + x, 720 + y, recWidth, recHeight);
            g.DrawRectangle(new Pen(Color.Black), 40 + recWidth + x, 720 + y, recWidth, recHeight);

            float KuangY1 = 735 + y;
            float KuangY2 = 760 + y;
            float KuangY3 = 795 + y;
            float KuangY4 = 825 + y;
            float KuangY5 = 875 + y;
            float KuangY6 = 905 + y;
            float KuangY7 = 935 + y;
            float KuangY8 = 985 + y;
            float KuangY9 = 1015 + y;
            // 框内
            fontSize = g.MeasureString("购  货  方", f45);
            g.DrawString("购  货  方", f45, new SolidBrush(Color.Black), 40 + x + recWidth / 2 - fontSize.Width / 2, KuangY1);
            g.DrawString("单位名称：（章）", f4, new SolidBrush(Color.Black), 40 + x + 10, KuangY2);
            g.DrawString("法人代表：", f4, new SolidBrush(Color.Black), 40 + x + 10, KuangY3);
            g.DrawString("地    址：", f4, new SolidBrush(Color.Black), 40 + x + 10, KuangY4);
            g.DrawString("电    话：", f4, new SolidBrush(Color.Black), 40 + x + 10, KuangY5);
            g.DrawString("传    真：", f4, new SolidBrush(Color.Black), 40 + x + 10, KuangY6);
            g.DrawString("代 理 人：\n（签 字）", f4, new SolidBrush(Color.Black), 40 + x + 10, KuangY7);
            g.DrawString("开户银行：", f4, new SolidBrush(Color.Black), 40 + x + 10, KuangY8);
            g.DrawString("帐    号：", f4, new SolidBrush(Color.Black), 40 + x + 10, KuangY9);

            fontSize = g.MeasureString("销  货  方", f45);
            g.DrawString("销  货  方", f45, new SolidBrush(Color.Black), 40 + x + recWidth / 2 - fontSize.Width / 2 + recWidth, KuangY1);
            g.DrawString("单位名称：（章）", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, KuangY2);
            g.DrawString("法人代表：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, KuangY3);
            g.DrawString("地    址：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, KuangY4);
            g.DrawString("电    话：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, KuangY5);
            g.DrawString("传    真：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, KuangY6);
            g.DrawString("代 理 人：\n（签 字）", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, KuangY7);
            g.DrawString("开户银行：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, KuangY8);
            g.DrawString("帐    号：", f4, new SolidBrush(Color.Black), 40 + x + 10 + recWidth, KuangY9);
            //框内文字

            g.DrawString(tbHTghfName.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 15, KuangY2);
            g.DrawString(tbHTghfPresenter.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50, KuangY3);
            g.DrawString((tbHTghfAddress.Text.Length > 22) ? tbHTghfAddress.Text.Insert(22, "\n") : tbHTghfAddress.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50, KuangY4);
            g.DrawString(tbHTghfPhone.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50, KuangY5);
            g.DrawString(tbHTghfFax.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50, KuangY6);
            g.DrawString(tbHTghfBankName.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50, KuangY8);
            g.DrawString(tbHTghfBankNumber.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50, KuangY9);

            g.DrawString(tbHTxsfName.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 15 + recWidth, KuangY2);
            g.DrawString(tbHTxsfPresenter.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50 + recWidth, KuangY3);
            g.DrawString((tbHTxsfAddress.Text.Length > 22) ? tbHTxsfAddress.Text.Insert(22, "\n") : tbHTxsfAddress.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50 + recWidth, KuangY4);
            g.DrawString(tbHTxsfPhone.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50 + recWidth, KuangY5);
            g.DrawString(tbHTxsfFax.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50 + recWidth, KuangY6);
            g.DrawString(tbHTxsfBankName.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50 + recWidth, KuangY8);
            g.DrawString(tbHTxsfBankNumber.Text, f4, new SolidBrush(Color.Black), x + recWidth / 2 - 50 + recWidth, KuangY9);

            // 水印
            //g.DrawString("合同版本由唯达软件提供提    ", f6, new SolidBrush(Color.Red), 40, 35);
            //g.DrawString("共1页 一式两份", f6, new SolidBrush(Color.Red), pageWidth / 2 - 50, pageHeight - 35);
            //fontSize = g.MeasureString("专业软件定制 888888888", f6);
            //g.DrawString("专业软件定制 888888888", f6, new SolidBrush(Color.Red), pageWidth - fontSize.Width - 40 + x, 35);
            if (MainWindow.DEGREE > 0)
            {

            }
            else
            {
                //g.DrawString("合同版本由唯达软件系统提供 http://www.vividapp.net/   软件定制电话: 15024345993   QQ: 70269387", f6, new SolidBrush(Color.Red), 40 + x, pageHeight - 90);
                g.DrawString("合同版本由唯达软件系统提供 http://www.vividapp.net/   联系电话: 139-0583-5966   客服QQ: 2424935927", f6, new SolidBrush(Color.Red), 40 + x, 1045 + y);
                //g.DrawString("共1页 一式两份", f6, new SolidBrush(Color.Red), pageWidth / 2 - 50, pageHeight - 35);
                //fontSize = g.MeasureString("专业软件定制 888888888", f6);
            }
            //g.DrawString("专业软件定制 888888888", f6, new SolidBrush(Color.Red), pageWidth - fontSize.Width - 40 + x, pageHeight - 50);
        }
        #endregion

        private void cbHTGoodsNameA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHTGoodsNameA.SelectedIndex != -1)
            {
                if (cbHTGoodsNameA.Text.Equals(""))
                {
                    cbHTGoodsA4.ReadOnly = true;
                    cbHTGoodsA5.ReadOnly = true;
                    cbHTGoodsA7.ReadOnly = true;
                    clearControlValueByList(new List<Control>() { cbHTGoodsA0, cbHTGoodsA1, cbHTGoodsA2, cbHTGoodsA3, cbHTGoodsA4, cbHTGoodsA5, cbHTGoodsA6, cbHTGoodsA7 });
                }
                else if (cbHTGoodsNameA.SelectedIndex == 1)
                {
                    InitPicker(cbHTGoodsNameA, false);
                }
                else
                {
                    cbHTGoodsA4.ReadOnly = false;
                    cbHTGoodsA5.ReadOnly = false;
                    cbHTGoodsA7.ReadOnly = false;
                    HtSetControlsValue(new List<Control>() { cbHTGoodsA0, cbHTGoodsA1, cbHTGoodsA2, cbHTGoodsA3, cbHTGoodsA4 }, cbHTGoodsNameA);
                }
            }
        }

        private void cbHTGoodsNameB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHTGoodsNameB.SelectedIndex != -1)
            {
                if (cbHTGoodsNameB.Text.Equals(""))
                {
                    cbHTGoodsB4.ReadOnly = true;
                    cbHTGoodsB5.ReadOnly = true;
                    cbHTGoodsB7.ReadOnly = true;
                    clearControlValueByList(new List<Control>() { cbHTGoodsB0, cbHTGoodsB1, cbHTGoodsB2, cbHTGoodsB3, cbHTGoodsB4, cbHTGoodsB5, cbHTGoodsB6, cbHTGoodsB7 });
                }
                else if (cbHTGoodsNameB.SelectedIndex == 1)
                {
                    InitPicker(cbHTGoodsNameB, false);
                }
                else
                {
                    cbHTGoodsB4.ReadOnly = false;
                    cbHTGoodsB5.ReadOnly = false;
                    cbHTGoodsB7.ReadOnly = false;
                    HtSetControlsValue(new List<Control>() { cbHTGoodsB0, cbHTGoodsB1, cbHTGoodsB2, cbHTGoodsB3, cbHTGoodsB4 }, cbHTGoodsNameB);
                }
            }
        }

        private void cbHTGoodsNameC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHTGoodsNameC.SelectedIndex != -1)
            {
                if (cbHTGoodsNameC.Text.Equals(""))
                {
                    cbHTGoodsC4.ReadOnly = true;
                    cbHTGoodsC5.ReadOnly = true;
                    cbHTGoodsC7.ReadOnly = true;
                    clearControlValueByList(new List<Control>() { cbHTGoodsC0, cbHTGoodsC1, cbHTGoodsC2, cbHTGoodsC3, cbHTGoodsC4, cbHTGoodsC5, cbHTGoodsC6, cbHTGoodsC7 });
                }
                else if (cbHTGoodsNameC.SelectedIndex == 1)
                {
                    InitPicker(cbHTGoodsNameC, false);
                }
                else
                {
                    cbHTGoodsC4.ReadOnly = false;
                    cbHTGoodsC5.ReadOnly = false;
                    cbHTGoodsC7.ReadOnly = false;
                    HtSetControlsValue(new List<Control>() { cbHTGoodsC0, cbHTGoodsC1, cbHTGoodsC2, cbHTGoodsC3, cbHTGoodsC4 }, cbHTGoodsNameC);
                }
            }
        }

        private void cbHTGoodsNameD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHTGoodsNameD.SelectedIndex != -1)
            {
                if (cbHTGoodsNameD.Text.Equals(""))
                {
                    cbHTGoodsD4.ReadOnly = true;
                    cbHTGoodsD5.ReadOnly = true;
                    cbHTGoodsD7.ReadOnly = true;
                    clearControlValueByList(new List<Control>() { cbHTGoodsD0, cbHTGoodsD1, cbHTGoodsD2, cbHTGoodsD3, cbHTGoodsD4, cbHTGoodsD5, cbHTGoodsD6, cbHTGoodsD7 });
                }
                else if (cbHTGoodsNameD.SelectedIndex == 1)
                {
                    InitPicker(cbHTGoodsNameD, false);
                }
                else
                {
                    cbHTGoodsD4.ReadOnly = false;
                    cbHTGoodsD5.ReadOnly = false;
                    cbHTGoodsD7.ReadOnly = false;
                    HtSetControlsValue(new List<Control>() { cbHTGoodsD0, cbHTGoodsD1, cbHTGoodsD2, cbHTGoodsD3, cbHTGoodsD4 }, cbHTGoodsNameD);
                }
            }
        }

        private void cbHTGoodsNameE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHTGoodsNameE.SelectedIndex != -1)
            {
                if (cbHTGoodsNameE.Text.Equals(""))
                {
                    cbHTGoodsE4.ReadOnly = true;
                    cbHTGoodsE5.ReadOnly = true;
                    cbHTGoodsE7.ReadOnly = true;
                    clearControlValueByList(new List<Control>() { cbHTGoodsE0, cbHTGoodsE1, cbHTGoodsE2, cbHTGoodsE3, cbHTGoodsE4, cbHTGoodsE5, cbHTGoodsE6, cbHTGoodsE7 });
                }
                else if (cbHTGoodsNameE.SelectedIndex == 1)
                {
                    InitPicker(cbHTGoodsNameE, false);
                }
                else
                {
                    cbHTGoodsE4.ReadOnly = false;
                    cbHTGoodsE5.ReadOnly = false;
                    cbHTGoodsE7.ReadOnly = false;
                    HtSetControlsValue(new List<Control>() { cbHTGoodsE0, cbHTGoodsE1, cbHTGoodsE2, cbHTGoodsE3, cbHTGoodsE4 }, cbHTGoodsNameE);
                }
            }
        }

        private void cbHTGoodsA5_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(cbHTGoodsA4, cbHTGoodsA5, cbHTGoodsA6);
        }

        private void cbHTGoodsB5_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(cbHTGoodsB4, cbHTGoodsB5, cbHTGoodsB6);
        }

        private void cbHTGoodsC5_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(cbHTGoodsC4, cbHTGoodsC5, cbHTGoodsC6);
        }

        private void cbHTGoodsD5_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(cbHTGoodsD4, cbHTGoodsD5, cbHTGoodsD6);
        }

        private void cbHTGoodsE5_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(cbHTGoodsE4, cbHTGoodsE5, cbHTGoodsE6);
        }

        private void tbGoods11_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(tbGoods11, tbGoods12, tbGoods13);
            tbGoods14.Text = tbGoods11.Text;
        }

        private void tbGoods12_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(tbGoods11, tbGoods12, tbGoods13);
        }

        private void tbGoods15_TextChanged(object sender, EventArgs e)
        {
            calculateSmallSum(tbGoods14, tbGoods15, tbGoods16);
        }

        ToolTip tbClient2Tooltip = new ToolTip();
        private void tbClient2_MouseEnter(object sender, EventArgs e)
        {
            tbClient2Tooltip.Show("拥有管账宝帐号的用户可以相互远程签单!", tbClient2);
        }

        private void tbClient2_MouseLeave(object sender, EventArgs e)
        {
            tbClient2Tooltip.Hide(tbClient2);
        }

        private void tbDz3_TextChanged(object sender, EventArgs e)
        {
            if (danziComboBox.SelectedIndex == 1)
            {
                if (tbDz3.Text.Contains('￥'))
                {
                    tbDz9.Text = tbDz3.Text.Split('￥')[1];
                }
                else
                {
                    tbDz9.Text = tbDz3.Text;
                }
            }
            else
            {
                tbDz9.Text = "";
            }
        }

        private void remoteSignButton_Click(object sender, EventArgs e)
        {
            // 远程签单
            //  if (remoteSignButton.Enabled)
            {
                if (MessageBox.Show("已检测到对方客户也是管账宝会员用户,是否确认发送远程签单请求?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    ConfirmPassword cp = new ConfirmPassword();
                    if (cp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {

                    }
                }
            }
        }

        private void tbDz10_TextChanged(object sender, EventArgs e)
        {
            tbDz11.Text = (float.Parse(tbDz8.Text.Equals("") ? "0" : tbDz8.Text) + float.Parse(tbDz9.Text.Equals("") ? "0" : tbDz9.Text) - float.Parse(tbDz10.Text.Equals("") ? "0" : tbDz10.Text)).ToString();
        }

        private void tbDz9_TextChanged(object sender, EventArgs e)
        {
            tbDz11.Text = (float.Parse(tbDz8.Text.Equals("") ? "0" : tbDz8.Text) + float.Parse(tbDz9.Text.Equals("") ? "0" : tbDz9.Text) - float.Parse(tbDz10.Text.Equals("") ? "0" : tbDz10.Text)).ToString();
        }

        private void tbDz8_TextChanged(object sender, EventArgs e)
        {
            if (danziComboBox.SelectedIndex == 0)
            {
                tbDz8.Text = "";
            }
        }

    }
}
