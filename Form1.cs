using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace dynmapConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void storageFromCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _itemFrom = storageFromCombobox.GetItemText(storageFromCombobox.SelectedItem);
            _cv.from = _itemFrom; 
            switch (_itemFrom)
            {
                case "MySQL":
                    mysqlUser.Visible = true;
                    mysqlPasswd.Visible = true;
                    mysqlAddr.Visible = true;
                    mysqlDb.Visible = true;
                    textBox1.Visible = true;
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    textBox4.Visible = true;
                    break;
            }
        }

        private void storageToCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _itemTo = storageToCombobox.GetItemText(storageToCombobox.SelectedItem);
            _cv.to = _itemTo;
            switch (_itemTo)
            {
                case "MySQL":
                    mysqlUser.Visible = true;
                    mysqlPasswd.Visible = true;
                    mysqlAddr.Visible = true;
                    mysqlDb.Visible = true;
                    textBox1.Visible = true;
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    textBox4.Visible = true;
                    break;
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            _cv.msUser = mysqlUser.Text;
            _cv.msPwd = mysqlPasswd.Text;
            _cv.msAddr = mysqlAddr.Text;
            _cv.msDb = mysqlDb.Text;
            progressBar1.Value += 1;
            _cv.start = true;
            _cv.StartConversion();
        }

        private string _itemFrom = "";
        private string _itemTo = "";
        private Convert _cv = new Convert();
    }

    public class Convert
    {
        public string from { get; set; }
        public string fromPath { get; set; }
        public string to { get; set; }
        public string toPath { get; set; }
        
        
        public string msUser { get; set; }
        public string msPwd { get; set; }
        public string msAddr { get; set; }
        public string msDb { get; set; }
        public bool start { get; set; }
        private DbConnection dbC = DbConnection.Instance();
        
        public void StartConversion()
        {
            if (start && from != null && to != null)
            {
                if (from == "FileTree")
                {
                    MessageBox.Show("started conversion from: " + from +" to: " + to);
                }

                if (from == "MySQL")
                {
                    dbC.Password = msPwd;
                    dbC.UserName = msUser;
                    dbC.Server = msAddr;
                    dbC.DatabaseName = msDb;
                }

                if (dbC.IsConnect())
                {
                    
                }
            }
        }
    }

    public class DbConnection
    {
        private DbConnection()
        {
        }
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        private MySqlConnection Connection { get; set;}

        private static DbConnection _instance = null;
        public static DbConnection Instance()
        {
            if (_instance == null)
                _instance = new DbConnection();
            return _instance;
        }
        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (string.IsNullOrEmpty(DatabaseName))
                    return false;
                string connString = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                Connection = new MySqlConnection(connString);
                Connection.Open();
            }
            return true;
        }
        public void Close()
        {
            Connection.Close();
        }  
    }
}
