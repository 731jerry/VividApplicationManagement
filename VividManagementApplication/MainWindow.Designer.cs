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
            this.lbLastLogonTime = new System.Windows.Forms.Label();
            this.label112 = new System.Windows.Forms.Label();
            this.label106 = new System.Windows.Forms.Label();
            this.label95 = new System.Windows.Forms.Label();
            this.label98 = new System.Windows.Forms.Label();
            this.lbUserName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.ContentPanel = new System.Windows.Forms.Panel();
            this.mainDGVTitle = new System.Windows.Forms.Label();
            this.backupData = new ControlExs.QQButton();
            this.refeshButton = new ControlExs.QQButton();
            this.MainDataGridView = new System.Windows.Forms.DataGridView();
            this.qqButton1 = new ControlExs.QQButton();
            this.DiscardButton = new ControlExs.QQButton();
            this.ViewButton = new ControlExs.QQButton();
            this.NavPanel = new System.Windows.Forms.Panel();
            this.listHtButton = new ControlExs.QQButton();
            this.newHtButton = new ControlExs.QQButton();
            this.htRadio = new ControlExs.QQRadioButton();
            this.listSfzhButton = new ControlExs.QQButton();
            this.newPzButton = new ControlExs.QQButton();
            this.cwRadio = new ControlExs.QQRadioButton();
            this.listKhdzButton = new ControlExs.QQButton();
            this.listCgXsButton = new ControlExs.QQButton();
            this.newCgZsButton = new ControlExs.QQButton();
            this.ywRadio = new ControlExs.QQRadioButton();
            this.listCcdButton = new ControlExs.QQButton();
            this.listJcdButton = new ControlExs.QQButton();
            this.listKcButton = new ControlExs.QQButton();
            this.newJCcButton = new ControlExs.QQButton();
            this.ccRadio = new ControlExs.QQRadioButton();
            this.listSpButton = new ControlExs.QQButton();
            this.newSpButton = new ControlExs.QQButton();
            this.spRadio = new ControlExs.QQRadioButton();
            this.listCxButton = new ControlExs.QQButton();
            this.newCxButton = new ControlExs.QQButton();
            this.cxRadio = new ControlExs.QQRadioButton();
            this.pbDownFile = new System.Windows.Forms.ProgressBar();
            this.updateDataTimer = new System.Windows.Forms.Timer(this.components);
            this.tmrShows = new System.Windows.Forms.Timer(this.components);
            this.notificationPanel = new System.Windows.Forms.Panel();
            this.lblSHOWS2 = new System.Windows.Forms.Label();
            this.lblSHOWS = new System.Windows.Forms.Label();
            this.commerceTimer = new System.Windows.Forms.Timer(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.MainPanel.SuspendLayout();
            this.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainDataGridView)).BeginInit();
            this.NavPanel.SuspendLayout();
            this.notificationPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLastLogonTime
            // 
            resources.ApplyResources(this.lbLastLogonTime, "lbLastLogonTime");
            this.lbLastLogonTime.BackColor = System.Drawing.Color.Transparent;
            this.lbLastLogonTime.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbLastLogonTime.Name = "lbLastLogonTime";
            // 
            // label112
            // 
            resources.ApplyResources(this.label112, "label112");
            this.label112.BackColor = System.Drawing.Color.Transparent;
            this.label112.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label112.Name = "label112";
            // 
            // label106
            // 
            resources.ApplyResources(this.label106, "label106");
            this.label106.BackColor = System.Drawing.Color.Transparent;
            this.label106.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label106.Name = "label106";
            // 
            // label95
            // 
            resources.ApplyResources(this.label95, "label95");
            this.label95.BackColor = System.Drawing.Color.Transparent;
            this.label95.ForeColor = System.Drawing.Color.Red;
            this.label95.Name = "label95";
            // 
            // label98
            // 
            resources.ApplyResources(this.label98, "label98");
            this.label98.BackColor = System.Drawing.Color.Transparent;
            this.label98.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label98.Name = "label98";
            // 
            // lbUserName
            // 
            resources.ApplyResources(this.lbUserName, "lbUserName");
            this.lbUserName.BackColor = System.Drawing.Color.Transparent;
            this.lbUserName.ForeColor = System.Drawing.Color.Red;
            this.lbUserName.Name = "lbUserName";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Name = "label1";
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
            this.ContentPanel.Controls.Add(this.mainDGVTitle);
            this.ContentPanel.Controls.Add(this.backupData);
            this.ContentPanel.Controls.Add(this.refeshButton);
            this.ContentPanel.Controls.Add(this.MainDataGridView);
            this.ContentPanel.Controls.Add(this.qqButton1);
            this.ContentPanel.Controls.Add(this.DiscardButton);
            this.ContentPanel.Controls.Add(this.ViewButton);
            resources.ApplyResources(this.ContentPanel, "ContentPanel");
            this.ContentPanel.Name = "ContentPanel";
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
            // 
            // qqButton1
            // 
            resources.ApplyResources(this.qqButton1, "qqButton1");
            this.qqButton1.Name = "qqButton1";
            this.qqButton1.UseVisualStyleBackColor = true;
            this.qqButton1.Click += new System.EventHandler(this.qqButton1_Click);
            // 
            // DiscardButton
            // 
            resources.ApplyResources(this.DiscardButton, "DiscardButton");
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.UseVisualStyleBackColor = true;
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
            this.NavPanel.Controls.Add(this.listHtButton);
            this.NavPanel.Controls.Add(this.newHtButton);
            this.NavPanel.Controls.Add(this.htRadio);
            this.NavPanel.Controls.Add(this.listSfzhButton);
            this.NavPanel.Controls.Add(this.newPzButton);
            this.NavPanel.Controls.Add(this.cwRadio);
            this.NavPanel.Controls.Add(this.listKhdzButton);
            this.NavPanel.Controls.Add(this.listCgXsButton);
            this.NavPanel.Controls.Add(this.newCgZsButton);
            this.NavPanel.Controls.Add(this.ywRadio);
            this.NavPanel.Controls.Add(this.listCcdButton);
            this.NavPanel.Controls.Add(this.listJcdButton);
            this.NavPanel.Controls.Add(this.listKcButton);
            this.NavPanel.Controls.Add(this.newJCcButton);
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
            // listCgXsButton
            // 
            resources.ApplyResources(this.listCgXsButton, "listCgXsButton");
            this.listCgXsButton.Name = "listCgXsButton";
            this.listCgXsButton.UseVisualStyleBackColor = true;
            this.listCgXsButton.Click += new System.EventHandler(this.listCgXsButton_Click);
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
            // newJCcButton
            // 
            resources.ApplyResources(this.newJCcButton, "newJCcButton");
            this.newJCcButton.Name = "newJCcButton";
            this.newJCcButton.UseVisualStyleBackColor = true;
            this.newJCcButton.Click += new System.EventHandler(this.newJCcButton_Click);
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
            // pbDownFile
            // 
            resources.ApplyResources(this.pbDownFile, "pbDownFile");
            this.pbDownFile.Name = "pbDownFile";
            // 
            // updateDataTimer
            // 
            this.updateDataTimer.Interval = 3000;
            this.updateDataTimer.Tick += new System.EventHandler(this.updateDataTimer_Tick);
            // 
            // tmrShows
            // 
            this.tmrShows.Tick += new System.EventHandler(this.tmrShows_Tick);
            // 
            // notificationPanel
            // 
            this.notificationPanel.BackColor = System.Drawing.Color.White;
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
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.notificationPanel);
            this.Controls.Add(this.pbDownFile);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.lbLastLogonTime);
            this.Controls.Add(this.label112);
            this.Controls.Add(this.label106);
            this.Controls.Add(this.label95);
            this.Controls.Add(this.label98);
            this.Controls.Add(this.lbUserName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbLastLogonTime;
        private System.Windows.Forms.Label label112;
        private System.Windows.Forms.Label label106;
        private System.Windows.Forms.Label label95;
        private System.Windows.Forms.Label label98;
        private System.Windows.Forms.Label lbUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Panel NavPanel;
        private ControlExs.QQButton ViewButton;
        private ControlExs.QQButton DiscardButton;
        private System.Windows.Forms.Panel ContentPanel;
        private ControlExs.QQButton qqButton1;
        private System.Windows.Forms.DataGridView MainDataGridView;
        private ControlExs.QQButton refeshButton;
        private ControlExs.QQButton backupData;
        private System.Windows.Forms.ProgressBar pbDownFile;
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
        private ControlExs.QQButton listCgXsButton;
        private ControlExs.QQButton newCgZsButton;
        private ControlExs.QQRadioButton ywRadio;
        private ControlExs.QQButton listCcdButton;
        private ControlExs.QQButton listJcdButton;
        private ControlExs.QQButton listKcButton;
        private ControlExs.QQButton newJCcButton;
        private ControlExs.QQRadioButton ccRadio;
        private ControlExs.QQButton listSpButton;
        private ControlExs.QQButton newSpButton;
        private ControlExs.QQRadioButton spRadio;
        private ControlExs.QQButton listCxButton;
        private ControlExs.QQButton newCxButton;
        private ControlExs.QQRadioButton cxRadio;
        private System.Windows.Forms.Label mainDGVTitle;
    }
}

