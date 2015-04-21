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

        #region 电子签名
        
        Bitmap bitmap;

        private void SignPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (bitmap != null)
            {
                e.Graphics.DrawImage(bitmap, 0, 0, this.SignPictureBox.Width, this.SignPictureBox.Height);
                this.SignPictureBox.Image = bitmap;
                //byte[] temp = BitmapToBytes(bitmap);

            }
        }

        private void SignqqButton_Click(object sender, EventArgs e)
        {
            // 如果未签名则开启签名
            Signature fm = new Signature();
            if (fm.ShowDialog() == DialogResult.OK)
            {
                bitmap = fm.SavedBitmap;
                // 可以用来保存Bitmap
                //Bitmap compressedBitmap = new Bitmap(bitmap, new Size(this.SignPictureBox.Width, this.SignPictureBox.Height));
                //Console.WriteLine(ImgToBase64String(compressedBitmap));
                this.SignPictureBox.Invalidate();
            }
        }

        //图片 转为    base64编码的文本
        private String ImgToBase64String(Bitmap btmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                btmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);
                return strbaser64;
            }
            catch (Exception ex)
            {
                return ("ImgToBase64String 转换失败\nException:" + ex.Message);
            }
        }

        //base64编码的文本 转为    图片
        private Bitmap Base64StringToImage(String inputStr)
        {
            Bitmap btmp = null;
            try
            {
                byte[] arr = Convert.FromBase64String(inputStr);
                MemoryStream ms = new MemoryStream(arr);
                btmp = new Bitmap(ms);
                ms.Close();
                return btmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Base64StringToImage 转换失败\nException：" + ex.Message);
                return btmp;
            }
        }

        private void SignPictureBox_Validated(object sender, EventArgs e)
        {
            MessageBox.Show(ImgToBase64String(bitmap));
        }

        private void qqButton1_Click(object sender, EventArgs e)
        {
            String tmp = "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCABiANADASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwDvNA8CeFtY0iK9eW6vpnJ86VbyWPD55XarADb0xjtT7/wxYaFd21touoxw3tzv+zafqKi5inKjJGW+dR6kNxnpVl9E1W30xrjQfKkj1KzVbu0lmMWJDGF86NwDhsYBBGDgHr188uJde1Z9GvNb1fTba50Sd7G78q6xeYlZYmfYVwCBk7s8jmvocJRniHze1vHs9dbO2nrpovTyyk0uh614dOkeIfD1jq0Wl2iLdRBynkqdrdCM45wQRWn/AGPpn/QOtP8Avwv+FYnw9ht7fwbBDZ7fssd1drDtbcNguZduD3GMc11FeLioRhXnCGybS9LmkXdIpf2Ppn/QOtP+/C/4Uf2Ppn/QOtP+/C/4VdorAZS/sfTP+gdaf9+F/wAKP7H0z/oHWn/fhf8ACrtFAFL+x9M/6B1p/wB+F/wo/sfTP+gdaf8Afhf8Ku0UAUv7H0z/AKB1p/34X/Cj+x9M/wCgdaf9+F/wq7RQBS/sfTP+gdaf9+F/wo/sfTP+gdaf9+F/wq7RQBS/sfTP+gdaf9+F/wAKP7H0z/oHWn/fhf8ACrtFAFL+x9M/6B1p/wB+F/wo/sfTP+gdaf8Afhf8Ku0UAUv7H0z/AKB1p/34X/Cj+x9M/wCgdaf9+F/wq7RQBS/sfTP+gdaf9+F/wo/sfTP+gdaf9+F/wq7RQBS/sfTP+gdaf9+F/wAKP7H0z/oHWn/fhf8ACrtFAHD+L9Ri0NWh0vw5YXdwlv58rzIEjiUsEXkKdxLHoMcA5I4rR8NNb6tp09xc6Pp0Fxb3MluWt1DxsUOCyMVBxnI6dQa2tX0q11vS5tPvA5gl2k+W5RgVYMpBHIIIB/CjTdNttH0mHT7NWW3gj2IGbJ+pPck812SqYd4ZQUffvv5ff+FibPm8hdJ/5A1j/wBe8f8A6CKx/G2j2mpeE9XZrG3mu1s5WhkeIMyuEJBBxnqBWxpP/IGsf+veP/0EVZljWaF4n5V1Kt9DWFGq6VSM10Y2rqxFYPBJp1s9qqrbtErRKoAAUgYwBwOKWa9t7e5treWULLcsUhXB+chSxH5AmsjwTI0vgbQnf7xsYR+SAUeLbWaXRlvbSNpLzTZlvYEU8uU+8n/AkLr/AMCrR0o/WHSk+rX+X47ivpc3qKgsryDULGC9tnEkE8ayRsO6kZFT1ztNOzKCiiikAUUUUAFR3FxDa28lxcSpFDEpd5HOFUDqSakrlUjbxfqTyTgroVjcMkcJ/wCXyZGwXb1jVgQB3IyegralTU7yk7RW/wDkvN/1oJslTxVeXiLcaZ4b1O7syN3nMY4C49USRgx/ECnr4ysYGC6va3ujEnAa/iCxn/tqpZB+LCuipGVXUqyhlIwQRkGr9rRejp6eTd/xuvwQWfcbHLHNEssTrJGwyrKcgj1Bp9clLDN4MuGubKHf4ccl7m2QHdZE9ZIwOsfdl7ckdxXVQzRXMKTQSpLFINyOjAqw9QR1qKtLkSlF3i9n+j7P+tgTH0UUViMKKKhuru3sbWS6u5o4IIl3PJIwVVHqTTSbdkBNRXKi/wBb8TLjS1k0jTG5F/MgM86+sUZ+6D2Z+f8AZ70kun6x4aIvNNvL3VrIc3VldyebLjHLxOed3+weD2wa6vqtvdlJKXb/ADeyf9OxPMdXRVexvrbUrGG9s5lmt5l3I69x/Q+3arFcrTi7PcoKRvuN9KWkb7jfSkBU0n/kDWP/AF7x/wDoIq5VPSf+QNY/9e8f/oIq5QBz3gX/AJELQf8Arxi/9BFdDXPeBePAmhr3WzjVh6EDBH1BBFdDXRi/94qer/MUdkcntuvB1xcSRwNc+HpXaZliBMliTy2F/ijJy2ByuTwR06a0u7e+tY7q0njnt5V3JJG25WHsamrm7nwzPZ3Ul94bvRp08jb5bV032s7dyydVY/3lIPqDV88K/wDEdpd+j9fPz69e4tVsdJRXPW/igW0iW3iG0Ok3LHCyO4a2lP8Asy9Af9ltp9jXQAggEHIPQisKlKdN+8v8n6PZjTuLRRRWYyvfTtbafczoAWiiZwD0yATWb4RtktfB+jxR5INpG7E9WZlDMT7kkn8a072A3VhcW6kKZYmQE9sjFZXhC5+0eFrBHjMU9rELWeI9Ukj+Rh+YyPYg10r/AHd27r8nb9RdTcooormGHUYNc34Wj/s691rRYgBaWdyJLYD+BJV8wpj0DFsexA7V0lc94bJutU8QaljEc195EX+0sKCMn/vsOPwFdNL+FUT2svvuv0v+InujoaKzNX1/TdDjQ3txiWTiGCNS8sp9EQcsfpWao8Qa+n7zfoNi38KlXu5B9eVi/DcfpUww8pR55aR7v9Or+XzBsuax4ktNKlS0RXvNTl/1Nhb4Mr+5/ur6s2AKp23h+61O6j1DxLJHPJGQ8Gnxc29ufU5/1j/7RGB2A6nU0jQtN0OJ0sLYI8hzLMxLySn1dzyx+prRq3WjTXLQ+97/AC7fn520Fa+4UUUVylHM3ml6no2oTal4fjingnJe70yR/LEj/wDPSNuiue4PDdeDzWjpXiCx1Z2gQvBexjMtncLsmj+qnqP9oZB7GtWuQ8SX/hjUZvsc0cuo6nbnMaaarNcwt7On+r/FgK7ab+se7OLbXVb/AD6NeenqS9Dr6RvuN9K4/wAM2Xi2PUfN1C7ePSAvy2t66T3JOD1kRVCjpxlz15547BvuN9Kwr0VSnyqSl6DTuVNJ/wCQNY/9e8f/AKCKuVT0n/kDWP8A17x/+girlYjOdfwN4eeWST7JOrSO0jBL2dRuYkk4DgDJJPFM/wCEKsocnTtR1jT2P/PC/kZc+u2QspPTt2rpaK6frmI6zb9Xf8xcq7HLtL4l0Bi0oOv6f3aNFju4h/ujCSfhtPsa19J17TNciZ9Pu0lZOJIjlZIj6Oh+ZT9RWjWVqvhvS9YdZrm223SD93dwMYpo/wDddcMOvTOKftKVT+IrPuv1Wi+63zFZrY0Z4IbqB4LiKOaFxteORQysPQg9a51fCtzpbE+HdYmsIeosp4/tFuD/ALKkhkHsrAe1M+3az4aZU1NZdV0vOFvoIibiEdvNjUfP/vIM+q96rafocfimS61nWob5IriT/QbSS4liMMKgAEorDDOQWPfBFb0oSpRcnP3PRSTfo7a+tmvmrpu5dOqeJ9Px9u0KHUIweZdMuAGx6+VJj8gzVLD400J5RDc3bafcH/ljqETW7Z9t4AP4E1H/AMIJ4d/59Lj/AMDp/wD4umTfD3wxcRGKewlljPVXvJ2B/AvT5sDL4+ZeiS/OT/Cwe8dMjrIgdGDKehByDXMXxn8Na9daulrLPpN6iG8EC7nglXjzdg5ZSuA2Mn5AcHmmRfDbwrbxPHbadLbhhg+TdzJ/J6m/4Q/yjm08Q6/b85A+2+cB/wB/Q2fxqKbw0JO0209HeNtPk3/X3Ddzesr211Gzju7KeOe3lGUkjbIIqckAZPAFeb6h8KJLyeSeHxTqFrOzFxJDEkZLdmYR7Qx6ZOATUS/CvU3RUvvF1xqSrn5b6OaRT9V8/H6Vr9VwT1+sW8uV/wBfkLml2NDWvHthdTyadper29vCjBLjUVYOwJ/5ZwIMmSQ+oBUe54q3p9trdxYw6fpMEmhaTGDi5u8S3cuTkkIchCSSSXycn7opNP8AB2q6SMadqejWnGMwaGqE9OpEnPQflWh9g8Xw/OmvaZcsP+Wc2msin8VlJH5GtZzw0YqFCUfnd697ctr+t0u27aV+pe0vw9p2kyvcQwmS8kGJbydjJNJ9XPOPYYHtWrXNL4pk02RYPEtidNJHF4jeZaOf+umMp9HA+pro0dJY1kjZXRgGVlOQQehBrza8KyfNV1v13T9GWrdB1FFZmr69YaKifapGaeU4htoVMk0x9FQcn69B3IrKEJTlyxV2O9jTrnbzxZE12+n6Javq+oIdrpAwEMJ/6aSn5V+gy3tUJ03WvETE6vK+macT8thay/vpB6TSDoP9lP8Avo10NnZWunWqWtlbxW9vGMJHEgVR+Aro5aVH4/el2W3zfX5feTqznx4av9WAfxJqssyt96wsmMNuAf4SR88n4nB9K3rHT7PTLZbaxtYbaBekcKBR+lWaKzqYipUXK3p2Wi+4aSQUjfcb6UtI33G+lYjKmk/8gax/694//QRVyqek/wDIGsf+veP/ANBFXKACiiigAooooAKKKKACiiigAooooAKKKKACiiigBrokiMkiqyMMMrDII9DXImdfA+ox27s50C73+RHHE0j2s3Xy1C5JjYZwAODwOCAOworajV5Lxkrxe6/X1X/AE0cw2ra3rjeRo1hNp1vn95f6jDtOP+mcWdxPu2APQ1paT4estIZp08y5vpBiW9uW3zSexbsP9kYA9K1aKqdduPJBcq/P1fX8vILdwooornGFFFFABSN9xvpS0jfcb6UAYcuuaf4e8KWuoalP5VulunIUksdmcADvgGqWl65rkniGC31WwgtLO/hlktIlO6eLy9n+tOduSGJwOmMc1ZudHfWfDWlxw3ZtJ4PJuIpRGJAGVehU9QQSKdY6BfJq8OpaprL30tvG8cCJbpCih8bicZJJwO+OOld1J4dUXzW5nfe9/K2lt97v/gy73N+iiiuEoKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAOe8UeIzo629jZLFLq982y0ilDbOoBdiP4V3Zx1OMCsm1m1yWxs9c0/xIus2Tyjz7c2kcStGTtbZgblZDzhifukGtTxJ4OtPEt7ZXkt9f2dzZhvJks5VQgkg5OVPdRWfofgW60b7RLL4o1SeSa5NzKI1jiSRiBncu09dvJBGfrzXq0pYWOHVmufqmr330TtppbVNO/fS0PmudJbkpbRKp2qEAAHQDFSb2/vH86KK8osN7f3j+dG9v7x/OiigA3t/eP50b2/vH86KKADe394/nRvb+8fzoooAN7f3j+dG9v7x/OiigA3t/eP50b2/vH86KKADe394/nRvb+8fzoooAN7f3j+dG9v7x/OiigA3t/eP50b2/vH86KKADe394/nRvb+8fzoooAN7f3j+dG9v7x/OiigA3t/eP50b2/vH86KKADe394/nS7m/vH86KKAP//Z";
            bitmap = Base64StringToImage(tmp);
            this.SignPictureBox.Invalidate();
        }

        #endregion
    }
}
