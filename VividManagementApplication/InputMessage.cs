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
    public partial class InputMessage : Form
    {
        public String detailMessage;
        public InputMessage()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (detailedMessageTextbox.Text.Equals(""))
            {
                MessageBox.Show("填入的信息不能为空！", "提示");
            }
            else
            {
                detailMessage = detailedMessageTextbox.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
