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
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void ChangePasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PasswordGroupBox.Enabled = ChangePasswordCheckBox.Checked;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePasswordQQButton_Click(object sender, EventArgs e)
        {
            if (FormBasicFeatrues.GetInstence().isPassValidateControls(new List<Control>() { tbInfo1, tbInfo2, tbInfo3, tbInfo4, tbInfo5, tbInfo6, tbInfo7, tbInfo8, tbInfo9}))
            {

            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {

        }

        private void CancelQQButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
