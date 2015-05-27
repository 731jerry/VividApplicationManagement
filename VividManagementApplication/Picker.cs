using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VividManagementApplication
{
    public partial class Picker : Form
    {
        public Boolean isClient = false;
        public Picker()
        {
            InitializeComponent();
        }

        DataGridViewTextBoxColumn Column1 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column2 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column3 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column4 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column5 = new DataGridViewTextBoxColumn();

        public String selectedClientID = "";
        private void Picker_Load(object sender, EventArgs e)
        {
            if (isClient)
            {
                Column1.HeaderText = "客户编号";
                Column2.HeaderText = "客户类型";
                Column3.HeaderText = "公司名称";
                Column4.HeaderText = "联系人";
                Column5.HeaderText = "联系电话";

                Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                Column3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 }, "clients", -1,
                    new string[] { "clientID", 
                    "case when type = '0' then '供应商' when type = '1' then '销售商' else '其他' end as 'type'", 
                    "company", "companyOwner", "phone" });
            }
            else
            {
                Column1.HeaderText = "商品编号";
                Column2.HeaderText = "商品名称";
                Column3.HeaderText = "商品等级";
                Column4.HeaderText = "商品规格";
                Column5.HeaderText = "当期存量";

                Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                Column2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                CreateMainDataGridView(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 }, "goods", -1,
                    new string[] { "goodId", "name", "dengji", "guige", "currentCount" });
            }
        }

        private void CreateMainDataGridView(DataGridViewColumn[] dgvcArray, string table, int discardFlagIndex, string[] queryArray)
        {
            string order = " ORDER BY id ASC ";

            this.PickerDataGridView.Columns.AddRange(dgvcArray);
            List<string[]> resultsList = DatabaseConnections.LocalConnector().LocalGetData(table, queryArray, order);
            for (int i = 0; i < resultsList.Count; i++)
            {
                this.PickerDataGridView.Rows.Add(resultsList[i]);
            }
        }

        private void PickerDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                btnOK.PerformClick();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.PickerDataGridView.Rows.Count > 0)
            {
                selectedClientID = this.PickerDataGridView.SelectedRows[0].Cells[0].Value.ToString();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

    }
}
