namespace DynmapConverterV2
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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.fromMysqlDatabase = new System.Windows.Forms.TextBox();
            this.fromMysqlPasswd = new System.Windows.Forms.TextBox();
            this.fromMysqlUser = new System.Windows.Forms.TextBox();
            this.fromMysqlAddr = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(276, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(305, 90);
            this.label1.TabIndex = 2;
            this.label1.Text = "Dynmap Storage Converter";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(152, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "From:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(558, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 21);
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
            this.storageToCombobox.Location = new System.Drawing.Point(565, 208);
            this.storageToCombobox.Name = "storageToCombobox";
            this.storageToCombobox.Size = new System.Drawing.Size(120, 21);
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
            this.storageFromCombobox.Location = new System.Drawing.Point(163, 208);
            this.storageFromCombobox.Name = "storageFromCombobox";
            this.storageFromCombobox.Size = new System.Drawing.Size(117, 21);
            this.storageFromCombobox.TabIndex = 4;
            this.storageFromCombobox.SelectedIndexChanged += new System.EventHandler(this.storageFromCombobox_SelectedIndexChanged);
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(335, 189);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(164, 57);
            this.Start.TabIndex = 5;
            this.Start.Text = "Start Process";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 413);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(782, 29);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 6;
            this.progressBar1.UseWaitCursor = true;
            // 
            // progressLabel
            // 
            this.progressLabel.Location = new System.Drawing.Point(5, 389);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(477, 24);
            this.progressLabel.TabIndex = 7;
            this.progressLabel.Text = "Wait_For_User_Input";
            // 
            // toMysqlAddr
            // 
            this.toMysqlAddr.Location = new System.Drawing.Point(626, 272);
            this.toMysqlAddr.Name = "toMysqlAddr";
            this.toMysqlAddr.Size = new System.Drawing.Size(96, 20);
            this.toMysqlAddr.TabIndex = 9;
            this.toMysqlAddr.Visible = false;
            // 
            // toMysqlUser
            // 
            this.toMysqlUser.Location = new System.Drawing.Point(626, 297);
            this.toMysqlUser.Name = "toMysqlUser";
            this.toMysqlUser.Size = new System.Drawing.Size(96, 20);
            this.toMysqlUser.TabIndex = 10;
            this.toMysqlUser.Visible = false;
            // 
            // toMysqlPasswd
            // 
            this.toMysqlPasswd.Location = new System.Drawing.Point(626, 322);
            this.toMysqlPasswd.Name = "toMysqlPasswd";
            this.toMysqlPasswd.Size = new System.Drawing.Size(96, 20);
            this.toMysqlPasswd.TabIndex = 11;
            this.toMysqlPasswd.Visible = false;
            // 
            // toMysqlDatabase
            // 
            this.toMysqlDatabase.Location = new System.Drawing.Point(626, 347);
            this.toMysqlDatabase.Name = "toMysqlDatabase";
            this.toMysqlDatabase.Size = new System.Drawing.Size(96, 20);
            this.toMysqlDatabase.TabIndex = 12;
            this.toMysqlDatabase.Visible = false;
            // 
            // mysqlUserText
            // 
            this.mysqlUserText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mysqlUserText.Location = new System.Drawing.Point(510, 297);
            this.mysqlUserText.Name = "mysqlUserText";
            this.mysqlUserText.Size = new System.Drawing.Size(119, 21);
            this.mysqlUserText.TabIndex = 13;
            this.mysqlUserText.Text = "MySQL USER:\r\n\r\n";
            this.mysqlUserText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlUserText.Visible = false;
            // 
            // mysqlPasswdText
            // 
            this.mysqlPasswdText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mysqlPasswdText.Location = new System.Drawing.Point(483, 322);
            this.mysqlPasswdText.Name = "mysqlPasswdText";
            this.mysqlPasswdText.Size = new System.Drawing.Size(146, 21);
            this.mysqlPasswdText.TabIndex = 13;
            this.mysqlPasswdText.Text = "MySQL PASSWD:\r\n\r\n";
            this.mysqlPasswdText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlPasswdText.Visible = false;
            // 
            // mysqlDbText
            // 
            this.mysqlDbText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mysqlDbText.Location = new System.Drawing.Point(467, 346);
            this.mysqlDbText.Name = "mysqlDbText";
            this.mysqlDbText.Size = new System.Drawing.Size(162, 21);
            this.mysqlDbText.TabIndex = 13;
            this.mysqlDbText.Text = "MySQL DATABASE:\r\n\r\n";
            this.mysqlDbText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlDbText.Visible = false;
            // 
            // mysqlAddress
            // 
            this.mysqlAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mysqlAddress.Location = new System.Drawing.Point(522, 273);
            this.mysqlAddress.Name = "mysqlAddress";
            this.mysqlAddress.Size = new System.Drawing.Size(104, 18);
            this.mysqlAddress.TabIndex = 14;
            this.mysqlAddress.Text = "MySQL IP:";
            this.mysqlAddress.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlAddress.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(626, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(162, 163);
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
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(114, 272);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 18);
            this.label4.TabIndex = 23;
            this.label4.Text = "MySQL IP:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(59, 345);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 21);
            this.label5.TabIndex = 20;
            this.label5.Text = "MySQL DATABASE:\r\n\r\n";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label5.Visible = false;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(75, 321);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(146, 21);
            this.label6.TabIndex = 21;
            this.label6.Text = "MySQL PASSWD:\r\n\r\n";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label6.Visible = false;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(102, 296);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 21);
            this.label7.TabIndex = 22;
            this.label7.Text = "MySQL USER:\r\n\r\n";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label7.Visible = false;
            // 
            // fromMysqlDatabase
            // 
            this.fromMysqlDatabase.Location = new System.Drawing.Point(218, 346);
            this.fromMysqlDatabase.Name = "fromMysqlDatabase";
            this.fromMysqlDatabase.Size = new System.Drawing.Size(96, 20);
            this.fromMysqlDatabase.TabIndex = 19;
            this.fromMysqlDatabase.Visible = false;
            // 
            // fromMysqlPasswd
            // 
            this.fromMysqlPasswd.Location = new System.Drawing.Point(218, 321);
            this.fromMysqlPasswd.Name = "fromMysqlPasswd";
            this.fromMysqlPasswd.Size = new System.Drawing.Size(96, 20);
            this.fromMysqlPasswd.TabIndex = 18;
            this.fromMysqlPasswd.Visible = false;
            // 
            // fromMysqlUser
            // 
            this.fromMysqlUser.Location = new System.Drawing.Point(218, 296);
            this.fromMysqlUser.Name = "fromMysqlUser";
            this.fromMysqlUser.Size = new System.Drawing.Size(96, 20);
            this.fromMysqlUser.TabIndex = 17;
            this.fromMysqlUser.Visible = false;
            // 
            // fromMysqlAddr
            // 
            this.fromMysqlAddr.Location = new System.Drawing.Point(218, 271);
            this.fromMysqlAddr.Name = "fromMysqlAddr";
            this.fromMysqlAddr.Size = new System.Drawing.Size(96, 20);
            this.fromMysqlAddr.TabIndex = 16;
            this.fromMysqlAddr.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox fromMysqlDatabase;
        private System.Windows.Forms.TextBox fromMysqlPasswd;
        private System.Windows.Forms.TextBox fromMysqlUser;
        private System.Windows.Forms.TextBox fromMysqlAddr;
    }
}