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
    public partial class BillSign : Form
    {
        public Image signImage;
        public Boolean isSendSign = false;

        public BillSign()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isSendSign)
            {// 发送请求

            }
            else
            {// 确认签名
                ConfirmPassword cp = new ConfirmPassword();
                if (cp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //cp.password
                }
            }
        }

        private void BillSign_Load(object sender, EventArgs e)
        {
            if (signImage != null)
            {
                SignPictureBox.Image = signImage;
            }

            if (isSendSign)
            {
                btnSave.Text = "发送请求";
            }
            else
            {
                btnSave.Text = "确认签名";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
