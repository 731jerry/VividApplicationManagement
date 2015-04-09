﻿using System;
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
        public int FilterOKClickedIndex = -1; // Filter窗口

        public Filter()
        {
            InitializeComponent();
        }

        private void Filter_Load(object sender, EventArgs e)
        {
            fromDate.Text = DateTime.Now.ToShortDateString();
            toDate.Text = DateTime.Now.ToShortDateString();

            addItemsToCombox(DatabaseConnections.GetInstence().LocalGetIdsOfTable("clients", "clientID", " ORDER BY id ASC "), clientID);
            clientID.SelectedIndex = 0;

        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            FilterOKClickedIndex = 1;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            FilterOKClickedIndex = 0;
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
            clientName.Items.Clear();
            FormBasicFeatrues.GetInstence().SetControlsVauleByControlList(new List<Control>() { clientName }, DatabaseConnections.GetInstence().LocalGetOneRowDataById("clients", new String[] { "company" }, "clientID", clientID.Text).ToList<String>());
            clientName.SelectedIndex = 0;
        }

    }
}