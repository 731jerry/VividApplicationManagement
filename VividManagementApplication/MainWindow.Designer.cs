namespace VividManagementApplication
{
    partial class MainWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        /// 改动
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.lbExpireTime = new System.Windows.Forms.Label();
            this.label112 = new System.Windows.Forms.Label();
            this.lbUserName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.ContentPanel = new System.Windows.Forms.Panel();
            this.qqButton2 = new ControlExs.QQButton();
            this.qqButton1 = new ControlExs.QQButton();
            this.PrintButton = new ControlExs.QQButton();
            this.settingQQButton = new ControlExs.QQButton();
            this.mainDGVTitle = new System.Windows.Forms.Label();
            this.backupData = new ControlExs.QQButton();
            this.refeshButton = new ControlExs.QQButton();
            this.MainDataGridView = new System.Windows.Forms.DataGridView();
            this.ViewButton = new ControlExs.QQButton();
            this.NavPanel = new System.Windows.Forms.Panel();
            this.listXsButton = new ControlExs.QQButton();
            this.listHtButton = new ControlExs.QQButton();
            this.newHtButton = new ControlExs.QQButton();
            this.htRadio = new ControlExs.QQRadioButton();
            this.listSfzhButton = new ControlExs.QQButton();
            this.newPzButton = new ControlExs.QQButton();
            this.cwRadio = new ControlExs.QQRadioButton();
            this.listKhdzButton = new ControlExs.QQButton();
            this.listCgButton = new ControlExs.QQButton();
            this.newCgZsButton = new ControlExs.QQButton();
            this.ywRadio = new ControlExs.QQRadioButton();
            this.listCcdButton = new ControlExs.QQButton();
            this.listJcdButton = new ControlExs.QQButton();
            this.listKcButton = new ControlExs.QQButton();
            this.ccRadio = new ControlExs.QQRadioButton();
            this.listSpButton = new ControlExs.QQButton();
            this.newSpButton = new ControlExs.QQButton();
            this.spRadio = new ControlExs.QQRadioButton();
            this.listCxButton = new ControlExs.QQButton();
            this.newCxButton = new ControlExs.QQButton();
            this.cxRadio = new ControlExs.QQRadioButton();
            this.pbUploadDownloadFile = new System.Windows.Forms.ProgressBar();
            this.updateDataTimer = new System.Windows.Forms.Timer(this.components);
            this.tmrShows = new System.Windows.Forms.Timer(this.components);
            this.notificationPanel = new System.Windows.Forms.Panel();
            this.lblSHOWS2 = new System.Windows.Forms.Label();
            this.lblSHOWS = new System.Windows.Forms.Label();
            this.commerceTimer = new System.Windows.Forms.Timer(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.keepOnlineTimer = new System.Windows.Forms.Timer(this.components);
            this.pbUploadDownloadLabel = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.exit = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyBlinkTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyImageList = new System.Windows.Forms.ImageList(this.components);
            this.ExtendExpireLinkLabel = new System.Windows.Forms.LinkLabel();
            this.UserDegreeLabel = new System.Windows.Forms.Label();
            this.qqButton3 = new ControlExs.QQButton();
            this.qqRadioButton1 = new ControlExs.QQRadioButton();
            this.MainPanel.SuspendLayout();
            this.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainDataGridView)).BeginInit();
            this.NavPanel.SuspendLayout();
            this.notificationPanel.SuspendLayout();
            this.notifyIconContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbExpireTime
            // 
            resources.ApplyResources(this.lbExpireTime, "lbExpireTime");
            this.lbExpireTime.BackColor = System.Drawing.Color.Transparent;
            this.lbExpireTime.ForeColor = System.Drawing.Color.OrangeRed;
            this.lbExpireTime.Name = "lbExpireTime";
            // 
            // label112
            // 
            resources.ApplyResources(this.label112, "label112");
            this.label112.BackColor = System.Drawing.Color.Transparent;
            this.label112.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label112.Name = "label112";
            // 
            // lbUserName
            // 
            resources.ApplyResources(this.lbUserName, "lbUserName");
            this.lbUserName.BackColor = System.Drawing.Color.Transparent;
            this.lbUserName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbUserName.ForeColor = System.Drawing.Color.Blue;
            this.lbUserName.Name = "lbUserName";
            this.lbUserName.Click += new System.EventHandler(this.lbUserName_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // MainPanel
            // 
            this.MainPanel.BackColor = System.Drawing.Color.White;
            this.MainPanel.Controls.Add(this.ContentPanel);
            this.MainPanel.Controls.Add(this.NavPanel);
            resources.ApplyResources(this.MainPanel, "MainPanel");
            this.MainPanel.Name = "MainPanel";
            // 
            // ContentPanel
            // 
            this.ContentPanel.BackColor = System.Drawing.Color.Transparent;
            this.ContentPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ContentPanel.Controls.Add(this.qqButton2);
            this.ContentPanel.Controls.Add(this.qqButton1);
            this.ContentPanel.Controls.Add(this.PrintButton);
            this.ContentPanel.Controls.Add(this.settingQQButton);
            this.ContentPanel.Controls.Add(this.mainDGVTitle);
            this.ContentPanel.Controls.Add(this.backupData);
            this.ContentPanel.Controls.Add(this.refeshButton);
            this.ContentPanel.Controls.Add(this.MainDataGridView);
            this.ContentPanel.Controls.Add(this.ViewButton);
            resources.ApplyResources(this.ContentPanel, "ContentPanel");
            this.ContentPanel.Name = "ContentPanel";
            // 
            // qqButton2
            // 
            resources.ApplyResources(this.qqButton2, "qqButton2");
            this.qqButton2.Name = "qqButton2";
            this.qqButton2.UseVisualStyleBackColor = true;
            this.qqButton2.Click += new System.EventHandler(this.qqButton2_Click);
            // 
            // qqButton1
            // 
            resources.ApplyResources(this.qqButton1, "qqButton1");
            this.qqButton1.Name = "qqButton1";
            this.qqButton1.UseVisualStyleBackColor = true;
            this.qqButton1.Click += new System.EventHandler(this.btnFlicker_Click);
            // 
            // PrintButton
            // 
            resources.ApplyResources(this.PrintButton, "PrintButton");
            this.PrintButton.Name = "PrintButton";
            this.PrintButton.UseVisualStyleBackColor = true;
            this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // settingQQButton
            // 
            resources.ApplyResources(this.settingQQButton, "settingQQButton");
            this.settingQQButton.Name = "settingQQButton";
            this.settingQQButton.UseVisualStyleBackColor = true;
            this.settingQQButton.Click += new System.EventHandler(this.settingQQButton_Click);
            // 
            // mainDGVTitle
            // 
            resources.ApplyResources(this.mainDGVTitle, "mainDGVTitle");
            this.mainDGVTitle.Name = "mainDGVTitle";
            // 
            // backupData
            // 
            resources.ApplyResources(this.backupData, "backupData");
            this.backupData.Name = "backupData";
            this.backupData.UseVisualStyleBackColor = true;
            this.backupData.Click += new System.EventHandler(this.backupData_Click);
            // 
            // refeshButton
            // 
            resources.ApplyResources(this.refeshButton, "refeshButton");
            this.refeshButton.Name = "refeshButton";
            this.refeshButton.UseVisualStyleBackColor = true;
            this.refeshButton.Click += new System.EventHandler(this.refeshButton_Click);
            // 
            // MainDataGridView
            // 
            this.MainDataGridView.AllowUserToAddRows = false;
            this.MainDataGridView.AllowUserToDeleteRows = false;
            this.MainDataGridView.AllowUserToResizeRows = false;
            this.MainDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.MainDataGridView, "MainDataGridView");
            this.MainDataGridView.MultiSelect = false;
            this.MainDataGridView.Name = "MainDataGridView";
            this.MainDataGridView.ReadOnly = true;
            this.MainDataGridView.RowHeadersVisible = false;
            this.MainDataGridView.RowTemplate.Height = 23;
            this.MainDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MainDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainDataGridView_CellMouseDoubleClick);
            // 
            // ViewButton
            // 
            resources.ApplyResources(this.ViewButton, "ViewButton");
            this.ViewButton.Name = "ViewButton";
            this.ViewButton.UseVisualStyleBackColor = true;
            this.ViewButton.Click += new System.EventHandler(this.ViewButton_Click);
            // 
            // NavPanel
            // 
            this.NavPanel.BackColor = System.Drawing.Color.Transparent;
            this.NavPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.NavPanel.Controls.Add(this.qqButton3);
            this.NavPanel.Controls.Add(this.qqRadioButton1);
            this.NavPanel.Controls.Add(this.listXsButton);
            this.NavPanel.Controls.Add(this.listHtButton);
            this.NavPanel.Controls.Add(this.newHtButton);
            this.NavPanel.Controls.Add(this.htRadio);
            this.NavPanel.Controls.Add(this.listSfzhButton);
            this.NavPanel.Controls.Add(this.newPzButton);
            this.NavPanel.Controls.Add(this.cwRadio);
            this.NavPanel.Controls.Add(this.listKhdzButton);
            this.NavPanel.Controls.Add(this.listCgButton);
            this.NavPanel.Controls.Add(this.newCgZsButton);
            this.NavPanel.Controls.Add(this.ywRadio);
            this.NavPanel.Controls.Add(this.listCcdButton);
            this.NavPanel.Controls.Add(this.listJcdButton);
            this.NavPanel.Controls.Add(this.listKcButton);
            this.NavPanel.Controls.Add(this.ccRadio);
            this.NavPanel.Controls.Add(this.listSpButton);
            this.NavPanel.Controls.Add(this.newSpButton);
            this.NavPanel.Controls.Add(this.spRadio);
            this.NavPanel.Controls.Add(this.listCxButton);
            this.NavPanel.Controls.Add(this.newCxButton);
            this.NavPanel.Controls.Add(this.cxRadio);
            resources.ApplyResources(this.NavPanel, "NavPanel");
            this.NavPanel.Name = "NavPanel";
            // 
            // listXsButton
            // 
            resources.ApplyResources(this.listXsButton, "listXsButton");
            this.listXsButton.Name = "listXsButton";
            this.listXsButton.UseVisualStyleBackColor = true;
            this.listXsButton.Click += new System.EventHandler(this.listXsButton_Click);
            // 
            // listHtButton
            // 
            resources.ApplyResources(this.listHtButton, "listHtButton");
            this.listHtButton.Name = "listHtButton";
            this.listHtButton.UseVisualStyleBackColor = true;
            this.listHtButton.Click += new System.EventHandler(this.listHtButton_Click);
            // 
            // newHtButton
            // 
            resources.ApplyResources(this.newHtButton, "newHtButton");
            this.newHtButton.Name = "newHtButton";
            this.newHtButton.UseVisualStyleBackColor = true;
            this.newHtButton.Click += new System.EventHandler(this.newHtButton_Click);
            // 
            // htRadio
            // 
            resources.ApplyResources(this.htRadio, "htRadio");
            this.htRadio.BackColor = System.Drawing.Color.Transparent;
            this.htRadio.Checked = true;
            this.htRadio.Name = "htRadio";
            this.htRadio.TabStop = true;
            this.htRadio.UseVisualStyleBackColor = false;
            this.htRadio.CheckedChanged += new System.EventHandler(this.htRadio_CheckedChanged);
            // 
            // listSfzhButton
            // 
            resources.ApplyResources(this.listSfzhButton, "listSfzhButton");
            this.listSfzhButton.Name = "listSfzhButton";
            this.listSfzhButton.UseVisualStyleBackColor = true;
            this.listSfzhButton.Click += new System.EventHandler(this.listSfzhButton_Click);
            // 
            // newPzButton
            // 
            resources.ApplyResources(this.newPzButton, "newPzButton");
            this.newPzButton.Name = "newPzButton";
            this.newPzButton.UseVisualStyleBackColor = true;
            this.newPzButton.Click += new System.EventHandler(this.newPzButton_Click);
            // 
            // cwRadio
            // 
            resources.ApplyResources(this.cwRadio, "cwRadio");
            this.cwRadio.BackColor = System.Drawing.Color.Transparent;
            this.cwRadio.Checked = true;
            this.cwRadio.Name = "cwRadio";
            this.cwRadio.TabStop = true;
            this.cwRadio.UseVisualStyleBackColor = false;
            this.cwRadio.CheckedChanged += new System.EventHandler(this.cwRadio_CheckedChanged);
            // 
            // listKhdzButton
            // 
            resources.ApplyResources(this.listKhdzButton, "listKhdzButton");
            this.listKhdzButton.Name = "listKhdzButton";
            this.listKhdzButton.UseVisualStyleBackColor = true;
            this.listKhdzButton.Click += new System.EventHandler(this.listKhdzButton_Click);
            // 
            // listCgButton
            // 
            resources.ApplyResources(this.listCgButton, "listCgButton");
            this.listCgButton.Name = "listCgButton";
            this.listCgButton.UseVisualStyleBackColor = true;
            this.listCgButton.Click += new System.EventHandler(this.listCgXsButton_Click);
            // 
            // newCgZsButton
            // 
            resources.ApplyResources(this.newCgZsButton, "newCgZsButton");
            this.newCgZsButton.Name = "newCgZsButton";
            this.newCgZsButton.UseVisualStyleBackColor = true;
            this.newCgZsButton.Click += new System.EventHandler(this.newCgZsButton_Click);
            // 
            // ywRadio
            // 
            resources.ApplyResources(this.ywRadio, "ywRadio");
            this.ywRadio.BackColor = System.Drawing.Color.Transparent;
            this.ywRadio.Checked = true;
            this.ywRadio.Name = "ywRadio";
            this.ywRadio.TabStop = true;
            this.ywRadio.UseVisualStyleBackColor = false;
            this.ywRadio.CheckedChanged += new System.EventHandler(this.ywRadio_CheckedChanged);
            // 
            // listCcdButton
            // 
            resources.ApplyResources(this.listCcdButton, "listCcdButton");
            this.listCcdButton.Name = "listCcdButton";
            this.listCcdButton.UseVisualStyleBackColor = true;
            this.listCcdButton.Click += new System.EventHandler(this.listCcdButton_Click);
            // 
            // listJcdButton
            // 
            resources.ApplyResources(this.listJcdButton, "listJcdButton");
            this.listJcdButton.Name = "listJcdButton";
            this.listJcdButton.UseVisualStyleBackColor = true;
            this.listJcdButton.Click += new System.EventHandler(this.listJcdButton_Click);
            // 
            // listKcButton
            // 
            resources.ApplyResources(this.listKcButton, "listKcButton");
            this.listKcButton.Name = "listKcButton";
            this.listKcButton.UseVisualStyleBackColor = true;
            this.listKcButton.Click += new System.EventHandler(this.listKcButton_Click);
            // 
            // ccRadio
            // 
            resources.ApplyResources(this.ccRadio, "ccRadio");
            this.ccRadio.BackColor = System.Drawing.Color.Transparent;
            this.ccRadio.Checked = true;
            this.ccRadio.Name = "ccRadio";
            this.ccRadio.TabStop = true;
            this.ccRadio.UseVisualStyleBackColor = false;
            this.ccRadio.CheckedChanged += new System.EventHandler(this.ccRadio_CheckedChanged);
            // 
            // listSpButton
            // 
            resources.ApplyResources(this.listSpButton, "listSpButton");
            this.listSpButton.Name = "listSpButton";
            this.listSpButton.UseVisualStyleBackColor = true;
            this.listSpButton.Click += new System.EventHandler(this.listSpButton_Click);
            // 
            // newSpButton
            // 
            resources.ApplyResources(this.newSpButton, "newSpButton");
            this.newSpButton.Name = "newSpButton";
            this.newSpButton.UseVisualStyleBackColor = true;
            this.newSpButton.Click += new System.EventHandler(this.newSpButton_Click);
            // 
            // spRadio
            // 
            resources.ApplyResources(this.spRadio, "spRadio");
            this.spRadio.BackColor = System.Drawing.Color.Transparent;
            this.spRadio.Checked = true;
            this.spRadio.Name = "spRadio";
            this.spRadio.TabStop = true;
            this.spRadio.UseVisualStyleBackColor = false;
            this.spRadio.CheckedChanged += new System.EventHandler(this.spRadio_CheckedChanged);
            // 
            // listCxButton
            // 
            resources.ApplyResources(this.listCxButton, "listCxButton");
            this.listCxButton.Name = "listCxButton";
            this.listCxButton.UseVisualStyleBackColor = true;
            this.listCxButton.Click += new System.EventHandler(this.listCxButton_Click);
            // 
            // newCxButton
            // 
            resources.ApplyResources(this.newCxButton, "newCxButton");
            this.newCxButton.Name = "newCxButton";
            this.newCxButton.UseVisualStyleBackColor = true;
            this.newCxButton.Click += new System.EventHandler(this.newCxButton_Click);
            // 
            // cxRadio
            // 
            resources.ApplyResources(this.cxRadio, "cxRadio");
            this.cxRadio.BackColor = System.Drawing.Color.Transparent;
            this.cxRadio.Checked = true;
            this.cxRadio.Name = "cxRadio";
            this.cxRadio.TabStop = true;
            this.cxRadio.UseVisualStyleBackColor = false;
            this.cxRadio.CheckedChanged += new System.EventHandler(this.cxRadio_CheckedChanged);
            // 
            // pbUploadDownloadFile
            // 
            resources.ApplyResources(this.pbUploadDownloadFile, "pbUploadDownloadFile");
            this.pbUploadDownloadFile.Name = "pbUploadDownloadFile";
            // 
            // updateDataTimer
            // 
            this.updateDataTimer.Interval = 3500;
            this.updateDataTimer.Tick += new System.EventHandler(this.updateDataTimer_Tick);
            // 
            // tmrShows
            // 
            this.tmrShows.Tick += new System.EventHandler(this.tmrShows_Tick);
            // 
            // notificationPanel
            // 
            this.notificationPanel.BackColor = System.Drawing.Color.Transparent;
            this.notificationPanel.Controls.Add(this.lblSHOWS2);
            this.notificationPanel.Controls.Add(this.lblSHOWS);
            resources.ApplyResources(this.notificationPanel, "notificationPanel");
            this.notificationPanel.Name = "notificationPanel";
            // 
            // lblSHOWS2
            // 
            resources.ApplyResources(this.lblSHOWS2, "lblSHOWS2");
            this.lblSHOWS2.ForeColor = System.Drawing.Color.Red;
            this.lblSHOWS2.Name = "lblSHOWS2";
            // 
            // lblSHOWS
            // 
            resources.ApplyResources(this.lblSHOWS, "lblSHOWS");
            this.lblSHOWS.ForeColor = System.Drawing.Color.Red;
            this.lblSHOWS.Name = "lblSHOWS";
            // 
            // commerceTimer
            // 
            this.commerceTimer.Interval = 1000;
            this.commerceTimer.Tick += new System.EventHandler(this.commerceTimer_Tick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Update_24.png");
            this.imageList1.Images.SetKeyName(1, "Delete_2_24.png");
            this.imageList1.Images.SetKeyName(2, "AppBox_grid.png");
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printDocument1
            // 
            this.printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_BeginPrint);
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printPreviewDialog1
            // 
            resources.ApplyResources(this.printPreviewDialog1, "printPreviewDialog1");
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            // 
            // keepOnlineTimer
            // 
            this.keepOnlineTimer.Interval = 60000;
            this.keepOnlineTimer.Tick += new System.EventHandler(this.keepOnlineTimer_Tick);
            // 
            // pbUploadDownloadLabel
            // 
            resources.ApplyResources(this.pbUploadDownloadLabel, "pbUploadDownloadLabel");
            this.pbUploadDownloadLabel.BackColor = System.Drawing.Color.Transparent;
            this.pbUploadDownloadLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pbUploadDownloadLabel.Name = "pbUploadDownloadLabel";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyIconContextMenuStrip;
            resources.ApplyResources(this.notifyIcon, "notifyIcon");
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // notifyIconContextMenuStrip
            // 
            this.notifyIconContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showWindow,
            this.exit});
            this.notifyIconContextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.notifyIconContextMenuStrip, "notifyIconContextMenuStrip");
            // 
            // showWindow
            // 
            this.showWindow.Image = global::VividManagementApplication.Properties.Resources.ShowWindow;
            this.showWindow.Name = "showWindow";
            resources.ApplyResources(this.showWindow, "showWindow");
            this.showWindow.Click += new System.EventHandler(this.showWindow_Click);
            // 
            // exit
            // 
            this.exit.Image = global::VividManagementApplication.Properties.Resources.Exit;
            this.exit.Name = "exit";
            resources.ApplyResources(this.exit, "exit");
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // notifyBlinkTimer
            // 
            this.notifyBlinkTimer.Interval = 450;
            this.notifyBlinkTimer.Tick += new System.EventHandler(this.notifyBlinkTimer_Tick);
            // 
            // notifyImageList
            // 
            this.notifyImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("notifyImageList.ImageStream")));
            this.notifyImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.notifyImageList.Images.SetKeyName(0, "notifyIcon.ico");
            this.notifyImageList.Images.SetKeyName(1, "empty.ico");
            // 
            // ExtendExpireLinkLabel
            // 
            resources.ApplyResources(this.ExtendExpireLinkLabel, "ExtendExpireLinkLabel");
            this.ExtendExpireLinkLabel.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.ExtendExpireLinkLabel.Name = "ExtendExpireLinkLabel";
            this.ExtendExpireLinkLabel.TabStop = true;
            this.ExtendExpireLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ExtendExpireLinkLabel_LinkClicked);
            // 
            // UserDegreeLabel
            // 
            resources.ApplyResources(this.UserDegreeLabel, "UserDegreeLabel");
            this.UserDegreeLabel.BackColor = System.Drawing.Color.Transparent;
            this.UserDegreeLabel.ForeColor = System.Drawing.Color.OrangeRed;
            this.UserDegreeLabel.Name = "UserDegreeLabel";
            // 
            // qqButton3
            // 
            resources.ApplyResources(this.qqButton3, "qqButton3");
            this.qqButton3.Name = "qqButton3";
            this.qqButton3.UseVisualStyleBackColor = true;
            // 
            // qqRadioButton1
            // 
            resources.ApplyResources(this.qqRadioButton1, "qqRadioButton1");
            this.qqRadioButton1.BackColor = System.Drawing.Color.Transparent;
            this.qqRadioButton1.Checked = true;
            this.qqRadioButton1.Name = "qqRadioButton1";
            this.qqRadioButton1.TabStop = true;
            this.qqRadioButton1.UseVisualStyleBackColor = false;
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UserDegreeLabel);
            this.Controls.Add(this.ExtendExpireLinkLabel);
            this.Controls.Add(this.pbUploadDownloadLabel);
            this.Controls.Add(this.notificationPanel);
            this.Controls.Add(this.pbUploadDownloadFile);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.lbExpireTime);
            this.Controls.Add(this.label112);
            this.Controls.Add(this.lbUserName);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.MainPanel.ResumeLayout(false);
            this.ContentPanel.ResumeLayout(false);
            this.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainDataGridView)).EndInit();
            this.NavPanel.ResumeLayout(false);
            this.NavPanel.PerformLayout();
            this.notificationPanel.ResumeLayout(false);
            this.notificationPanel.PerformLayout();
            this.notifyIconContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbExpireTime;
        private System.Windows.Forms.Label label112;
        private System.Windows.Forms.Label lbUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Panel NavPanel;
        private ControlExs.QQButton ViewButton;
        private System.Windows.Forms.Panel ContentPanel;
        private System.Windows.Forms.DataGridView MainDataGridView;
        private ControlExs.QQButton refeshButton;
        private ControlExs.QQButton backupData;
        private System.Windows.Forms.ProgressBar pbUploadDownloadFile;
        private System.Windows.Forms.Timer updateDataTimer;
        private System.Windows.Forms.Timer tmrShows;
        private System.Windows.Forms.Panel notificationPanel;
        private System.Windows.Forms.Label lblSHOWS2;
        private System.Windows.Forms.Label lblSHOWS;
        private System.Windows.Forms.Timer commerceTimer;
        private System.Windows.Forms.ImageList imageList1;
        private ControlExs.QQButton listHtButton;
        private ControlExs.QQButton newHtButton;
        private ControlExs.QQRadioButton htRadio;
        private ControlExs.QQButton listSfzhButton;
        private ControlExs.QQButton newPzButton;
        private ControlExs.QQRadioButton cwRadio;
        private ControlExs.QQButton listKhdzButton;
        private ControlExs.QQButton listCgButton;
        private ControlExs.QQButton newCgZsButton;
        private ControlExs.QQRadioButton ywRadio;
        private ControlExs.QQButton listCcdButton;
        private ControlExs.QQButton listJcdButton;
        private ControlExs.QQButton listKcButton;
        private ControlExs.QQRadioButton ccRadio;
        private ControlExs.QQButton listSpButton;
        private ControlExs.QQButton newSpButton;
        private ControlExs.QQRadioButton spRadio;
        private ControlExs.QQButton listCxButton;
        private ControlExs.QQButton newCxButton;
        private ControlExs.QQRadioButton cxRadio;
        private ControlExs.QQButton settingQQButton;
        private ControlExs.QQButton listXsButton;
        public System.Windows.Forms.Label mainDGVTitle;
        private ControlExs.QQButton PrintButton;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.Timer keepOnlineTimer;
        private System.Windows.Forms.Label pbUploadDownloadLabel;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Timer notifyBlinkTimer;
        private System.Windows.Forms.ImageList notifyImageList;
        private ControlExs.QQButton qqButton1;
        private System.Windows.Forms.ContextMenuStrip notifyIconContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showWindow;
        private System.Windows.Forms.ToolStripMenuItem exit;
        private ControlExs.QQButton qqButton2;
        private System.Windows.Forms.LinkLabel ExtendExpireLinkLabel;
        private System.Windows.Forms.Label UserDegreeLabel;
        private ControlExs.QQButton qqButton3;
        private ControlExs.QQRadioButton qqRadioButton1;
    }
}

