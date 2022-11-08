﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlexibleConfiguration.Providers;
using MySql.Data;
using MySql.Data.MySqlClient;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dynmapConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _cv.UpdateProgress += UpdateProgress;
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
                mysqlAddr.Visible = true;
                mysqlUser.Visible = true;
                mysqlPasswd.Visible = true;
                mysqlDatabase.Visible = true;
            }
            else
            {
                mysqlUserText.Visible = false;
                mysqlPasswdText.Visible = false;
                mysqlAddress.Visible = false;
                mysqlDbText.Visible = false;
                mysqlAddr.Visible = false;
                mysqlUser.Visible = false;
                mysqlPasswd.Visible = false;
                mysqlDatabase.Visible = false;
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
                mysqlAddr.Visible = true;
                mysqlUser.Visible = true;
                mysqlPasswd.Visible = true;
                mysqlDatabase.Visible = true;
            }

            else
            {
                mysqlUserText.Visible = false;
                mysqlPasswdText.Visible = false;
                mysqlAddress.Visible = false;
                mysqlDbText.Visible = false;
                mysqlAddr.Visible = false;
                mysqlUser.Visible = false;
                mysqlPasswd.Visible = false;
                mysqlDatabase.Visible = false;
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
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            _cv.msUser = mysqlUser.Text;
            _cv.msPwd = mysqlPasswd.Text;
            _cv.msAddr = mysqlAddr.Text;
            _cv.msDb = mysqlDatabase.Text;
            _cv.start = true;
            _cv.StartConversion();
        }
        private void UpdateProgress(int progress)
        {
            progressBar1.Value = progress;  
        }
        private string _itemFrom = "";
        private string _itemTo = "";
        private Convert _cv = new Convert();
        private void Form1_Load(object sender, EventArgs e)
        {

        }
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
        private int mapsCount { get; set; }
        private int totalFoldersCount { get; set; }
        private int foldersCount { get; set; }
        private string configDirFileLocation { get; set; }
        private GetConfigFile getConfig = new GetConfigFile();
        internal delegate void UpdateProgressDelegate(int ProgressPercentage);

        internal event UpdateProgressDelegate UpdateProgress;
        public void StartConversion()
        {
            fromPath = "C:\\Users\\Jurgen\\Dynmap-tests\\paper-1.16.5\\plugins\\dynmap\\web\\tiles";
            configDirFileLocation = getConfig.getDynmapConfig(fromPath);
            Console.WriteLine(configDirFileLocation);
            var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
            dynamic myConfig = deserializer.Deserialize<ExpandoObject>(File.ReadAllText(configDirFileLocation));

            foreach (var item in myConfig)
            {
                if (item.Key == "prefix")
                {
                    dbC.prefix = item.Value;
                }
                
            }
            if (start && from != null && to != null)
            {
                if (from == "FileTree" && to == "MySQL")
                {
                    dbC.Password = msPwd;
                    dbC.UserName = msUser;
                    dbC.Server = msAddr;
                    dbC.DatabaseName = msDb;
                    var directories = CustomSearcher.GetDirectories(fromPath, SearchOption.TopDirectoryOnly); //tiles/<worldname>
                    if (dbC.IsConnect())
                    {
                        MessageBox.Show("sucessfully connected to: " + dbC.Server);
                    }
                    dbC.InitDatabase();
                    foreach (var d in directories)
                    {
                        Console.WriteLine(d);
                        foreach (var folder in CustomSearcher.GetDirectories(d, SearchOption.TopDirectoryOnly)) //<worldname>/mapsnames
                        {
                            foreach (var MCAFolder in CustomSearcher.GetDirectories(folder, SearchOption.TopDirectoryOnly))
                                totalFoldersCount++;
                        }
                    }
                    foreach (var d in directories)
                    {
                        Console.WriteLine(d);
                        foreach (var folder in CustomSearcher.GetDirectories(d, SearchOption.TopDirectoryOnly)) //<worldname>/mapsnames
                        {
                            mapsCount++;
                            foreach (var MCAFolder in CustomSearcher.GetDirectories(folder, SearchOption.TopDirectoryOnly)) //<worldname>/mapmape/mcaTileFolder
                            {
                                foldersCount++;
                                foreach (var image in Directory.GetFiles(MCAFolder))
                                {
                                    FileStream fs = new FileStream(image, FileMode.Open, FileAccess.Read);
                                    BinaryReader br = new BinaryReader(fs);
                                    byte[] imageData = br.ReadBytes((int)fs.Length);
                                    br.Close();
                                    fs.Close();
                                    string imageName = Path.GetFileNameWithoutExtension(image);
                                    string[] imageNameChunks = imageName.Split('_');
                                    int x = Int32.Parse(imageNameChunks[imageNameChunks.Length - 2]);
                                    int y = Int32.Parse(imageNameChunks[imageNameChunks.Length - 1]);
                                    int zoomLevel;
                                    if (imageNameChunks.Length > 2)
                                    {
                                        zoomLevel = imageNameChunks[imageNameChunks.Length - 3].Length;
                                    }
                                    else
                                    {
                                        zoomLevel = 0;
                                    }
                                    //Console.WriteLine(zoomLevel);
                                    //Console.WriteLine(imageName + " x " + x + " y " + y + " zoom " + zoomLevel + " mapsCount " + mapsCount);
                                    dbC.SendData(mapsCount, x, y, zoomLevel, imageData);
                                }
                                Console.WriteLine(MCAFolder);
                                UpdateProgress(foldersCount*100/totalFoldersCount);
                                Application.DoEvents();
                            }
                            dbC.Close();
                        }
                    }
                    //MessageBox.Show(mapsCount.ToString());
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

        private MySqlConnection Connection { get; set; }

        private static DbConnection _instance = null;
        public string prefix { get; set; }
        private string tableTiles;
        private string tableMaps;
        private string tableFaces;
        private string tableMarkerIcons;
        private string tableMarkerFiles;
        private string tableStandaloneFiles;
        private string tableSchemaVersion;
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
                Console.WriteLine("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                string connString = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                try
                {
                    Connection = new MySqlConnection(connString);
                    Connection.Open();
                }
                catch (MySqlException ex)
                {
                    switch (ex.Number)
                    {
                        case 0:
                            MessageBox.Show("Cannot connect to server.  Contact administrator");
                            break;

                        case 1045:
                            MessageBox.Show("Invalid username/password, please try again");
                            break;
                        case 1042:
                            break;
                    }
                }
            }
            return true;
        }
        public bool InitDatabase()
        {
            if (prefix == null)
            {
                prefix = "";
            }
            tableTiles = prefix + "Tiles";
            tableMaps = prefix + "Maps";
            tableFaces = prefix + "Faces";
            tableMarkerIcons = prefix + "MarkerIcons";
            tableMarkerFiles = prefix + "MarkerFiles";
            tableStandaloneFiles = prefix + "StandaloneFiles";
            tableSchemaVersion = prefix + "SchemaVersion";
            string checkTableExistsQuery = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '" + DatabaseName + "' AND table_name = '" + tableSchemaVersion + "';";
            MySqlCommand checkCMD = new MySqlCommand(checkTableExistsQuery, Connection);
            Int64 resultCheck = (long)checkCMD.ExecuteScalar();
            Console.WriteLine(resultCheck.GetType());
            if (resultCheck != 1)
            {
                string createTableMapsQuery = "CREATE TABLE " + tableMaps + " (ID INTEGER PRIMARY KEY AUTO_INCREMENT, WorldID VARCHAR(64) NOT NULL, MapID VARCHAR(64) NOT NULL, Variant VARCHAR(16) NOT NULL, ServerID BIGINT NOT NULL DEFAULT 0);";
                string createTableTilesQuery = "CREATE TABLE " + tableTiles + " (MapID INT NOT NULL, x INT NOT NULL, y INT NOT NULL, zoom INT NOT NULL, HashCode BIGINT NOT NULL, LastUpdate BIGINT NOT NULL, Format INT NOT NULL, Image MEDIUMBLOB, NewImage MEDIUMBLOB, PRIMARY KEY(MapID, x, y, zoom));";
                string createTableFacesQuery = "CREATE TABLE " + tableFaces + " (PlayerName VARCHAR(64) NOT NULL, TypeID INT NOT NULL, Image MEDIUMBLOB, PRIMARY KEY(PlayerName, TypeID));";
                string createTableMarkerIconsQuery = "CREATE TABLE " + tableMarkerIcons + " (IconName VARCHAR(128) PRIMARY KEY NOT NULL, Image MEDIUMBLOB);";
                string createTableMarkerFilesQuery = "CREATE TABLE " + tableMarkerFiles + " (FileName VARCHAR(128) PRIMARY KEY NOT NULL, Content MEDIUMTEXT);";
                string createTableStandaloneFilesQuery = "CREATE TABLE " + tableStandaloneFiles + " (FileName VARCHAR(128) NOT NULL, ServerID BIGINT NOT NULL DEFAULT 0, Content MEDIUMTEXT, PRIMARY KEY (FileName, ServerID));";
                string createTableMapsIndexQuery = "CREATE INDEX " + tableMaps + "_idx ON " + tableMaps + "(WorldID, MapID, Variant, ServerID);";
                string createTableSchemaVersionQuery = "CREATE TABLE " + tableSchemaVersion + " (level INT PRIMARY KEY NOT NULL);";
                string setTableSchemaVersionValueQuery = "INSERT INTO " + tableSchemaVersion + " (level) VALUES (6);";

                string[] queries = { createTableMapsQuery, createTableTilesQuery, createTableFacesQuery, createTableMarkerIconsQuery, createTableMarkerFilesQuery, createTableStandaloneFilesQuery, createTableMapsIndexQuery, createTableSchemaVersionQuery, setTableSchemaVersionValueQuery };
                Console.WriteLine(queries);
                foreach (var query in queries)
                {
                    Console.WriteLine(query);

                    MySqlCommand cmd = new MySqlCommand(query, Connection);
                    var result = cmd.ExecuteScalar();
                }
            }

            return true;
        }
        public bool SendData(int mapID, int x, int y, int zoom, byte[] NewImage)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
                string insertMapQuery = "INSERT INTO " + tableTiles + "(mapID, x, y, HashCode, LastUpdate, Format, zoom, NewImage) VALUES('" + mapID + "','" + x + "','" + y + "',0,0,1,'" + zoom + "',?Images);";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, Connection);
            MySqlParameter parImage = new MySqlParameter
            {
                ParameterName = "?Images",
                MySqlDbType = MySqlDbType.MediumBlob,
                Size = 3000000,
                Value = NewImage//here you should put your byte []
            };
            mySqlCommand.Parameters.Add(parImage);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public void Close()
        {
            Connection.Close();
        }  
    }
    public class CustomSearcher
    {
        public static List<string> GetDirectories(string path, SearchOption searchOption = SearchOption.AllDirectories, string searchPattern = "*")
        {
            if (searchOption == SearchOption.TopDirectoryOnly)
                return Directory.GetDirectories(path, searchPattern).ToList();

            var directories = new List<string>(GetDirectories(path, searchPattern));

            for (var i = 0; i < directories.Count; i++)
                directories.AddRange(GetDirectories(directories[i], searchPattern));

            return directories;
        }

        private static List<string> GetDirectories(string path, string searchPattern)
        {
            try
            {
                return Directory.GetDirectories(path, searchPattern).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("UnauthorizedAccessException check folder permissions");
                return new List<string>();
            }
        }
    }
    public class GetConfigFile
    {
        public string getDynmapConfig(string path)
        {
            // use the correct seperator for the environment
            var pathParts = path.Split(Path.DirectorySeparatorChar);

            // this assumes a case sensitive check. If you don't want this, you may want to loop through the pathParts looking
            // for your "startAfterPath" with a StringComparison.OrdinalIgnoreCase check instead
            int startFrom = Array.IndexOf(pathParts, "dynmap");
            startFrom += 1;
            if (startFrom == -1)
            {
                // path not found
                return null;
            }
            var configDir = string.Join(Path.DirectorySeparatorChar.ToString(), pathParts, 0, startFrom);
            var configFile = string.Join(Path.DirectorySeparatorChar.ToString(), configDir, "configuration.txt");
            return configFile;
        }
    }
}
