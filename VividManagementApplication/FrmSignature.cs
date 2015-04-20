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
    public partial class FrmSignature : Form
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

        public FrmSignature()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
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
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mousePath.StartFigure();
            }

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
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
            pictureBox1.CreateGraphics().Clear(Color.White);
            mousePath.Reset();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SavedBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(SavedBitmap, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
