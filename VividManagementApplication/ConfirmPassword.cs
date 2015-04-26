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
    public partial class ConfirmPassword : Form
    {
        public String password = "";
        public ConfirmPassword()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            password = confirmPasswordText.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
