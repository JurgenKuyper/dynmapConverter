namespace dynmapConverter
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.storageToCombobox = new System.Windows.Forms.ComboBox();
            this.storageFromCombobox = new System.Windows.Forms.ComboBox();
            this.Start = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.fromOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toOpenFIleDialog = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.mysqlUser = new System.Windows.Forms.Label();
            this.mysqlPasswd = new System.Windows.Forms.Label();
            this.mysqlDb = new System.Windows.Forms.Label();
            this.mysqlAddr = new System.Windows.Forms.Label();
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
            this.storageToCombobox.Items.AddRange(new object[] {"FileTree", "SQLite", "MySQL"});
            this.storageToCombobox.Location = new System.Drawing.Point(565, 208);
            this.storageToCombobox.Name = "storageToCombobox";
            this.storageToCombobox.Size = new System.Drawing.Size(120, 21);
            this.storageToCombobox.TabIndex = 3;
            this.storageToCombobox.SelectedIndexChanged += new System.EventHandler(this.storageToCombobox_SelectedIndexChanged);
            // 
            // storageFromCombobox
            // 
            this.storageFromCombobox.FormattingEnabled = true;
            this.storageFromCombobox.Items.AddRange(new object[] {"FileTree", "SQLite", "MySQL"});
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
            // fromOpenFileDialog
            // 
            this.fromOpenFileDialog.FileName = "fromOpenFileDialog";
            // 
            // toOpenFIleDialog
            // 
            this.toOpenFIleDialog.FileName = "toOpenFileDialog";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(403, 270);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(96, 20);
            this.textBox1.TabIndex = 9;
            this.textBox1.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(403, 295);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(96, 20);
            this.textBox2.TabIndex = 10;
            this.textBox2.Visible = false;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(403, 320);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(96, 20);
            this.textBox3.TabIndex = 11;
            this.textBox3.Visible = false;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(403, 345);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(96, 20);
            this.textBox4.TabIndex = 12;
            this.textBox4.Visible = false;
            // 
            // mysqlUser
            // 
            this.mysqlUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.mysqlUser.Location = new System.Drawing.Point(287, 295);
            this.mysqlUser.Name = "mysqlUser";
            this.mysqlUser.Size = new System.Drawing.Size(119, 21);
            this.mysqlUser.TabIndex = 13;
            this.mysqlUser.Text = "MySQL USER:\r\n\r\n";
            this.mysqlUser.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlUser.Visible = false;
            // 
            // mysqlPasswd
            // 
            this.mysqlPasswd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.mysqlPasswd.Location = new System.Drawing.Point(260, 320);
            this.mysqlPasswd.Name = "mysqlPasswd";
            this.mysqlPasswd.Size = new System.Drawing.Size(146, 21);
            this.mysqlPasswd.TabIndex = 13;
            this.mysqlPasswd.Text = "MySQL PASSWD:\r\n\r\n";
            this.mysqlPasswd.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlPasswd.Visible = false;
            // 
            // mysqlDb
            // 
            this.mysqlDb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.mysqlDb.Location = new System.Drawing.Point(244, 344);
            this.mysqlDb.Name = "mysqlDb";
            this.mysqlDb.Size = new System.Drawing.Size(162, 21);
            this.mysqlDb.TabIndex = 13;
            this.mysqlDb.Text = "MySQL DATABASE:\r\n\r\n";
            this.mysqlDb.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlDb.Visible = false;
            // 
            // mysqlAddr
            // 
            this.mysqlAddr.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.mysqlAddr.Location = new System.Drawing.Point(299, 271);
            this.mysqlAddr.Name = "mysqlAddr";
            this.mysqlAddr.Size = new System.Drawing.Size(104, 18);
            this.mysqlAddr.TabIndex = 14;
            this.mysqlAddr.Text = "MySQL IP:";
            this.mysqlAddr.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.mysqlAddr.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mysqlAddr);
            this.Controls.Add(this.mysqlDb);
            this.Controls.Add(this.mysqlPasswd);
            this.Controls.Add(this.mysqlUser);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.storageFromCombobox);
            this.Controls.Add(this.storageToCombobox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label mysqlAddr;

        private System.Windows.Forms.Label mysqlPasswd;
        private System.Windows.Forms.Label mysqlDb;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;

        private System.Windows.Forms.Label mysqlUser;

        private System.Windows.Forms.TextBox textBox1;

        private System.Windows.Forms.OpenFileDialog fromOpenFileDialog;
        private System.Windows.Forms.OpenFileDialog toOpenFIleDialog;

        private System.Windows.Forms.Label progressLabel;

        private System.Windows.Forms.ProgressBar progressBar1;

        private System.Windows.Forms.Button Start;

        private System.Windows.Forms.ComboBox storageToCombobox;

        private System.Windows.Forms.ComboBox storageFromCombobox;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        #endregion
    }
}

