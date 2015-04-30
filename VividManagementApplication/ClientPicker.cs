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
    public partial class ClientPicker : Form
    {
        public ClientPicker()
        {
            InitializeComponent();
        }

        DataGridViewTextBoxColumn Column1 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column2 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column3 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column4 = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn Column5 = new DataGridViewTextBoxColumn();

        public String selectedClientID = "";
        private void ClientPicker_Load(object sender, EventArgs e)
        {
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

        private void CreateMainDataGridView(DataGridViewColumn[] dgvcArray, string table, int discardFlagIndex, string[] queryArray)
        {
            string order = " ORDER BY id ASC ";

            this.ClientPickerDataGridView.Columns.AddRange(dgvcArray);
            List<string[]> resultsList = DatabaseConnections.GetInstence().LocalGetData(table, queryArray, order);
            for (int i = 0; i < resultsList.Count; i++)
            {
                this.ClientPickerDataGridView.Rows.Add(resultsList[i]);
            }
        }

        private void ClientPickerDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1) {
                btnOK.PerformClick();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.ClientPickerDataGridView.Rows.Count > 0)
            {
                selectedClientID = this.ClientPickerDataGridView.SelectedRows[0].Cells[0].Value.ToString();
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
