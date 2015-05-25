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
        public String gzbIDStirng = "";
        public String remoteSignId = "";
        public String companyNameStirng = "";
        public Image signImage;
        public Boolean isSigned = false;

        public BillSign()
        {
            InitializeComponent();
        }

        private void BillSign_Load(object sender, EventArgs e)
        {
            if (signImage != null)
            {
                SignPictureBox.Image = signImage;
            }

            if (isSendRequest)
            {
                OKButton.Text = "发送请求";
                RefuseButton.Text = "取消";
                Image _signImage = FormBasicFeatrues.GetInstence().Base64StringToImage(FormBasicFeatrues.GetInstence().DecompressString(FormBasicFeatrues.GetInstence().DecompressString(MainWindow.SIGNATURE)));
                if (_signImage != null)
                {
                    using (Graphics gr = Graphics.FromImage(SignPictureBox.Image))
                    {
                        gr.DrawImage(_signImage, new Rectangle(420, 480, 104, 36));
                    }
                    SignPictureBox.Invalidate();
                }
            }
            else
            {
                OKButton.Text = "确认签名";
                RefuseButton.Text = "拒绝";
            }

            if (isSigned || gzbIDStirng.Equals(MainWindow.USER_ID))
            {
                OKButton.Enabled = false;
                RefuseButton.Enabled = false;
            }
            else
            {
                OKButton.Enabled = true;
                RefuseButton.Enabled = true;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (isSendRequest)
            {// 发送请求
                if (DatabaseConnections.GetInstence().OnlineInsertData("gzb_remotesign",
                    "fromGZBID,toGZBID,companyNickName,sendTime,signValue",
                    "'" + MainWindow.USER_ID + "','" + gzbIDStirng + "','" + companyNameStirng + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + FormBasicFeatrues.GetInstence().CompressString(FormBasicFeatrues.GetInstence().CompressString(FormBasicFeatrues.GetInstence().ImgToBase64String(new Bitmap(signImage)))) + "'") > 0)
                {
                    MessageBox.Show("发送请求成功!", "提示");
                    //this.Close();
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else
            {// 确认签名
                ConfirmPassword cp = new ConfirmPassword();
                if (cp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Image _signImage = FormBasicFeatrues.GetInstence().Base64StringToImage(FormBasicFeatrues.GetInstence().DecompressString(FormBasicFeatrues.GetInstence().DecompressString(MainWindow.SIGNATURE)));
                    if (_signImage != null)
                    {
                        using (Graphics gr = Graphics.FromImage(SignPictureBox.Image))
                        {
                            gr.DrawImage(_signImage, new Rectangle(150, 480, 104, 36));
                        }
                        SignPictureBox.Invalidate();
                        if (DatabaseConnections.GetInstence().OnlineUpdateData("gzb_remotesign", new String[] { "isSigned", "signTime", "signValue" }, new String[] { "1", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), FormBasicFeatrues.GetInstence().CompressString(FormBasicFeatrues.GetInstence().CompressString(FormBasicFeatrues.GetInstence().ImgToBase64String(new Bitmap(SignPictureBox.Image)))) }, remoteSignId) > 0)
                        {
                            OKButton.Enabled = false;
                            RefuseButton.Enabled = false;
                            MessageBox.Show("远程签名成功!", "提示");
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                    }
                }
            }
        }



        private void RefuseButton_Click(object sender, EventArgs e)
        {
            if (isSendRequest)
            {// 发送请求
                this.Close();
            }
            else
            {// 
                InputMessage im = new InputMessage();
                if (im.ShowDialog() == DialogResult.OK)
                {
                    if (DatabaseConnections.GetInstence().OnlineUpdateData("gzb_remotesign", new String[] { "isSigned", "refusedMessage", "signTime" }, new String[] { "-1", im.detailMessage, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }, remoteSignId) > 0)
                    {
                        MessageBox.Show("发送拒签成功!", "提示");
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                }
            }
        }


    }
}
