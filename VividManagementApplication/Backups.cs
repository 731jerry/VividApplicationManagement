using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VividManagementApplication
{
    public partial class Backups : Form
    {
        public Backups()
        {
            InitializeComponent();
        }

        private BackgroundWorker m_BackgroundWorker;// 载入文件列表后台对象
        private BackgroundWorker m_Download_BackgroundWorker;// 下载文件列表后台对象

        private Boolean canClose = true;
        private void Backups_Load(object sender, EventArgs e)
        {
            DataGridViewTextBoxColumn Column1 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new DataGridViewTextBoxColumn();
            Column1.HeaderText = "名称";
            Column2.HeaderText = "时间";
            Column1.Visible = false;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            FileDataGridView.Columns.AddRange(Column1, Column2);

            /*
            m_BackgroundWorker = new BackgroundWorker(); // 实例化后台对象
            //m_BackgroundWorker.WorkerReportsProgress = true; // 设置可以通告进度
            //m_BackgroundWorker.WorkerSupportsCancellation = true; // 设置可以取消
            m_BackgroundWorker.DoWork += new DoWorkEventHandler(listBackupDatabaseWork);
            //m_BackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(UpdateProgress);
            //m_BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompletedWork);
            m_BackgroundWorker.RunWorkerAsync(this);
            */
            listBackupDatabase();
        }

        private void listBackupDatabaseWork(object sender, DoWorkEventArgs e)
        {
            listBackupDatabase();
        }
        private void listBackupDatabase()
        {
            try
            {
                FTPConnector ftpH = new FTPConnector(MainWindow.ONLINE_FTP_USERNAME, MainWindow.ONLINE_FTP_PASSWORD);
                List<FileStruct> listString = ftpH.ListFiles("ftp://" + MainWindow.ONLINE_FTP_HOSTNAME + "/" + "Project/GZB/Users/");
                foreach (FileStruct item in listString)
                {
                    String fileName = item.Name;
                    if (fileName.Split('.')[0].Split('@')[0].Equals(MainWindow.USER_ID + "_backup"))
                    {
                        fileName = fileName.Split('.')[0].Split('@')[1];
                        fileName = fileName.Split('&')[0] + " " + fileName.Split('&')[1].Replace("-", ":");
                        fileName = DateTime.Parse(fileName).ToString("yyyy年MM月dd日HH:mm:ss");
                        FileDataGridView.Rows.Insert(0, item.Name, fileName + "的备份");
                    }
                }

            }
            catch { }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认要重置数据库?", "提示", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                restoreDatabase(FileDataGridView.SelectedRows[0].Cells[0].Value.ToString());
            }
            //MessageBox.Show(this.FileDataGridView.SelectedRows[0].Cells[0].Value.ToString());
        }

        private void FileDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                btnOK.PerformClick();
            }
        }

        private String downloadingFileName;
        private void restoreDatabase(String fileName)
        {
            this.Text = "正在重置数据库...请勿关闭...";
            downloadingFileName = "ftp://" + MainWindow.ONLINE_FTP_HOSTNAME + "/" + "Project/GZB/Users/" + fileName;
            canClose = false;

            m_Download_BackgroundWorker = new BackgroundWorker(); // 实例化后台对象
            //m_Download_BackgroundWorker.WorkerReportsProgress = true; // 设置可以通告进度
            //m_Download_BackgroundWorker.WorkerSupportsCancellation = true; // 设置可以取消
            m_Download_BackgroundWorker.DoWork += new DoWorkEventHandler(DownloadDatabaseWork);
            //m_Download_BackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(UpdateProgress);
            m_Download_BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DownloadCompletedWork);
            m_Download_BackgroundWorker.RunWorkerAsync(this);
        }

        private void DownloadDatabaseWork(object sender, DoWorkEventArgs e)
        {
            FTPConnector ftpH = new FTPConnector(MainWindow.ONLINE_FTP_USERNAME, MainWindow.ONLINE_FTP_PASSWORD);
            ftpH.DownLoadFile(downloadingFileName, MainWindow.LOCAL_DATABASE_LOCATION);
        }

        private void DownloadCompletedWork(object sender, RunWorkerCompletedEventArgs e)
        {
            canClose = true;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void Backups_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!canClose)
            {
                e.Cancel = true;
            }
        }

    }
}
