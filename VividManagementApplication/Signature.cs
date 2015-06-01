using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace VividManagementApplication
{
    public partial class Signature : Form
    {
        //记录直线或者曲线的对象
        GraphicsPath mousePath = new System.Drawing.Drawing2D.GraphicsPath();
        //画笔透明度
        private int myAlpha = 100;
        //画笔颜色对象
        private Color myUserColor = new Color();
        //画笔宽度
        private int myPenWidth = 3;
        //签名的图片对象
        public Bitmap SavedBitmap;

        public Signature()
        {
            InitializeComponent();
        }

        private void SignPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                try
                {
                    mousePath.AddLine(e.X, e.Y, e.X, e.Y);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            SignPictureBox.Invalidate();
        }

        private void SignPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mousePath.StartFigure();
            }
        }

        private void SignPictureBox_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                myUserColor = System.Drawing.Color.Blue;
                myAlpha = 255;
                Pen CurrentPen = new Pen(Color.FromArgb(myAlpha, myUserColor), myPenWidth);
                e.Graphics.DrawPath(CurrentPen, mousePath);
            }
            catch { }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            SignPictureBox.CreateGraphics().Clear(Color.White);
            mousePath.Reset();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SavedBitmap = new Bitmap(SignPictureBox.Width, SignPictureBox.Height);
            SignPictureBox.DrawToBitmap(SavedBitmap, new Rectangle(0, 0, SignPictureBox.Width, SignPictureBox.Height));
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void uploadSignPicButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofg = new OpenFileDialog())
            {
                ofg.Filter = "图片文件(*.png)|*.png|图片文件(*.jpg)|*.jpg|图片文件(*.bmp)|*.bmp";
                if (ofg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    btnClear.PerformClick();

                    //Bitmap UploadedBitmap = new Bitmap(ofg.FileName);
                    //SignPictureBox.Image = UploadedBitmap;

                    /*
                    #region 区域截取
                    Image imageSource = new Bitmap(ofg.FileName); ;//或者（ Image.FromFile(filepath)）
                    double orgWidth = Convert.ToDouble(imageSource.Width);
                    double orgHight = Convert.ToDouble(imageSource.Height);

                    Rectangle cropArea = new Rectangle();

                    double x = orgWidth / 2 - 145;//（145是从中间开始向两边截图的宽度，可以自定义）
                    double y = 0;

                    double width = 290;
                    double height = orgHight;

                    cropArea.X = Convert.ToInt32(x);
                    cropArea.Y = Convert.ToInt32(y);
                    cropArea.Width = Convert.ToInt32(width);
                    cropArea.Height = Convert.ToInt32(height);

                    Bitmap bmpImage = new Bitmap(imageSource);
                    Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);

                    SignPictureBox.Image = bmpCrop;
                    #endregion
                     */

                    Bitmap UploadedBitmap = new Bitmap(ofg.FileName);
                    SignPictureBox.Image = ResizeImage(UploadedBitmap, SignPictureBox.Width, SignPictureBox.Height);

                }
            }
        }

        public Bitmap ResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量   
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }


    }
}
