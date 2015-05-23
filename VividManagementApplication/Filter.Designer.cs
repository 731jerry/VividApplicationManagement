namespace VividManagementApplication
{
    partial class Filter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.clientID = new System.Windows.Forms.ComboBox();
            this.label63 = new System.Windows.Forms.Label();
            this.OKButton = new ControlExs.QQButton();
            this.CancelButton = new ControlExs.QQButton();
            this.DzTypeComboBox = new System.Windows.Forms.ComboBox();
            this.clientName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // toDate
            // 
            this.toDate.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.toDate.Location = new System.Drawing.Point(332, 107);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(154, 26);
            this.toDate.TabIndex = 319;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(300, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 20);
            this.label3.TabIndex = 318;
            this.label3.Text = "到:";
            // 
            // fromDate
            // 
            this.fromDate.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.fromDate.Location = new System.Drawing.Point(332, 66);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(154, 26);
            this.fromDate.TabIndex = 317;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(300, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 20);
            this.label2.TabIndex = 316;
            this.label2.Text = "从:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label19.Location = new System.Drawing.Point(258, 26);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(68, 20);
            this.label19.TabIndex = 314;
            this.label19.Text = "对方单位:";
            // 
            // clientID
            // 
            this.clientID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clientID.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.clientID.FormattingEnabled = true;
            this.clientID.Location = new System.Drawing.Point(103, 21);
            this.clientID.Name = "clientID";
            this.clientID.Size = new System.Drawing.Size(122, 28);
            this.clientID.TabIndex = 313;
            this.clientID.SelectedIndexChanged += new System.EventHandler(this.clientID_SelectedIndexChanged);
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.label63.ForeColor = System.Drawing.Color.Black;
            this.label63.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label63.Location = new System.Drawing.Point(29, 25);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(68, 20);
            this.label63.TabIndex = 312;
            this.label63.Text = "客户编号:";
            // 
            // OKButton
            // 
            this.OKButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.OKButton.Location = new System.Drawing.Point(33, 150);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(91, 28);
            this.OKButton.TabIndex = 320;
            this.OKButton.Text = "确定";
            this.OKButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.CancelButton.Location = new System.Drawing.Point(395, 150);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(91, 28);
            this.CancelButton.TabIndex = 321;
            this.CancelButton.Text = "取消";
            this.CancelButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // DzTypeComboBox
            // 
            this.DzTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DzTypeComboBox.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.DzTypeComboBox.FormattingEnabled = true;
            this.DzTypeComboBox.Items.AddRange(new object[] {
            "应收款对账单",
            "应付款对账单"});
            this.DzTypeComboBox.Location = new System.Drawing.Point(33, 81);
            this.DzTypeComboBox.Name = "DzTypeComboBox";
            this.DzTypeComboBox.Size = new System.Drawing.Size(192, 28);
            this.DzTypeComboBox.TabIndex = 323;
            // 
            // clientName
            // 
            this.clientName.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.clientName.Location = new System.Drawing.Point(332, 23);
            this.clientName.Name = "clientName";
            this.clientName.Size = new System.Drawing.Size(154, 26);
            this.clientName.TabIndex = 324;
            // 
            // Filter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 204);
            this.Controls.Add(this.clientName);
            this.Controls.Add(this.DzTypeComboBox);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.toDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fromDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.clientID);
            this.Controls.Add(this.label63);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Filter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Filter";
            this.Load += new System.EventHandler(this.Filter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.DateTimePicker fromDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label19;
        public System.Windows.Forms.ComboBox clientID;
        private System.Windows.Forms.Label label63;
        public ControlExs.QQButton OKButton;
        public ControlExs.QQButton CancelButton;
        public System.Windows.Forms.ComboBox DzTypeComboBox;
        public System.Windows.Forms.TextBox clientName;
    }
}