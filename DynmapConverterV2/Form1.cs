using System.Data;

namespace DynmapConverterV2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _cv.UpdateProgress += UpdateProgress;
            _cv.UpdateStatus += UpdateStatus;
        }

        private void storageFromCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _itemFrom = storageFromCombobox.GetItemText(storageFromCombobox.SelectedItem);
            _cv.from = _itemFrom;
            if (_itemFrom == "MySQL")
            {
                mysqlUserText.Visible = true;
                mysqlPasswdText.Visible = true;
                mysqlAddress.Visible = true;
                mysqlDbText.Visible = true;
                fromMysqlAddr.Visible = true;
                fromMysqlUser.Visible = true;
                fromMysqlPasswd.Visible = true;
                fromMysqlDatabase.Visible = true;
            }
            else
            {
                mysqlUserText.Visible = false;
                mysqlPasswdText.Visible = false;
                mysqlAddress.Visible = false;
                mysqlDbText.Visible = false;
                fromMysqlAddr.Visible = false;
                fromMysqlUser.Visible = false;
                fromMysqlPasswd.Visible = false;
                fromMysqlDatabase.Visible = false;
                if (_itemFrom == "FileTree")
                {
                    FolderBrowserDialog fromFolderBrowserDialog = new FolderBrowserDialog();
                    DialogResult fromDialogResult = fromFolderBrowserDialog.ShowDialog();
                    if (fromDialogResult == DialogResult.OK)
                    {
                        string folderPath = fromFolderBrowserDialog.SelectedPath;
                        _cv.fromPath = folderPath;
                    }
                }
                if (_itemFrom == "SQLite")
                {
                    OpenFileDialog fromFileDialog = new OpenFileDialog();
                    if (fromFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        _cv.fromPath = fromFileDialog.FileName;
                    }
                }
            }
        }

        private void storageToCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _itemTo = storageToCombobox.GetItemText(storageToCombobox.SelectedItem);
            _cv.to = _itemTo;
            if (_itemTo == "MySQL")
            {
                mysqlUserText.Visible = true;
                mysqlPasswdText.Visible = true;
                mysqlAddress.Visible = true;
                mysqlDbText.Visible = true;
                toMysqlAddr.Visible = true;
                toMysqlUser.Visible = true;
                toMysqlPasswd.Visible = true;
                toMysqlDatabase.Visible = true;
            }
            else
            {
                mysqlUserText.Visible = false;
                mysqlPasswdText.Visible = false;
                mysqlAddress.Visible = false;
                mysqlDbText.Visible = false;
                toMysqlAddr.Visible = false;
                toMysqlUser.Visible = false;
                toMysqlPasswd.Visible = false;
                toMysqlDatabase.Visible = false;
                if (_itemTo == "FileTree")
                {
                    FolderBrowserDialog toFolderBrowserDialog = new FolderBrowserDialog();
                    DialogResult toDialogResult = toFolderBrowserDialog.ShowDialog();
                    if (toDialogResult == DialogResult.OK)
                    {
                        string folderPath = toFolderBrowserDialog.SelectedPath;
                        _cv.toPath = folderPath;
                    }
                }
                if (_itemTo == "SQLite")
                {
                    OpenFileDialog toFileDialog = new OpenFileDialog();
                    if (toFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        _cv.toPath = toFileDialog.FileName;
                    }
                }
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            _cv.fromMsUser = fromMysqlUser.Text;
            _cv.fromMsPwd = fromMysqlPasswd.Text;
            _cv.fromMsAddr = fromMysqlAddr.Text;
            _cv.fromMsDb = fromMysqlDatabase.Text;
            _cv.toMsUser = toMysqlUser.Text;
            _cv.toMsPwd = toMysqlPasswd.Text;
            _cv.toMsAddr = toMysqlAddr.Text;
            _cv.toMsDb = toMysqlDatabase.Text;
            _cv.start = true;
            _cv.StartConversion();
        }
        private void UpdateProgress(int progress)
        {
            progressBar1.Value = progress;
        }
        private void UpdateStatus(string status)
        {
            progressLabel.Text = status;
        }
        private string _itemFrom = "";
        private string _itemTo = "";
        private Convert _cv = new Convert();
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}