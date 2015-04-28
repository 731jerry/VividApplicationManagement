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
        public String gzbIDStirng = "";
        public String companyNickNameStirng = "";
        public String remoteSignId = "";

        public BillSign()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isSendSign)
            {// 发送请求
                if (DatabaseConnections.GetInstence().OnlineInsertData("gzb_remotesign",
                    "fromGZBID,toGZBID,companyNickName,sendTime,signValue",
                    "'" + MainWindow.USER_ID + "','" + gzbIDStirng + "','" + companyNickNameStirng + "','" + DateTime.Now + "','" + FormBasicFeatrues.GetInstence().ImgToBase64String(new Bitmap(signImage)) + "'") > 0)
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
                    if (DatabaseConnections.GetInstence().OnlineUpdateData("remoteSign", new String[] { "isSigned" }, new String[] { "1" }, remoteSignId) > 0)
                    {
                        MessageBox.Show("远程签名成功!", "提示");
                        this.Close();
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
