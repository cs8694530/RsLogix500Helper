namespace RSLogix500ToDoNet
{
    partial class Index
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Index));
            this.SeachBtn = new System.Windows.Forms.Button();
            this.Path_Label = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Filter_ListBox = new System.Windows.Forms.ListBox();
            this.RawData_BTN = new System.Windows.Forms.Button();
            this.ConvertData_BTN = new System.Windows.Forms.Button();
            this.TotalCount_Label = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // SeachBtn
            // 
            this.SeachBtn.Location = new System.Drawing.Point(28, 23);
            this.SeachBtn.Name = "SeachBtn";
            this.SeachBtn.Size = new System.Drawing.Size(75, 23);
            this.SeachBtn.TabIndex = 0;
            this.SeachBtn.Text = "選擇檔案";
            this.SeachBtn.UseVisualStyleBackColor = true;
            this.SeachBtn.Click += new System.EventHandler(this.SeachBtn_Click);
            // 
            // Path_Label
            // 
            this.Path_Label.AutoSize = true;
            this.Path_Label.Location = new System.Drawing.Point(109, 28);
            this.Path_Label.Name = "Path_Label";
            this.Path_Label.Size = new System.Drawing.Size(0, 12);
            this.Path_Label.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(28, 52);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(760, 468);
            this.textBox1.TabIndex = 2;
            this.textBox1.WordWrap = false;
            // 
            // Filter_ListBox
            // 
            this.Filter_ListBox.FormattingEnabled = true;
            this.Filter_ListBox.ItemHeight = 12;
            this.Filter_ListBox.Location = new System.Drawing.Point(794, 110);
            this.Filter_ListBox.Name = "Filter_ListBox";
            this.Filter_ListBox.Size = new System.Drawing.Size(93, 88);
            this.Filter_ListBox.TabIndex = 3;
            this.Filter_ListBox.SelectedIndexChanged += new System.EventHandler(this.Filter_ListBox_SelectedIndexChanged);
            // 
            // RawData_BTN
            // 
            this.RawData_BTN.Location = new System.Drawing.Point(803, 52);
            this.RawData_BTN.Name = "RawData_BTN";
            this.RawData_BTN.Size = new System.Drawing.Size(75, 23);
            this.RawData_BTN.TabIndex = 4;
            this.RawData_BTN.Text = "原始資料";
            this.RawData_BTN.UseVisualStyleBackColor = true;
            this.RawData_BTN.Click += new System.EventHandler(this.RawData_BTN_Click);
            // 
            // ConvertData_BTN
            // 
            this.ConvertData_BTN.Location = new System.Drawing.Point(803, 81);
            this.ConvertData_BTN.Name = "ConvertData_BTN";
            this.ConvertData_BTN.Size = new System.Drawing.Size(75, 23);
            this.ConvertData_BTN.TabIndex = 5;
            this.ConvertData_BTN.Text = "翻譯資料";
            this.ConvertData_BTN.UseVisualStyleBackColor = true;
            this.ConvertData_BTN.Click += new System.EventHandler(this.ConvertData_BTN_Click);
            // 
            // TotalCount_Label
            // 
            this.TotalCount_Label.AutoSize = true;
            this.TotalCount_Label.Location = new System.Drawing.Point(801, 508);
            this.TotalCount_Label.Name = "TotalCount_Label";
            this.TotalCount_Label.Size = new System.Drawing.Size(58, 12);
            this.TotalCount_Label.TabIndex = 6;
            this.TotalCount_Label.Text = "TotalCount";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(819, 227);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 532);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.TotalCount_Label);
            this.Controls.Add(this.ConvertData_BTN);
            this.Controls.Add(this.RawData_BTN);
            this.Controls.Add(this.Filter_ListBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Path_Label);
            this.Controls.Add(this.SeachBtn);
            this.Name = "Index";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SeachBtn;
        private System.Windows.Forms.Label Path_Label;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox Filter_ListBox;
        private System.Windows.Forms.Button RawData_BTN;
        private System.Windows.Forms.Button ConvertData_BTN;
        private System.Windows.Forms.Label TotalCount_Label;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

