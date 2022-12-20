namespace dynmapConverter
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.storageToCombobox = new System.Windows.Forms.ComboBox();
            this.storageFromCombobox = new System.Windows.Forms.ComboBox();
            this.Start = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.toMysqlAddr = new System.Windows.Forms.TextBox();
            this.toMysqlUser = new System.Windows.Forms.TextBox();
            this.toMysqlPasswd = new System.Windows.Forms.TextBox();
            this.toMysqlDatabase = new System.Windows.Forms.TextBox();
            this.mysqlUserText = new System.Windows.Forms.Label();
            this.mysqlPasswdText = new System.Windows.Forms.Label();
            this.mysqlDbText = new System.Windows.Forms.Label();
            this.mysqlAddress = new System.Windows.Forms.Label();
            this.fromFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fromFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fromMysqlAddrText = new System.Windows.Forms.Label();
            this.fromMysqlDbText = new System.Windows.Forms.Label();
            this.fromMysqlPasswdText = new System.Windows.Forms.Label();
            this.fromMysqlUserText = new System.Windows.Forms.Label();
            this.fromMysqlDatabase = new System.Windows.Forms.TextBox();
            this.fromMysqlPasswd = new System.Windows.Forms.TextBox();
            this.fromMysqlUser = new System.Windows.Forms.TextBox();
            this.fromMysqlAddr = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(322, 111);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(356, 104);
            this.label1.TabIndex = 2;
            this.label1.Text = "Dynmap Storage Converter";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(177, 207);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "From:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(651, 207);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "To:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // storageToCombobox
            // 
            this.storageToCombobox.FormattingEnabled = true;
            this.storageToCombobox.Items.AddRange(new object[] {
            "FileTree",
            "SQLite",
            "MySQL"});
            this.storageToCombobox.Location = new System.Drawing.Point(659, 240);
            this.storageToCombobox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.storageToCombobox.Name = "storageToCombobox";
            this.storageToCombobox.Size = new System.Drawing.Size(139, 23);
            this.storageToCombobox.TabIndex = 3;
            this.storageToCombobox.SelectedIndexChanged += new System.EventHandler(this.storageToCombobox_SelectedIndexChanged);
            // 
            // storageFromCombobox
            // 
            this.storageFromCombobox.FormattingEnabled = true;
            this.storageFromCombobox.Items.AddRange(new object[] {
            "FileTree",
            "SQLite",
            "MySQL"});
            this.storageFromCombobox.Location = new System.Drawing.Point(190, 240);
            this.storageFromCombobox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.storageFromCombobox.Name = "storageFromCombobox";
            this.storageFromCombobox.Size = new System.Drawing.Size(136, 23);
            this.storageFromCombobox.TabIndex = 4;
            this.storageFromCombobox.SelectedIndexChanged += new System.EventHandler(this.storageFromCombobox_SelectedIndexChanged);
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(391, 218);
            this.Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(191, 66);
            this.Start.TabIndex = 5;
            this.Start.Text = "Start Process";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(9, 477);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(912, 33);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 6;
            this.progressBar1.UseWaitCursor = true;
            // 
            // progressLabel
            // 
            this.progressLabel.Location = new System.Drawing.Point(6, 449);
            this.progressLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(556, 28);
            this.progressLabel.TabIndex = 7;
            this.progressLabel.Text = "Wait_For_User_Input";
            // 
            // toMysqlAddr
            // 
            this.toMysqlAddr.Location = new System.Drawing.Point(730, 314);
            this.toMysqlAddr.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.toMysqlAddr.Name = "toMysqlAddr";
            this.toMysqlAddr.Size = new System.Drawing.Size(111, 23);
            this.toMysqlAddr.TabIndex = 9;
            this.toMysqlAddr.Visible = false;
            // 
            // toMysqlUser
            // 
            this.toMysqlUser.Location = new System.Drawing.Point(730, 343);
            this.toMysqlUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.toMysqlUser.Name = "toMysqlUser";
            this.toMysqlUser.Size = new System.Drawing.Size(111, 23);
            this.toMysqlUser.TabIndex = 10;
            this.toMysqlUser.Visible = false;
            // 
            // toMysqlPasswd
            // 
            this.toMysqlPasswd.Location = new System.Drawing.Point(730, 372);
            this.toMysqlPasswd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.toMysqlPasswd.Name = "toMysqlPasswd";
            this.toMysqlPasswd.Size = new System.Drawing.Size(111, 23);
            this.toMysqlPasswd.TabIndex = 11;
            this.toMysqlPasswd.Visible = false;
            // 
            // toMysqlDatabase
            // 
            this.toMysqlDatabase.Location = new System.Drawing.Point(730, 400);
            this.toMysqlDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.toMysqlDatabase.Name = "toMysqlDatabase";
            this.toMysqlDatabase.Size = new System.Drawing.Size(111, 23);
            this.toMysqlDatabase.TabIndex = 12;
            this.toMysqlDatabase.Visible = false;
            // 
            // mysqlUserText
            // 
            this.mysqlUserText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.mysqlUserText.Location = new System.Drawing.Point(595, 343);
            this.mysqlUserText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.mysqlUserText.Name = "mysqlUserText";
            this.mysqlUserText.Size = new System.Drawing.Size(139, 24);
            this.mysqlUserText.TabIndex = 13;
            this.mysqlUserText.Text = "MySQL USER:\r\n\r\n";
            this.mysqlUserText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlUserText.Visible = false;
            // 
            // mysqlPasswdText
            // 
            this.mysqlPasswdText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.mysqlPasswdText.Location = new System.Drawing.Point(564, 372);
            this.mysqlPasswdText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.mysqlPasswdText.Name = "mysqlPasswdText";
            this.mysqlPasswdText.Size = new System.Drawing.Size(170, 24);
            this.mysqlPasswdText.TabIndex = 13;
            this.mysqlPasswdText.Text = "MySQL PASSWD:\r\n\r\n";
            this.mysqlPasswdText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlPasswdText.Visible = false;
            // 
            // mysqlDbText
            // 
            this.mysqlDbText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.mysqlDbText.Location = new System.Drawing.Point(545, 399);
            this.mysqlDbText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.mysqlDbText.Name = "mysqlDbText";
            this.mysqlDbText.Size = new System.Drawing.Size(189, 24);
            this.mysqlDbText.TabIndex = 13;
            this.mysqlDbText.Text = "MySQL DATABASE:\r\n\r\n";
            this.mysqlDbText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlDbText.Visible = false;
            // 
            // mysqlAddress
            // 
            this.mysqlAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.mysqlAddress.Location = new System.Drawing.Point(609, 315);
            this.mysqlAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.mysqlAddress.Name = "mysqlAddress";
            this.mysqlAddress.Size = new System.Drawing.Size(121, 21);
            this.mysqlAddress.TabIndex = 14;
            this.mysqlAddress.Text = "MySQL IP:";
            this.mysqlAddress.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlAddress.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(730, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(189, 188);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // fromFileDialog
            // 
            this.fromFileDialog.FileName = "dynmap.db";
            // 
            // toFileDialog
            // 
            this.toFileDialog.FileName = "dynmap.db";
            // 
            // fromMysqlAddrText
            // 
            this.fromMysqlAddrText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fromMysqlAddrText.Location = new System.Drawing.Point(133, 314);
            this.fromMysqlAddrText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fromMysqlAddrText.Name = "fromMysqlAddrText";
            this.fromMysqlAddrText.Size = new System.Drawing.Size(121, 21);
            this.fromMysqlAddrText.TabIndex = 23;
            this.fromMysqlAddrText.Text = "MySQL IP:";
            this.fromMysqlAddrText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.fromMysqlAddrText.Visible = false;
            // 
            // fromMysqlDbText
            // 
            this.fromMysqlDbText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fromMysqlDbText.Location = new System.Drawing.Point(69, 398);
            this.fromMysqlDbText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fromMysqlDbText.Name = "fromMysqlDbText";
            this.fromMysqlDbText.Size = new System.Drawing.Size(189, 24);
            this.fromMysqlDbText.TabIndex = 20;
            this.fromMysqlDbText.Text = "MySQL DATABASE:\r\n\r\n";
            this.fromMysqlDbText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.fromMysqlDbText.Visible = false;
            // 
            // fromMysqlPasswdText
            // 
            this.fromMysqlPasswdText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fromMysqlPasswdText.Location = new System.Drawing.Point(88, 370);
            this.fromMysqlPasswdText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fromMysqlPasswdText.Name = "fromMysqlPasswdText";
            this.fromMysqlPasswdText.Size = new System.Drawing.Size(170, 24);
            this.fromMysqlPasswdText.TabIndex = 21;
            this.fromMysqlPasswdText.Text = "MySQL PASSWD:\r\n\r\n";
            this.fromMysqlPasswdText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.fromMysqlPasswdText.Visible = false;
            // 
            // fromMysqlUserText
            // 
            this.fromMysqlUserText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fromMysqlUserText.Location = new System.Drawing.Point(119, 342);
            this.fromMysqlUserText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fromMysqlUserText.Name = "fromMysqlUserText";
            this.fromMysqlUserText.Size = new System.Drawing.Size(139, 24);
            this.fromMysqlUserText.TabIndex = 22;
            this.fromMysqlUserText.Text = "MySQL USER:\r\n\r\n";
            this.fromMysqlUserText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.fromMysqlUserText.Visible = false;
            // 
            // fromMysqlDatabase
            // 
            this.fromMysqlDatabase.Location = new System.Drawing.Point(254, 399);
            this.fromMysqlDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.fromMysqlDatabase.Name = "fromMysqlDatabase";
            this.fromMysqlDatabase.Size = new System.Drawing.Size(111, 23);
            this.fromMysqlDatabase.TabIndex = 19;
            this.fromMysqlDatabase.Visible = false;
            // 
            // fromMysqlPasswd
            // 
            this.fromMysqlPasswd.Location = new System.Drawing.Point(254, 370);
            this.fromMysqlPasswd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.fromMysqlPasswd.Name = "fromMysqlPasswd";
            this.fromMysqlPasswd.Size = new System.Drawing.Size(111, 23);
            this.fromMysqlPasswd.TabIndex = 18;
            this.fromMysqlPasswd.Visible = false;
            // 
            // fromMysqlUser
            // 
            this.fromMysqlUser.Location = new System.Drawing.Point(254, 342);
            this.fromMysqlUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.fromMysqlUser.Name = "fromMysqlUser";
            this.fromMysqlUser.Size = new System.Drawing.Size(111, 23);
            this.fromMysqlUser.TabIndex = 17;
            this.fromMysqlUser.Visible = false;
            // 
            // fromMysqlAddr
            // 
            this.fromMysqlAddr.Location = new System.Drawing.Point(254, 313);
            this.fromMysqlAddr.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.fromMysqlAddr.Name = "fromMysqlAddr";
            this.fromMysqlAddr.Size = new System.Drawing.Size(111, 23);
            this.fromMysqlAddr.TabIndex = 16;
            this.fromMysqlAddr.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.fromMysqlAddrText);
            this.Controls.Add(this.fromMysqlDbText);
            this.Controls.Add(this.fromMysqlPasswdText);
            this.Controls.Add(this.fromMysqlUserText);
            this.Controls.Add(this.fromMysqlDatabase);
            this.Controls.Add(this.fromMysqlPasswd);
            this.Controls.Add(this.fromMysqlUser);
            this.Controls.Add(this.fromMysqlAddr);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.mysqlAddress);
            this.Controls.Add(this.mysqlDbText);
            this.Controls.Add(this.mysqlPasswdText);
            this.Controls.Add(this.mysqlUserText);
            this.Controls.Add(this.toMysqlDatabase);
            this.Controls.Add(this.toMysqlPasswd);
            this.Controls.Add(this.toMysqlUser);
            this.Controls.Add(this.toMysqlAddr);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.storageFromCombobox);
            this.Controls.Add(this.storageToCombobox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Dynmap Converter";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label mysqlAddress;

        private System.Windows.Forms.Label mysqlPasswdText;
        private System.Windows.Forms.Label mysqlDbText;
        private System.Windows.Forms.TextBox toMysqlUser;
        private System.Windows.Forms.TextBox toMysqlPasswd;
        private System.Windows.Forms.TextBox toMysqlDatabase;

        private System.Windows.Forms.Label mysqlUserText;

        private System.Windows.Forms.TextBox toMysqlAddr;

        private System.Windows.Forms.Label progressLabel;

        private System.Windows.Forms.ProgressBar progressBar1;

        private System.Windows.Forms.Button Start;

        private System.Windows.Forms.ComboBox storageToCombobox;

        private System.Windows.Forms.ComboBox storageFromCombobox;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        #endregion
        private System.Windows.Forms.FolderBrowserDialog fromFolderBrowserDialog;
        private System.Windows.Forms.FolderBrowserDialog toFolderBrowserDialog;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog fromFileDialog;
        private System.Windows.Forms.OpenFileDialog toFileDialog;
        private System.Windows.Forms.Label fromMysqlAddrText;
        private System.Windows.Forms.Label fromMysqlDbText;
        private System.Windows.Forms.Label fromMysqlPasswdText;
        private System.Windows.Forms.Label fromMysqlUserText;
        private System.Windows.Forms.TextBox fromMysqlDatabase;
        private System.Windows.Forms.TextBox fromMysqlPasswd;
        private System.Windows.Forms.TextBox fromMysqlUser;
        private System.Windows.Forms.TextBox fromMysqlAddr;
    }
}