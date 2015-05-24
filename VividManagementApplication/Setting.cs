using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
namespace VividManagementApplication
{
    public partial class Setting : Form
    {
        private Boolean isChangedPassword = false;
        private Boolean isChangedSignature = false;

        public Setting()
        {
            InitializeComponent();
            tbInfo1.Text = MainWindow.COMPANY_NAME;
            tbInfo2.Text = MainWindow.COMPANY_NICKNAME;
            tbInfo3.Text = MainWindow.PHONE;
            tbInfo4.Text = MainWindow.EMAIL;
            tbInfo5.Text = MainWindow.FAX;
            tbInfo6.Text = MainWindow.ADDRESS;
            tbInfo7.Text = MainWindow.BANK_NAME;
            tbInfo8.Text = MainWindow.BANK_CARD;
            tbInfo9.Text = MainWindow.COMPANY_OWNER;
            tbInfo10.Text = MainWindow.QQ;
            if (!MainWindow.SIGNATURE.Equals(""))
            {
                compressedBitmap = FormBasicFeatrues.GetInstence().Base64StringToImage(FormBasicFeatrues.GetInstence().DecompressString(FormBasicFeatrues.GetInstence().DecompressString(MainWindow.SIGNATURE)));
                //Console.WriteLine("之前:" + MainWindow.SIGNATURE);
                //Console.WriteLine("之后:" + ImgToBase64String(compressedBitmap));
                SignPictureShowBox.Image = compressedBitmap;
            }
        }

        private void ChangePasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (isChangedPassword)
            {
                ChangePasswordCheckBox.Checked = false;
                ChangePasswordLabel.Text = "您的密码已经被修改, 需要重启软件才能再修改密码!";
                //MessageBox.Show("您的密码已经被修改, 需要重启软件才能再一次修改密码!", "无法修改密码");
            }
            else
            {
                PasswordGroupBox.Enabled = ChangePasswordCheckBox.Checked;
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePasswordQQButton_Click(object sender, EventArgs e)
        {
            if (FormBasicFeatrues.GetInstence().GetMd5Hash(MD5.Create(), OldPasswordTextBox.Text).Equals(MainWindow.PASSWORD_HASH))
            {
                if (NewPasswordTextBox.Text.Equals(NewPasswordTextBox2.Text))
                {
                    try
                    {
                        if (DatabaseConnections.GetInstence().OnlineUpdateData(
                                                                              "users",
                                                                              new string[] { "password" },
                                                                              new string[] { FormBasicFeatrues.GetInstence().GetMd5Hash(MD5.Create(), NewPasswordTextBox.Text) },
                                                                              MainWindow.ID) > 0)
                        {
                            OldPasswordTextBox.Clear();
                            NewPasswordTextBox.Clear();
                            NewPasswordTextBox2.Clear();
                            MessageBox.Show("修改密码成功!下次登录时候您将需要使用新密码!", "成功");
                            //MainWindow.PASSWORD_HASH = FormBasicFeatrues.GetInstence().GetMd5Hash(MD5.Create(), NewPasswordTextBox.Text);
                            ChangePasswordCheckBox.Checked = false;
                            isChangedPassword = true;
                        }
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message, "密码修改错误");
                        //throw;
                    }
                }
                else
                {
                    MessageBox.Show("两次密码不同!", "错误");
                    NewPasswordTextBox.Clear();
                    NewPasswordTextBox2.Clear();
                }
            }
            else
            {
                MessageBox.Show("旧密码输入错误!", "错误");
                OldPasswordTextBox.Clear();
                NewPasswordTextBox.Clear();
                NewPasswordTextBox2.Clear();
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (FormBasicFeatrues.GetInstence().isPassValidateControls(new List<Control>() { tbInfo1, tbInfo2, tbInfo3, tbInfo4, tbInfo5, tbInfo6, tbInfo7, tbInfo8, tbInfo9 }))
            {
                try
                {
                    if (DatabaseConnections.GetInstence().OnlineUpdateData(
                                                                            "users",
                                                                            new string[] { "company", "companyNickname", "phone", "email", "fax", "address", "bankname", "bankcard", "companyowner", "QQ" },
                                                                            new string[] { tbInfo1.Text, tbInfo2.Text, tbInfo3.Text, tbInfo4.Text, tbInfo5.Text, tbInfo6.Text, tbInfo7.Text, tbInfo8.Text, tbInfo9.Text, tbInfo10.Text },
                                                                            MainWindow.ID) > 0)
                    {
                        MessageBox.Show("修改用户信息成功!", "成功!");
                        MainWindow.COMPANY_NAME = tbInfo1.Text;
                        MainWindow.COMPANY_NICKNAME = tbInfo2.Text;
                        MainWindow.PHONE = tbInfo3.Text;
                        MainWindow.EMAIL = tbInfo4.Text;
                        MainWindow.FAX = tbInfo5.Text;
                        MainWindow.ADDRESS = tbInfo6.Text;
                        MainWindow.BANK_NAME = tbInfo7.Text;
                        MainWindow.BANK_CARD = tbInfo8.Text;
                        MainWindow.COMPANY_OWNER = tbInfo9.Text;
                        MainWindow.QQ = tbInfo10.Text;
                    }

                    if (isChangedSignature)
                    {
                        //GZB_signature
                        String signatureString = FormBasicFeatrues.GetInstence().CompressString(FormBasicFeatrues.GetInstence().CompressString(FormBasicFeatrues.GetInstence().ImgToBase64String(compressedBitmap)));
                        if (!signatureString.Equals(""))
                        {
                            if (DatabaseConnections.GetInstence().OnlineUpdateData("users", new string[] { "GZB_signature" }, new string[] { signatureString }, MainWindow.ID) > 0)
                            {
                                MainWindow.SIGNATURE = signatureString;
                            }
                        }
                    }
                    this.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "用户信息修改错误");
                    //throw;
                }

            }
            else
            {
                MessageBox.Show("请先填入必填项(带*项目)!", "提示");
            }
        }

        private void CancelQQButton_Click(object sender, EventArgs e)
        {
            if (FormBasicFeatrues.GetInstence().isPassValidateControls(new List<Control>() { tbInfo1, tbInfo2, tbInfo3, tbInfo4, tbInfo5, tbInfo6, tbInfo7, tbInfo8, tbInfo9 }))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("请先填入必填项(带*项目)!", "提示");
            }
        }

        #region 电子签名

        Bitmap compressedBitmap;

        private void SignPictureBox_Paint(object sender, PaintEventArgs e)
        {
            //if (bitmap != null)
            //{
            //    e.Graphics.DrawImage(bitmap, 0, 0, this.SignPictureShowBox.Width, this.SignPictureShowBox.Height);
            //    this.SignPictureShowBox.Image = bitmap;
            //    //byte[] temp = BitmapToBytes(bitmap);
            //    bitmap = null;
            //}
        }

        private void SignqqButton_Click(object sender, EventArgs e)
        {
            // 如果未签名则开启签名
            Signature fm = new Signature();
            if (fm.ShowDialog() == DialogResult.OK)
            {
                isChangedSignature = true;
                // 可以用来保存Bitmap
                compressedBitmap = new Bitmap(fm.SavedBitmap, new Size(this.SignPictureShowBox.Width, this.SignPictureShowBox.Height));
                // this.SignPictureShowBox.Invalidate();
                this.SignPictureShowBox.Image = compressedBitmap;
            }
        }

        #endregion

    }
}
