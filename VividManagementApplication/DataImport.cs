﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Threading;
using System.Data.OleDb;

namespace VividManagementApplication
{
    public partial class DataImport : Form
    {
        public Boolean isTypeCx = false;
        public int importedCount = 0;
        private int colCount = 0;
        private String table = "";
        private String[] queryArray;
        private String baseName = "";

        public DataImport()
        {
            InitializeComponent();
        }

        private void DataImport_Load(object sender, EventArgs e)
        {
            //importingProgressBar.Style = ProgressBarStyle.Marquee;
            //importingProgressBar.Enabled = false;
            importingProgressBar.Style = ProgressBarStyle.Continuous;
            importingProgressBar.Minimum = 0;
            importingProgressBar.Maximum = 100;
            if (isTypeCx)
            {
                this.Text = "正在导入客户数据...";
                colCount = 9;
                table = "clients";
                baseName = "clientID";
                queryArray = new String[] { baseName, "gzbID", "type", "company", "companyOwner", "address", "phone", "bankName", "bankCard", "beizhu", "addtime" };
            }
            else
            {
                this.Text = "正在导入商品数据...";
                colCount = 8;
                table = "goods";
                baseName = "goodID";
                queryArray = new String[] { baseName, "name", "dengji", "guige", "unit", "initalCount", "purchasePrice", "purchaseTotal", "currentCount", "currntsalesPrice", "currentTotal", "beizhu", "addtime" };
            }
        }

        private void selectFileButton_Click(object sender, EventArgs e)
        {
            if (selectFileButton.Text.Equals("浏览"))
            {   // 浏览目录
                OpenFileDialog ofg = new OpenFileDialog();
                ofg.Filter = "表格文件(*.xls)|*.xls";
                if (ofg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FilePathTextBox.Text = ofg.FileName;
                    selectFileButton.Text = "导入";
                }
            }
            else
            { // 导入文件
                try
                {
                    String strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + FilePathTextBox.Text + ";" + "Extended Properties=Excel 8.0;";
                    using (OleDbConnection conn = new OleDbConnection(strConn))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        System.Data.DataTable sheetDt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        String[] sheet = new String[sheetDt.Rows.Count];
                        for (int i = 0; i < sheetDt.Rows.Count; i++)
                        {
                            sheet[i] = sheetDt.Rows[i]["TABLE_NAME"].ToString();
                        }
                        //OleDbDataAdapter da = new OleDbDataAdapter("Select * From [Sheet1$]", conn);
                        String strExcel = String.Format("select * from [{0}]", sheet[0]);
                        OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel, strConn);
                        DataTable dt = new DataTable();
                        myCommand.Fill(dt);
                        conn.Close();

                        if ((dt.Rows.Count > 0) && (dt.Columns.Count == colCount))
                        //if ((dt.Rows.Count > 0))
                        {
                            selectFileButton.Enabled = false;
                            Thread tt = new Thread(new ParameterizedThreadStart(importToSqliteWithObject));
                            //tt.IsBackground = true;
                            tt.Start(dt);
                            tt.DisableComObjectEagerCleanup();
                            //importToSqlite(dt);
                        }
                        else
                        {
                            MessageBox.Show("数据错误，请重试！", "提示");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据错误，请重试！" + ex.Message, "提示");
                }
            }
        }

        private void importToSqliteWithObject(Object obj)
        {
            importToSqlite(obj as DataTable);
        }

        delegate void SetImportingProgressBarCallback(int percentage);
        private void SetImportingProgressBar(int percentage)
        {
            if (this.importingProgressBar.InvokeRequired)
            {
                SetImportingProgressBarCallback d = new SetImportingProgressBarCallback(SetImportingProgressBar);
                this.Invoke(d, new object[] {percentage });
            }
            else
            {
                //Console.WriteLine(percentage);
                this.importingProgressBar.Value = percentage;
            }
        }

        private void importToSqlite(DataTable dt)
        {
            // importingProgressBar.Value = 0;
            // importingProgressBar.Enabled = true;
            //SetImportingProgressBar(0);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //importingProgressBar.Value = ((i + 1) / dt.Rows.Count) * 100;
                SetImportingProgressBar((((i + 1) * 100) / dt.Rows.Count));

                List<String> arrayList = new List<String>();
                if (isTypeCx)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        arrayList.Add("'" + dt.Rows[i][j].ToString() + "'");
                    }
                }
                else
                {
                    arrayList.Add("'" + dt.Rows[i][0].ToString() + "'");
                    arrayList.Add("'" + dt.Rows[i][1].ToString() + "'");
                    arrayList.Add("'" + dt.Rows[i][2].ToString() + "'");
                    arrayList.Add("'" + dt.Rows[i][3].ToString() + "'");
                    arrayList.Add("'" + dt.Rows[i][4].ToString() + "'");
                    arrayList.Add("'" + dt.Rows[i][5].ToString() + "'");
                    arrayList.Add("'" + checkNumberValue(dt.Rows[i][4].ToString()) * checkNumberValue(dt.Rows[i][5].ToString()) + "'");
                    arrayList.Add("'" + dt.Rows[i][4].ToString() + "'");
                    arrayList.Add("'" + dt.Rows[i][6].ToString() + "'");
                    arrayList.Add("'" + checkNumberValue(dt.Rows[i][4].ToString()) * checkNumberValue(dt.Rows[i][6].ToString()) + "'");
                    arrayList.Add("'" + dt.Rows[i][7].ToString() + "'");
                }
                arrayList.Insert(0, "'" + DatabaseConnections.Connector.LocalAutoincreaseID(table, baseName) + "'");
                arrayList.Insert(arrayList.Count, "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                DatabaseConnections.Connector.LocalInsertDataReturnAffectRows(table, String.Join(",", queryArray).ToString(), String.Join(",", arrayList));
                importedCount++;
            }
            // importingProgressBar.Value = 100;
            SetImportingProgressBar(100);
            selectFileButton.Enabled = true;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private float checkNumberValue(String str)
        {
            str = str.Trim();
            float result = 0.0f;
            if (!str.Equals(""))
            {
                if (FormBasicFeatrues.GetInstence().IsNumeric(str))
                {
                    result = float.Parse(str);
                }
            }
            return result;
        }


    }
}
