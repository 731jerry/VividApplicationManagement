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

namespace VividManagementApplication
{
    public partial class Setting : Form
    {
        private Boolean isChangedPassword = false;
        public Setting()
        {
            InitializeComponent();
            tbInfo1.Text = MainWindow.COMPANY_NAME;
            tbInfo2.Text = MainWindow.REAL_NAME;
            tbInfo3.Text = MainWindow.PHONE;
            tbInfo4.Text = MainWindow.EMAIL;
            tbInfo5.Text = MainWindow.FAX;
            tbInfo6.Text = MainWindow.ADDRESS;
            tbInfo7.Text = MainWindow.BANK_NAME;
            tbInfo8.Text = MainWindow.BANK_CARD;
            tbInfo9.Text = MainWindow.COMPANY_OWNER;
            tbInfo10.Text = MainWindow.QQ;

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
                        DatabaseConnections.GetInstence().OnlineUpdateData(
                                                                            "users",
                                                                            new string[] { "password" },
                                                                            new string[] { FormBasicFeatrues.GetInstence().GetMd5Hash(MD5.Create(), NewPasswordTextBox.Text) },
                                                                            MainWindow.ID);
                        OldPasswordTextBox.Clear();
                        NewPasswordTextBox.Clear();
                        NewPasswordTextBox2.Clear();
                        MessageBox.Show("修改密码成功!下次登录时候您将需要使用新密码!", "成功");
                        //MainWindow.PASSWORD_HASH = FormBasicFeatrues.GetInstence().GetMd5Hash(MD5.Create(), NewPasswordTextBox.Text);
                        ChangePasswordCheckBox.Checked = false;
                        isChangedPassword = true;
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
                    DatabaseConnections.GetInstence().OnlineUpdateData(
                                                                           "users",
                                                                           new string[] { "company", "realname", "phone", "email", "fax", "address", "bankname", "bankcard", "companyowner", "QQ" },
                                                                           new string[] { tbInfo1.Text, tbInfo2.Text, tbInfo3.Text, tbInfo4.Text, tbInfo5.Text, tbInfo6.Text, tbInfo7.Text, tbInfo8.Text, tbInfo9.Text, tbInfo10.Text },
                                                                           MainWindow.ID);
                    MessageBox.Show("修改用户信息成功!", "成功!");
                    MainWindow.COMPANY_NAME = tbInfo1.Text;
                    MainWindow.REAL_NAME = tbInfo2.Text;
                    MainWindow.PHONE = tbInfo3.Text;
                    MainWindow.EMAIL = tbInfo4.Text;
                    MainWindow.FAX = tbInfo5.Text;
                    MainWindow.ADDRESS = tbInfo6.Text;
                    MainWindow.BANK_NAME = tbInfo7.Text;
                    MainWindow.BANK_CARD = tbInfo8.Text;
                    MainWindow.COMPANY_OWNER = tbInfo9.Text;
                    MainWindow.QQ = tbInfo10.Text;
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
                MessageBox.Show("请先填入必填项(带*项目)!", "错误");
            }
        }

        private void CancelQQButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
