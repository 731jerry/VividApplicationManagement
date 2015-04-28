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
    public partial class BillSign : Form
    {
        public Image signImage;

        public BillSign()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ConfirmPassword cp = new ConfirmPassword();
            if (cp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //cp.password

            }
        }

        private void BillSign_Load(object sender, EventArgs e)
        {
            if (signImage != null)
            {
                SignPictureBox.Image = signImage;
            }
        }
    }
}