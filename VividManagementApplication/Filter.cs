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
    public partial class Filter : Form
    {
        public Filter()
        {
            InitializeComponent();
        }

        private void Filter_Load(object sender, EventArgs e)
        {
            fromDate.Text = DateTime.Now.AddDays(-7).ToShortDateString();
            toDate.Text = DateTime.Now.ToShortDateString();
            DzTypeComboBox.SelectedIndex = 0;

            clientID.Items.Add("");
            clientID.Items.Insert(1, "使用选择器...");

            addItemsToCombox(DatabaseConnections.LocalConnector().LocalGetIdsOfTable("clients", "clientID", " ORDER BY id ASC "), clientID);
            if (clientID.Items.Count > 2)
            {
                clientID.SelectedIndex = 2;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addItemsToCombox(List<String> items, ComboBox cb)
        {
            foreach (string item in items)
            {
                cb.Items.Add(item);
            }
        }

        private void clientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientID.SelectedIndex != -1)
            {
                if (clientID.Text.Equals(""))
                {
                    clientName.Clear();
                }
                else if (clientID.SelectedIndex == 1)
                {
                    Picker cp = new Picker();
                    cp.isClient = true;
                    if (cp.ShowDialog() == DialogResult.OK)
                    {
                        clientID.Text = cp.selectedClientID;
                    }
                    else
                    {
                        clientID.SelectedIndex = 0;
                    }
                }
                else
                {
                    FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(new List<Control>() { clientName }, DatabaseConnections.LocalConnector().LocalGetOneRowDataById("clients", new String[] { "company" }, "clientID", clientID.Text).ToList<String>());
                }
            }
        }

    }
}
