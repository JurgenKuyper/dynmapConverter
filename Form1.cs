﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Data.SQLite;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace dynmapConverter
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
                if (_itemFrom == "FileTree" || _itemFrom == "SQLite")
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
                if (_itemTo == "FileTree" || _itemTo == "SQLite")
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
        private MysqlDBConnection dbC = MysqlDBConnection.Instance();
        private SQLiteDBConnection SQLite = SQLiteDBConnection.Instance();
        private SQLiteConnection Connection;
        private int mapsCount { get; set; }
        private int totalFoldersCount { get; set; }
        private int foldersCount { get; set; }
        private string configDirFileLocation { get; set; }
        private string configDirLocation { get; set; }
        private string defTemplateSuffix { get; set; }
        private GetConfigFile getConfig = new GetConfigFile();
        //private GetMCData GMD = new GetMCData();
        internal delegate void UpdateProgressDelegate(int ProgressPercentage);
        internal event UpdateProgressDelegate UpdateProgress;

        internal delegate void UpdateStatusDelegate(string status);
        internal event UpdateStatusDelegate UpdateStatus;
        public void StartConversion()
        {
            fromPath = "C:\\Users\\Jurgen\\dynmap-servers\\paper-1.19\\plugins\\dynmap\\web\\tiles";
            configDirFileLocation = getConfig.getDynmapConfig(fromPath);
            configDirLocation = getConfig.getDynmapConfigFolder(fromPath);
            Console.WriteLine(configDirFileLocation);
            var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
            dynamic myConfig = deserializer.Deserialize<ExpandoObject>(File.ReadAllText(configDirFileLocation));

            foreach (var item in myConfig)
            {
                if (item.Key == "storage")
                {
                    foreach (var entry in item.Value)
                    {
                        if (entry.Key == "prefix")
                        {
                            dbC.prefix = entry.Value;
                        }
                        else if (entry.Key == "dbfile")
                        {
                            SQLite.dbfile = string.Join(Path.DirectorySeparatorChar.ToString(), configDirLocation, entry.Value);
                            Console.WriteLine(SQLite.dbfile);
                        }
                    }
                }
                else if (item.Key == "deftemplatesuffix")
                {
                    defTemplateSuffix = item.Value;
                }
            }
            if (start && from != null && to != null)
            {
                if (from == "FileTree" && to == "FileTree")
                {
                    int currentFileValue = 0, currentFolderValue = 0;
                    totalFoldersCount = Directory.GetDirectories(fromPath,"*", SearchOption.AllDirectories).Count();
                    int totalFilesCount = Directory.GetFiles(fromPath, "*", SearchOption.AllDirectories).Count();
                    // Now Create all of the directories
                    UpdateStatus("Stage 1 of 2; creating Directories");
                    foreach (string dirPath in Directory.GetDirectories(fromPath, "*", SearchOption.AllDirectories))
                    {
                        currentFolderValue++;
                        Directory.CreateDirectory(dirPath.Replace(fromPath, toPath));
                        UpdateProgress(currentFolderValue * 100 / totalFoldersCount);
                        Application.DoEvents();
                    }
                    UpdateStatus("Stage 2 of 2; copying files");
                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(fromPath, "*.*", SearchOption.AllDirectories))
                    {
                        currentFileValue++;
                        File.Copy(newPath, newPath.Replace(fromPath, toPath), true);
                        UpdateProgress(currentFileValue * 100 / totalFilesCount);
                        Application.DoEvents();
                    }
                }
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
                    UpdateStatus("Stage 1 of 2; creating MySQL tables ");
                    Application.DoEvents();
                    dbC.InitDatabase();
                    foreach (var d in directories)
                    {
                        Console.WriteLine(d);
                        foreach (var folder in CustomSearcher.GetDirectories(d, SearchOption.TopDirectoryOnly)) //<worldname>/mapsnames
                        {
                            totalFoldersCount += (CustomSearcher.GetDirectories(folder, SearchOption.TopDirectoryOnly)).Count();
                        }
                    }
                    foreach (var d in directories)
                    {
                        string worldName = new DirectoryInfo(d).Name;
                        UpdateStatus("Stage 2 of 2; uploading image files");
                        Application.DoEvents();
                        foreach (var folder in CustomSearcher.GetDirectories(d, SearchOption.TopDirectoryOnly)) //<worldname>/mapsnames
                        {
                            mapsCount++;
                            string mapName = new DirectoryInfo(folder).Name;
                            dbC.SendDataMaps(mapsCount, worldName, mapName);
                            foreach (var MCAFolder in CustomSearcher.GetDirectories(folder, SearchOption.TopDirectoryOnly)) //<worldname>/mapmape/mcaTileFolder
                            {
                                foldersCount++;
                                //Console.WriteLine(GMD.getMaps(configDirLocation, defTemplateSuffix, d));
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
                                    dbC.SendDataTiles(mapsCount, x, y, zoomLevel, imageData);
                                    UpdateProgress(foldersCount * 100 / totalFoldersCount);

                                }
                                Console.WriteLine(MCAFolder);
                                Application.DoEvents();
                            }
                        }
                        dbC.Close();
                    }
                    UpdateStatus("Done!");
                    Application.DoEvents();
                    //MessageBox.Show(mapsCount.ToString());
                }
                if (from == "FileTree" && to == "SQLite")
                {
                    if (string.IsNullOrEmpty(SQLite.dbfile))
                    {
                        MessageBox.Show("Cannot connect to server: DBFile not configured in configuration.txt");
                    }
                    else
                    {
                        Connection = SQLite.IsConnect;
                        UpdateStatus("Stage 1 of 2; creating SQLite tables ");
                        Application.DoEvents();
                        MessageBox.Show("sucessfully connected to: " + Connection.FileName);
                        SQLite.CreateTable(Connection);
                        var directories = CustomSearcher.GetDirectories(fromPath, SearchOption.TopDirectoryOnly); //tiles/<worldname>
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
                            UpdateStatus("Stage 2 of 2; uploading image files");
                            Application.DoEvents();
                            string worldName = new DirectoryInfo(d).Name;
                            foreach (var folder in CustomSearcher.GetDirectories(d, SearchOption.TopDirectoryOnly)) //<worldname>/mapsnames
                            {
                                mapsCount++;
                                string mapName = new DirectoryInfo(folder).Name;
                                SQLite.SendDataMaps(Connection, mapsCount, worldName, mapName);
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
                                        SQLite.SendDataTiles(Connection, mapsCount, x, y, zoomLevel, imageData.Length, imageData);
                                    }
                                    Console.WriteLine(MCAFolder);
                                    UpdateProgress(foldersCount * 100 / totalFoldersCount);
                                    Application.DoEvents();
                                }
                            }
                        }
                        UpdateStatus("Done!");
                        Application.DoEvents();
                    }
                }
                if (from == "SQLite" && to == "FileTree")
                {
                    //TODO
                }
                if (from == "SQLite" && to == "MySQL")
                {
                    dbC.Password = msPwd;
                    dbC.UserName = msUser;
                    dbC.Server = msAddr;
                    dbC.DatabaseName = msDb;
                    if (dbC.IsConnect())
                    {
                        MessageBox.Show("sucessfully connected to: " + dbC.Server);
                    }
                    dbC.InitDatabase();
                    if (string.IsNullOrEmpty(SQLite.dbfile))
                    {
                        MessageBox.Show("Cannot connect to server: DBFile not configured in configuration.txt");
                    }
                    else
                    {
                        Connection = SQLite.IsConnect;
                        MessageBox.Show("sucessfully connected to: " + Connection.FileName);
                        SQLite.CreateTable(Connection);
                    }
                    DataTable dt = SQLite.ReadData(Connection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataFaces((string)row["PlayerName"], (int)row["TypeID"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = SQLite.ReadData(Connection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMaps((long)row["ID"],(string)row["WorldID"], (string)row["MapID"], (string)row["Variant"]);
                    }
                    dt = SQLite.ReadData(Connection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMarkerFiles((string)row["FileName"], (string)row["Content"]);
                    }
                    dt = SQLite.ReadData(Connection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMarkerIcons((string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = SQLite.ReadData(Connection, "SchemaVersion");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataSchemaVersion((int)row["level"]);
                    }
                    dt = SQLite.ReadData(Connection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataTiles((int)row["MapID"], (int)row["x"], (int)row["y"], (int)row["zoom"], (byte[])row["Image"]);
                    }
                }
                if (from == "SQLite" && to == "SQLite")
                {
                    //TODO
                }
                if (from == "MySQL" && to == "FileTree")
                {
                    //TODO
                }
                if (from == "MySQL" && to == "SQLite")
                {
                    //TODO
                }
                if (from == "MySQL" && to == "MySQL")
                {
                    //TODO
                }
            }
        }
    }

    public class MysqlDBConnection
    {
        private MysqlDBConnection()
        {
        }
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        private MySqlConnection Connection { get; set; }

        private static MysqlDBConnection _instance = null;
        public string prefix { get; set; }
        private string tableTiles;
        private string tableMaps;
        private string tableFaces;
        private string tableMarkerIcons;
        private string tableMarkerFiles;
        private string tableStandaloneFiles;
        private string tableSchemaVersion;
        public static MysqlDBConnection Instance()
        {
            if (_instance == null)
                _instance = new MysqlDBConnection();
            return _instance;
        }
        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (string.IsNullOrEmpty(DatabaseName))
                    return false;
                //Console.WriteLine("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
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
                //Console.WriteLine(queries);
                foreach (var query in queries)
                {
                    Console.WriteLine(query);

                    MySqlCommand cmd = new MySqlCommand(query, Connection);
                    var result = cmd.ExecuteScalar();
                }
            }

            return true;
        }
        public bool SendDataFaces(string playerName, int typeID, byte[] image, int imageLen)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            string insertMapQuery = "INSERT INTO " + tableFaces + "(PlayerName, TypeID, Image, ImageLen) VALUES('" + playerName + "','" + typeID + "',?Images'" + imageLen + "');";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, Connection);
            MySqlParameter parImage = new MySqlParameter
            {
                ParameterName = "?Images",
                MySqlDbType = MySqlDbType.MediumBlob,
                Size = 3000000,
                Value = image//here you should put your byte []
            };
            mySqlCommand.Parameters.Add(parImage);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataMaps(long ID, string worldID, string mapName, string variant = "STANDARD", long serverID = 0)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            string insertMapQuery = "INSERT INTO " + tableMaps + "(ID, WorldID, MapID, Variant, ServerID) VALUES('" + ID + "','" + worldID + "','" + mapName + "','" + variant + "','" + serverID + "');";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, Connection);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }

        public bool SendDataMarkerFiles(string fileName, string content)
        {
            string insertMapQuery = "INSERT INTO " + tableMarkerFiles + "(FileName, Content) VALUES('" + fileName + "','" + content + "');";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, Connection);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataMarkerIcons(string iconName, byte[] image, int imageLen)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            string insertMapQuery = "INSERT INTO " + tableMarkerIcons + "(IconName, Image, ImageLen) VALUES('" + iconName + "',?Images'" + imageLen + "');";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, Connection);
            MySqlParameter parImage = new MySqlParameter
            {
                ParameterName = "?Images",
                MySqlDbType = MySqlDbType.Blob,
                Size = 3000000,
                Value = image//here you should put your byte []
            };
            mySqlCommand.Parameters.Add(parImage);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataSchemaVersion(int level)
        {
            string insertMapQuery = "INSERT INTO " + tableSchemaVersion + "(level) VALUES('" + level + "');";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, Connection);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataTiles(int mapID, int x, int y, int zoom, byte[] NewImage)
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

    public class SQLiteDBConnection
    {
        public string dbfile { get; set; }

        private SQLiteConnection Connection { get; set; }
        private static SQLiteDBConnection _instance = null;
        public static SQLiteDBConnection Instance()
        {
            if (_instance == null)
                _instance = new SQLiteDBConnection();
            return _instance;
        }
        public SQLiteConnection IsConnect
        {
            get
            {
                if (Connection == null)
                {
                    SQLiteConnection sqlite_conn;
                    // Create a new database connection:
                    sqlite_conn = new SQLiteConnection("Data Source=" + dbfile + ";New=False;");
                    try
                    {
                        sqlite_conn.Open();
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Cannot connect to server: " + ex);
                    }
                    return sqlite_conn;
                }
                else { return Connection; }
            }
        }

        static SQLiteConnection CreateConnection(string database)
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=" + database + ";");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return sqlite_conn;
        }

        public bool CreateTable(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='SchemaVersion'";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            Int64 resultCheck = 0;
            while (sqlite_datareader.Read())
            {
                resultCheck = sqlite_datareader.GetInt64(0);
            }
            if (resultCheck != 1)
            {
                string createTableMapsQuery = "CREATE TABLE Maps (ID INTEGER PRIMARY KEY AUTOINCREMENT, WorldID STRING NOT NULL, MapID STRING NOT NULL, Variant STRING NOT NULL)";
                string createTableTilesQuery = "CREATE TABLE Tiles (MapID INT NOT NULL, x INT NOT NULL, y INT NOT NULL, zoom INT NOT NULL, HashCode INT NOT NULL, LastUpdate INT NOT NULL, Format INT NOT NULL, Image BLOB, ImageLen INT, PRIMARY KEY(MapID, x, y, zoom))";
                string createTableFacesQuery = "CREATE TABLE Faces (PlayerName STRING NOT NULL, TypeID INT NOT NULL, Image BLOB, ImageLen INT, PRIMARY KEY(PlayerName, TypeID))";
                string createTableMarkerIconsQuery = "CREATE TABLE MarkerIcons (IconName STRING PRIMARY KEY NOT NULL, Image BLOB, ImageLen INT)";
                string createTableMarkerFilesQuery = "CREATE TABLE MarkerFiles (FileName STRING PRIMARY KEY NOT NULL, Content CLOB)";
                // Add index, since SQLite execution planner is stupid and scans Tiles table instead of using short Maps table...
                string createIndexMapindexQuery = "CREATE INDEX MapIndex ON Maps(WorldID, MapID, Variant)";
                string createTableSchemaVersionQuery = "CREATE TABLE SchemaVersion (level INT PRIMARY KEY NOT NULL)";
                string insertSchemaVersionQuery = "INSERT INTO SchemaVersion (level) VALUES (3)";
                string[] queries = { createTableMapsQuery, createTableTilesQuery, createTableFacesQuery, createTableMarkerIconsQuery, createTableMarkerFilesQuery, createIndexMapindexQuery, createTableSchemaVersionQuery, insertSchemaVersionQuery };
                sqlite_cmd = conn.CreateCommand();
                foreach (var query in queries)
                {
                    Console.WriteLine(query);
                    sqlite_cmd.CommandText = query;
                    sqlite_cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public bool SendDataTiles(SQLiteConnection conn, int mapID, int x, int y, int zoom, int imageLength, byte[] NewImage)
        {
            string insertMapQuery = "INSERT INTO Tiles(mapID, x, y, zoom, HashCode, LastUpdate, Format, Image, ImageLen) VALUES('" + mapID + "','" + x + "','" + y + "','" + zoom + "',0,0,1,@Images," + imageLength + ")";
            //string insertMapQuery = "update Tiles set Image = ?Images where MapID = 1;";
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = insertMapQuery;
            SQLiteParameter parImage = new SQLiteParameter
            {
                ParameterName = "@Images",
                DbType = DbType.Binary,
                Size = 3000000,
                Value = NewImage//here you should put your byte []
            };
            sqlite_cmd.Parameters.Add(parImage);
            sqlite_cmd.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataMaps(SQLiteConnection conn,int ID, string worldID, string mapName, string variant = "STANDARD", long serverID = 0)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            string insertMapQuery = "INSERT INTO Maps (ID, WorldID, MapID, Variant, ServerID) VALUES('" + ID + "','" + worldID + "','" + mapName + "','" + variant + "','" + serverID + "');";
            SQLiteCommand SQLiteCommand = new SQLiteCommand(insertMapQuery, conn);
            SQLiteCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public DataTable ReadData(SQLiteConnection conn, string table)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM " + table;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());
                //Add your exception code here.
            }
            return dt;
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
        public string getDynmapConfigFolder(string path)
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
            return configDir;
        }
    }
    //public class GetMCData
    //{
    //    string filePrefix { get; set; }
    //    string[] worldPrefixes { get; set; }
    //    public string getMaps(string dynmapConfigFolder, string defTemplateSuffix, string world)
    //    {
    //        if (world.Contains("nether"))
    //            filePrefix = "nether";
    //        else if (world.Contains("the_end"))
    //            filePrefix = "the_end";
    //        else
    //            filePrefix = "normal";
    //        var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
    //        dynamic WorldConfig = deserializer.Deserialize<ExpandoObject>(File.ReadAllText(string.Join(Path.DirectorySeparatorChar.ToString(), dynmapConfigFolder, "templates", string.Join("-", filePrefix, defTemplateSuffix + ".txt"))));
    //        foreach (var item in WorldConfig)
    //        {
    //            if (item.Key == "templates")
    //            {
    //                foreach (var entry in item.Value)
    //                {
    //                    if (entry.Key == string.Join("-", filePrefix, defTemplateSuffix))
    //                    {
    //                        foreach (var option in entry.Value)
    //                        {
    //                            if (option.Key == "maps")
    //                            {
    //                                foreach (var mapConfig in option.Value)
    //                                {
    //                                    //if (mapConfig.Key == "prefix")
    //                                    //{
    //                                    //    worldPrefixes += entry.Value;
    //                                    //    Console.WriteLine(entry.Value);
    //                                    //}
    //                                    foreach(var mapInfo in mapConfig)
    //                                    {
    //                                        if(mapInfo.Key == "prefix")
    //                                            Console.WriteLine(mapInfo.Value);
    //                                    }

    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        return "true";
    //    }
    //}
}
