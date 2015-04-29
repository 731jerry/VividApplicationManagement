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
        public Boolean isSendRequest = false;
        public String remoteSignId = "";
        public String gzbIDStirng = "";
        public String companyNickNameStirng = "";
        public Image signImage;
        public Boolean isSigned = false;

        public BillSign()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isSendRequest)
            {// 发送请求
                if (DatabaseConnections.GetInstence().OnlineInsertData("gzb_remotesign",
                    "fromGZBID,toGZBID,companyNickName,sendTime,signValue",
                    "'" + MainWindow.USER_ID + "','" + gzbIDStirng + "','" + companyNickNameStirng + "','" + DateTime.Now + "','" + FormBasicFeatrues.GetInstence().CompressString(FormBasicFeatrues.GetInstence().ImgToBase64String(new Bitmap(signImage))) + "'") > 0)
                {
                    MessageBox.Show("发送请求成功!", "提示");
                    this.Close();
                }
            }
            else
            {// 确认签名
                ConfirmPassword cp = new ConfirmPassword();
                if (cp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (DatabaseConnections.GetInstence().OnlineUpdateData("gzb_remotesign", new String[] { "isSigned", "signTime" }, new String[] { "1", DateTime.Now.ToString() }, remoteSignId) > 0)
                    {
                        MessageBox.Show("远程签名成功!", "提示");
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                }
            }
        }

        private void BillSign_Load(object sender, EventArgs e)
        {
            if (signImage != null)
            {
                SignPictureBox.Image = signImage;
            }

            if (isSendRequest)
            {
                btnSave.Text = "发送请求";
            }
            else
            {
                btnSave.Text = "确认签名";
            }

            if (isSigned)
            {
                btnSave.Enabled = false;
            }
            else {
                btnSave.Enabled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
