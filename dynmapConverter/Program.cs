using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Dynamic;
using MySql.Data.MySqlClient;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace dynmapConverter
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
    public class Convert
    {
        public string from { get; set; }
        public string fromPath { get; set; }
        public string to { get; set; }
        public string toPath { get; set; }
        public string fromDbFile { get; set; }
        public string toDbFile { get; set; }
        public string fromMsUser { get; set; }
        public string fromMsPwd { get; set; }
        public string fromMsAddr { get; set; }
        public string fromMsDb { get; set; }
        public string toMsUser { get; set; }
        public string toMsPwd { get; set; }
        public string toMsAddr { get; set; }
        public string toMsDb { get; set; }
        public bool start { get; set; }

        private MysqlDBConnection dbC = MysqlDBConnection.Instance();
        private SQLiteDBConnection SQLite = SQLiteDBConnection.Instance();
        private SqlDBConnection sqlC = SqlDBConnection.Instance();

        private MySqlConnection fromMySqlConnection;
        private SQLiteConnection fromSQLiteConnection;
        private SqlConnection fromSqlConnection;

        private MySqlConnection toMySqlConnection;
        private SQLiteConnection toSQLiteConnection;
        private SqlConnection toSqlConnection;
        private int mapsCount { get; set; }
        private int totalFoldersCount { get; set; }
        private int foldersCount { get; set; }
        private string configDirFileLocation { get; set; }
        private string configDirLocation { get; set; }
        private string defTemplateSuffix { get; set; }
        public enum ImageFormat
        {
            png,
            jpg,
            webp
        }
        private GetConfigFile getConfig = new GetConfigFile();
        internal delegate void UpdateProgressDelegate(int ProgressPercentage);
        internal event UpdateProgressDelegate UpdateProgress;

        internal delegate void UpdateStatusDelegate(string status);
        internal event UpdateStatusDelegate UpdateStatus;
        interface iStorage
        {

        }
        public void StartConversion()
        {
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
                            fromDbFile = string.Join(Path.DirectorySeparatorChar.ToString(), configDirLocation, entry.Value);
                            Console.WriteLine(fromDbFile);
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
                    totalFoldersCount = Directory.GetDirectories(fromPath, "*", SearchOption.AllDirectories).Count();
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
                    UpdateStatus("Done!");
                    Application.DoEvents();
                }
                if (from == "FileTree" && to == "MySQL")
                {
                    dbC.Password = toMsPwd;
                    dbC.UserName = toMsUser;
                    dbC.Server = toMsAddr;
                    dbC.DatabaseName = toMsDb;
                    var directories = Directory.GetDirectories(fromPath, "*", SearchOption.TopDirectoryOnly); //tiles/<worldname>
                    toMySqlConnection = dbC.IsConnect(toMySqlConnection);
                    MessageBox.Show("sucessfully connected to: " + dbC.Server);
                    UpdateStatus("Stage 1 of 2; creating MySQL tables ");
                    Application.DoEvents();
                    dbC.InitDatabase(toMySqlConnection);
                    foreach (var d in directories)
                    {
                        Console.WriteLine(d);
                        foreach (var folder in Directory.GetDirectories(d, "*", SearchOption.TopDirectoryOnly)) //<worldname>/mapsnames
                        {
                            totalFoldersCount += (Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly)).Count();
                        }
                    }
                    foreach (var d in directories)
                    {
                        string worldName = new DirectoryInfo(d).Name;
                        UpdateStatus("Stage 2 of 2; uploading image files");
                        Application.DoEvents();
                        foreach (var folder in Directory.GetDirectories(d, "*", SearchOption.TopDirectoryOnly)) //<worldname>/mapsnames
                        {
                            mapsCount++;
                            string mapName = new DirectoryInfo(folder).Name;
                            dbC.SendDataMaps(toMySqlConnection, mapsCount, worldName, mapName);
                            foreach (var MCAFolder in Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly)) //<worldname>/mapmape/mcaTileFolder
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
                                    dbC.SendDataTiles(toMySqlConnection, mapsCount, x, y, zoomLevel, imageData);
                                    UpdateProgress(foldersCount * 100 / totalFoldersCount);

                                }
                                Console.WriteLine(MCAFolder);
                                Application.DoEvents();
                            }
                        }
                    }
                    dbC.Close(toMySqlConnection);
                    UpdateStatus("Done!");
                    Application.DoEvents();
                    //MessageBox.Show(mapsCount.ToString());
                }
                if (from == "FileTree" && to == "SQLite")
                {
                    if (string.IsNullOrEmpty(fromDbFile))
                    {
                        MessageBox.Show("Cannot connect to server: dbfile not configured in configuration.txt");
                    }
                    else
                    {
                        toSQLiteConnection = SQLite.IsConnect(toDbFile, toSQLiteConnection);
                        UpdateStatus("Stage 1 of 2; creating SQLite tables ");
                        Application.DoEvents();
                        MessageBox.Show("sucessfully connected to: " + toSQLiteConnection.FileName);
                        SQLite.InitDatabase(toSQLiteConnection);
                        var directories = Directory.GetDirectories(fromPath, "*", SearchOption.TopDirectoryOnly); //tiles/<worldname>
                        foreach (var d in directories)
                        {
                            Console.WriteLine(d);
                            foreach (var folder in Directory.GetDirectories(d, "*", SearchOption.TopDirectoryOnly)) //<worldname>/mapsnames
                            {
                                foreach (var MCAFolder in Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly))
                                    totalFoldersCount++;
                            }
                        }
                        foreach (var d in directories)
                        {
                            Console.WriteLine(d);
                            UpdateStatus("Stage 2 of 2; uploading image files");
                            Application.DoEvents();
                            string worldName = new DirectoryInfo(d).Name;
                            foreach (var folder in Directory.GetDirectories(d, "*", SearchOption.TopDirectoryOnly)) //<worldname>/mapsnames
                            {
                                mapsCount++;
                                string mapName = new DirectoryInfo(folder).Name;
                                SQLite.SendDataMaps(toSQLiteConnection, mapsCount, worldName, mapName);
                                foreach (var MCAFolder in Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly)) //<worldname>/mapmape/mcaTileFolder
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
                                        SQLite.SendDataTiles(toSQLiteConnection, mapsCount, x, y, zoomLevel, imageData.Length, imageData);
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
                if (from == "FileTree" && to == "MSSQL")
                {
                    sqlC.Password = toMsPwd;
                    sqlC.UserName = toMsUser;
                    sqlC.Server = toMsAddr;
                    sqlC.DatabaseName = toMsDb;
                    var directories = Directory.GetDirectories(fromPath, "*", SearchOption.TopDirectoryOnly); //tiles/<worldname>
                    toSqlConnection = sqlC.IsConnect(toSqlConnection);
                    MessageBox.Show("sucessfully connected to: " + dbC.Server);
                    UpdateStatus("Stage 1 of 2; creating MySQL tables ");
                    Application.DoEvents();
                    sqlC.InitDatabase(toSqlConnection);
                    foreach (var d in directories)
                    {
                        Console.WriteLine(d);
                        foreach (var folder in Directory.GetDirectories(d, "*", SearchOption.TopDirectoryOnly)) //<worldname>/mapsnames
                        {
                            totalFoldersCount += (Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly)).Count();
                        }
                    }
                    foreach (var d in directories)
                    {
                        string worldName = new DirectoryInfo(d).Name;
                        UpdateStatus("Stage 2 of 2; uploading image files");
                        Application.DoEvents();
                        foreach (var folder in Directory.GetDirectories(d, "*", SearchOption.TopDirectoryOnly)) //<worldname>/mapsnames
                        {
                            mapsCount++;
                            string mapName = new DirectoryInfo(folder).Name;
                            sqlC.SendDataMaps(toSqlConnection, mapsCount, worldName, mapName);
                            foreach (var MCAFolder in Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly)) //<worldname>/mapmape/mcaTileFolder
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
                                    sqlC.SendDataTiles(toSqlConnection, mapsCount, x, y, zoomLevel, imageData);
                                    UpdateProgress(foldersCount * 100 / totalFoldersCount);

                                }
                                Console.WriteLine(MCAFolder);
                                Application.DoEvents();
                            }
                        }
                    }
                    sqlC.Close(toSqlConnection);
                    UpdateStatus("Done!");
                    Application.DoEvents();
                    //MessageBox.Show(mapsCount.ToString());
                }
                if (from == "SQLite" && to == "FileTree")
                {
                    fromSQLiteConnection = SQLite.IsConnect(fromDbFile, fromSQLiteConnection);
                    MessageBox.Show("sucessfully connected to: " + fromSQLiteConnection.FileName);
                    UpdateStatus("Stage 1 of 5; Creating maps directories ");
                    Application.DoEvents();
                    List<string> mapNames = new List<string>();
                    mapNames.Add("initial offset");
                    DataTable dt = SQLite.ReadData(fromSQLiteConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        DirectoryInfo di = Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, (string)row["WorldID"], (string)row["MapID"]));
                        mapNames.Add(di.FullName);
                    }
                    UpdateStatus("Stage 2 of 5; Creating and filling Skins directory");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        switch ((int)row["TypeID"])
                        {
                            case 0:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "8x8"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "8x8", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write((byte[])row["Image"], 0, (int)row["ImageLen"]);
                                }
                                break;
                            case 1:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "16x16"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "16x16", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write((byte[])row["Image"], 0, (int)row["ImageLen"]);
                                }
                                break;
                            case 2:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "32x32"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "32x32", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write((byte[])row["Image"], 0, (int)row["ImageLen"]);
                                }
                                break;
                            case 3:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "body"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "body", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write((byte[])row["Image"], 0, (int)row["ImageLen"]);
                                }
                                break;
                        }
                    }
                    UpdateStatus("Stage 3 of 5; importing Marker File data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "_markers_"));
                        File.WriteAllText(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "_markers_", "marker_" + (string)row["FileName"]) + ".json", (string)row["Content"]);
                    }
                    UpdateStatus("Stage 4 of 5; importing Marker Icon data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "_markers_", (string)row["IconName"]) + ".png", FileMode.Create, FileAccess.Write))
                        {
                            fs.Write((byte[])row["Image"], 0, (int)row["ImageLen"]);
                        }
                        //dbC.SendDataMarkerIcons(toMySqlConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    UpdateStatus("Stage 5 of 5; importing Tiles data ");
                    dt = SQLite.ReadData(fromSQLiteConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        UpdateProgress(dt.Rows.IndexOf(row) * 100 / dt.Rows.Count);
                        Application.DoEvents();
                        Console.WriteLine(row.Field<int>("MapID").ToString());
                        Console.WriteLine(mapNames[row.Field<int>("MapID")]);
                        Console.WriteLine();
                        int imageFormat = 0;
                        switch ((int)row["Format"])
                        {
                            case 1:
                                imageFormat = 0;
                                break;
                            case 2:
                                imageFormat = 1;
                                break;
                            case 3:
                                imageFormat = 1;
                                break;
                            case 4:
                                imageFormat = 1;
                                break;
                            case 5:
                                imageFormat = 1;
                                break;
                            case 6:
                                imageFormat = 1;
                                break;
                            case 7:
                                imageFormat = 1;
                                break;
                            case 8:
                                imageFormat = 1;
                                break;
                            case 9:
                                imageFormat = 2;
                                break;
                            case 10:
                                imageFormat = 2;
                                break;
                            case 11:
                                imageFormat = 2;
                                break;
                            case 12:
                                imageFormat = 2;
                                break;
                            case 13:
                                imageFormat = 2;
                                break;
                            case 14:
                                imageFormat = 2;
                                break;
                            case 15:
                                imageFormat = 2;
                                break;
                            case 16:
                                imageFormat = 2;
                                break;
                        }
                        string subFolderName = string.Join("_", (int)row["x"] >> 5, (int)row["y"] >> 5);
                        DirectoryInfo tilesDi = Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), mapNames[row.Field<int>("MapID")], subFolderName));
                        using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), tilesDi.FullName, (int)row["x"] + "_" + (int)row["y"] + "." + (ImageFormat)imageFormat), FileMode.Create, FileAccess.Write))
                        {
                            fs.Write((byte[])row["Image"], 0, (int)row["ImageLen"]);
                        }
                    }
                    UpdateStatus("Finished conversion");
                    UpdateProgress(100);
                }
                if (from == "SQLite" && to == "MySQL")
                {
                    dbC.Password = toMsPwd;
                    dbC.UserName = toMsUser;
                    dbC.Server = toMsAddr;
                    dbC.DatabaseName = toMsDb;

                    toMySqlConnection = dbC.IsConnect(toMySqlConnection);
                    MessageBox.Show("sucessfully connected to: " + dbC.Server);
                    UpdateStatus("Stage 1 of 7; creating MySQL tables ");
                    Application.DoEvents();
                    dbC.InitDatabase(toMySqlConnection);
                    fromSQLiteConnection = SQLite.IsConnect(fromDbFile, fromSQLiteConnection);
                    MessageBox.Show("sucessfully connected to: " + fromSQLiteConnection.FileName);
                    UpdateStatus("Stage 2 of 7; importing faces data ");
                    Application.DoEvents();
                    DataTable dt = SQLite.ReadData(fromSQLiteConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataFaces(toMySqlConnection, (string)row["PlayerName"], (int)row["TypeID"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    UpdateStatus("Stage 3 of 7; importing Maps data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMaps(toMySqlConnection, (int)row["ID"], (string)row["WorldID"], (string)row["MapID"], (string)row["Variant"]);
                    }
                    UpdateStatus("Stage 4 of 7; importing Marker File data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMarkerFiles(toMySqlConnection, (string)row["FileName"], (string)row["Content"]);
                    }
                    UpdateStatus("Stage 5 of 7; importing Marker Icon data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMarkerIcons(toMySqlConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    UpdateStatus("Stage 6 of 7; importing Schema Version data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "SchemaVersion");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataSchemaVersion(toMySqlConnection, (int)row["level"]);
                    }
                    UpdateStatus("Stage 7 of 7; importing Tiles data ");
                    dt = SQLite.ReadData(fromSQLiteConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        UpdateProgress(dt.Rows.IndexOf(row) * 100 / dt.Rows.Count);
                        Application.DoEvents();
                        dbC.SendDataTiles(toMySqlConnection, (int)row["MapID"], (int)row["x"], (int)row["y"], (int)row["zoom"], (byte[])row["Image"]);
                    }
                    UpdateStatus("Finished conversion");
                    UpdateProgress(100);
                }
                if (from == "SQLite" && to == "SQLite")
                {
                    UpdateStatus("Stage 1 of 7; creating sqlite tables ");
                    Application.DoEvents();
                    fromSQLiteConnection = SQLite.IsConnect(fromDbFile, fromSQLiteConnection);
                    toSQLiteConnection = SQLite.IsConnect(toDbFile, toSQLiteConnection);
                    MessageBox.Show("sucessfully connected to: " + fromSQLiteConnection.FileName + " and: " + toSQLiteConnection.FileName);
                    UpdateStatus("Stage 2 of 7; importing faces data ");
                    Application.DoEvents();
                    DataTable dt = SQLite.ReadData(fromSQLiteConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataFaces(toSQLiteConnection, (string)row["PlayerName"], (int)row["TypeID"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    UpdateStatus("Stage 3 of 7; importing Maps data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataMaps(toSQLiteConnection, (int)row["ID"], (string)row["WorldID"], (string)row["MapID"], (string)row["Variant"]);
                    }
                    UpdateStatus("Stage 4 of 7; importing Marker File data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataMarkerFiles(toSQLiteConnection, (string)row["FileName"], (string)row["Content"]);
                    }
                    UpdateStatus("Stage 5 of 7; importing Marker Icon data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataMarkerIcons(toSQLiteConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    UpdateStatus("Stage 6 of 7; importing Schema Version data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "SchemaVersion");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataSchemaVersion(toSQLiteConnection, (int)row["level"]);
                    }
                    UpdateStatus("Stage 7 of 7; importing Tiles data ");
                    dt = SQLite.ReadData(fromSQLiteConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        UpdateProgress(dt.Rows.IndexOf(row) * 100 / dt.Rows.Count);
                        Application.DoEvents();
                        SQLite.SendDataTiles(toSQLiteConnection, (int)row["MapID"], (int)row["x"], (int)row["y"], (int)row["zoom"], (int)row["ImageLen"], (byte[])row["Image"]);
                    }
                    UpdateStatus("Finished conversion");
                    UpdateProgress(100);
                }
                if (from == "SQLite" && to == "MSSQL")
                {
                    sqlC.Password = toMsPwd;
                    sqlC.UserName = toMsUser;
                    sqlC.Server = toMsAddr;
                    sqlC.DatabaseName = toMsDb;

                    toSqlConnection = sqlC.IsConnect(toSqlConnection);
                    MessageBox.Show("sucessfully connected to: " + sqlC.Server);
                    UpdateStatus("Stage 1 of 7; creating MySQL tables ");
                    Application.DoEvents();
                    sqlC.InitDatabase(toSqlConnection);
                    fromSQLiteConnection = SQLite.IsConnect(fromDbFile, fromSQLiteConnection);
                    MessageBox.Show("sucessfully connected to: " + fromSQLiteConnection.FileName);
                    UpdateStatus("Stage 2 of 7; importing faces data ");
                    Application.DoEvents();
                    DataTable dt = SQLite.ReadData(fromSQLiteConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataFaces(toSqlConnection, (string)row["PlayerName"], (int)row["TypeID"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    UpdateStatus("Stage 3 of 7; importing Maps data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataMaps(toSqlConnection, (int)row["ID"], (string)row["WorldID"], (string)row["MapID"], (string)row["Variant"]);
                    }
                    UpdateStatus("Stage 4 of 7; importing Marker File data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataMarkerFiles(toSqlConnection, (string)row["FileName"], (string)row["Content"]);
                    }
                    UpdateStatus("Stage 5 of 7; importing Marker Icon data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataMarkerIcons(toSqlConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    UpdateStatus("Stage 6 of 7; importing Schema Version data ");
                    Application.DoEvents();
                    dt = SQLite.ReadData(fromSQLiteConnection, "SchemaVersion");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataSchemaVersion(toSqlConnection, (int)row["level"]);
                    }
                    UpdateStatus("Stage 7 of 7; importing Tiles data ");
                    dt = SQLite.ReadData(fromSQLiteConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        UpdateProgress(dt.Rows.IndexOf(row) * 100 / dt.Rows.Count);
                        Application.DoEvents();
                        sqlC.SendDataTiles(toSqlConnection, (int)row["MapID"], (int)row["x"], (int)row["y"], (int)row["zoom"], (byte[])row["Image"]);
                    }
                    UpdateStatus("Finished conversion");
                    UpdateProgress(100);
                }
                if (from == "MySQL" && to == "FileTree")
                {
                    dbC.Password = fromMsPwd;
                    dbC.UserName = fromMsUser;
                    dbC.Server = fromMsAddr;
                    dbC.DatabaseName = fromMsDb;
                    fromMySqlConnection = dbC.IsConnect(fromMySqlConnection);
                    MessageBox.Show("sucessfully connected to: " + fromMySqlConnection.DataSource);
                    UpdateStatus("Stage 1 of 5; Creating maps directories ");
                    Application.DoEvents();
                    List<string> mapNames = new List<string>();
                    mapNames.Add("initial offset");
                    DataTable dt = dbC.ReadData(fromMySqlConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        DirectoryInfo di = Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, (string)row["WorldID"], (string)row["MapID"]));
                        mapNames.Add(di.FullName);
                    }
                    UpdateStatus("Stage 2 of 5; Creating and filling Skins directory");
                    Application.DoEvents();
                    dt = dbC.ReadData(fromMySqlConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        switch ((int)row["TypeID"])
                        {
                            case 0:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "8x8"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "8x8", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write(row.Field<byte[]>("Image"), 0, row.Field<byte[]>("Image").Length);
                                }
                                break;
                            case 1:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "16x16"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "16x16", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write(row.Field<byte[]>("Image"), 0, row.Field<byte[]>("Image").Length);
                                }
                                break;
                            case 2:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "32x32"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "32x32", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write(row.Field<byte[]>("Image"), 0, row.Field<byte[]>("Image").Length);
                                }
                                break;
                            case 3:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "body"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "body", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write(row.Field<byte[]>("Image"), 0, row.Field<byte[]>("Image").Length);
                                }
                                break;
                        }
                    }
                    UpdateStatus("Stage 3 of 5; importing Marker File data ");
                    Application.DoEvents();
                    dt = dbC.ReadData(fromMySqlConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "_markers_"));
                        File.WriteAllText(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "_markers_", "marker_" + (string)row["FileName"]) + ".json", (string)row["Content"]);
                    }
                    UpdateStatus("Stage 4 of 5; importing Marker Icon data ");
                    Application.DoEvents();
                    dt = dbC.ReadData(fromMySqlConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "_markers_", (string)row["IconName"]) + ".png", FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(row.Field<byte[]>("Image"), 0, row.Field<byte[]>("Image").Length);
                        }
                        //dbC.SendDataMarkerIcons(toMySqlConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    UpdateStatus("Stage 5 of 5; importing Tiles data ");
                    dt = dbC.ReadData(fromMySqlConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        UpdateProgress(dt.Rows.IndexOf(row) * 100 / dt.Rows.Count);
                        Application.DoEvents();
                        Console.WriteLine(row.Field<int>("MapID").ToString());
                        Console.WriteLine(mapNames[row.Field<int>("MapID")]);
                        Console.WriteLine();
                        int imageFormat = 0;
                        switch ((int)row["Format"])
                        {
                            case 1:
                                imageFormat = 0;
                                break;
                            case 2:
                                imageFormat = 1;
                                break;
                            case 3:
                                imageFormat = 1;
                                break;
                            case 4:
                                imageFormat = 1;
                                break;
                            case 5:
                                imageFormat = 1;
                                break;
                            case 6:
                                imageFormat = 1;
                                break;
                            case 7:
                                imageFormat = 1;
                                break;
                            case 8:
                                imageFormat = 1;
                                break;
                            case 9:
                                imageFormat = 2;
                                break;
                            case 10:
                                imageFormat = 2;
                                break;
                            case 11:
                                imageFormat = 2;
                                break;
                            case 12:
                                imageFormat = 2;
                                break;
                            case 13:
                                imageFormat = 2;
                                break;
                            case 14:
                                imageFormat = 2;
                                break;
                            case 15:
                                imageFormat = 2;
                                break;
                            case 16:
                                imageFormat = 2;
                                break;
                        }
                        string subFolderName = string.Join("_", (int)row["x"] >> 5, (int)row["y"] >> 5);
                        DirectoryInfo tilesDi = Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), mapNames[row.Field<int>("MapID")], subFolderName));
                        using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), tilesDi.FullName, (int)row["x"] + "_" + (int)row["y"] + "." + (ImageFormat)imageFormat), FileMode.Create, FileAccess.Write))
                        {
                            if ((byte[])row["Image"] != null)
                            {
                                fs.Write((byte[])row["Image"], 0, (int)row["ImageLen"]);
                            }
                            else
                            {
                                fs.Write((byte[])row["NewImage"], 0, (int)row["ImageLen"]);
                            }
                        }
                    }
                    UpdateStatus("Finished conversion");
                    UpdateProgress(100);
                }
                if (from == "MySQL" && to == "SQLite")
                {
                    dbC.Password = fromMsPwd;
                    dbC.UserName = fromMsUser;
                    dbC.Server = fromMsAddr;
                    dbC.DatabaseName = fromMsDb;
                    fromMySqlConnection = dbC.IsConnect(fromMySqlConnection);
                    MessageBox.Show("sucessfully connected to: " + toSQLiteConnection.FileName);
                    SQLite.InitDatabase(toSQLiteConnection);
                    DataTable dt = dbC.ReadData(fromMySqlConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataFaces(toSQLiteConnection, (string)row["PlayerName"], (int)row["TypeID"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataMaps(toSQLiteConnection, (int)row["ID"], (string)row["WorldID"], (string)row["MapID"], (string)row["Variant"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataMarkerFiles(toSQLiteConnection, (string)row["FileName"], (string)row["Content"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataMarkerIcons(toSQLiteConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "SchemaVersion");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataSchemaVersion(toSQLiteConnection, (int)row["level"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataTiles(toSQLiteConnection, (int)row["MapID"], (int)row["x"], (int)row["y"], (int)row["zoom"], (int)row["ImageLen"], (byte[])row["Image"]);
                    }
                }
                if (from == "MySQL" && to == "MySQL")
                {
                    dbC.Password = fromMsPwd;
                    dbC.UserName = fromMsUser;
                    dbC.Server = fromMsAddr;
                    dbC.DatabaseName = fromMsDb;
                    fromMySqlConnection = dbC.IsConnect(fromMySqlConnection);
                    dbC.Password = toMsPwd;
                    dbC.UserName = toMsUser;
                    dbC.Server = toMsAddr;
                    dbC.DatabaseName = toMsDb;
                    toMySqlConnection = dbC.IsConnect(toMySqlConnection);
                    dbC.InitDatabase(toMySqlConnection);
                    MessageBox.Show("sucessfully connected to: " + fromMySqlConnection.DataSource + " and: " + fromMySqlConnection.DataSource);
                    dbC.InitDatabase(toMySqlConnection);
                    DataTable dt = dbC.ReadData(fromMySqlConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataFaces(toMySqlConnection, (string)row["PlayerName"], (int)row["TypeID"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMaps(toMySqlConnection, (int)row["ID"], (string)row["WorldID"], (string)row["MapID"], (string)row["Variant"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMarkerFiles(toMySqlConnection, (string)row["FileName"], (string)row["Content"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMarkerIcons(toMySqlConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "SchemaVersion");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataSchemaVersion(toMySqlConnection, (int)row["level"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataTiles(toMySqlConnection, (int)row["MapID"], (int)row["x"], (int)row["y"], (int)row["zoom"], (byte[])row["Image"]);
                    }
                }
                if (from == "MySQL" && to == "MSSQL")
                {
                    dbC.Password = fromMsPwd;
                    dbC.UserName = fromMsUser;
                    dbC.Server = fromMsAddr;
                    dbC.DatabaseName = fromMsDb;
                    fromMySqlConnection = dbC.IsConnect(fromMySqlConnection);
                    sqlC.Password = toMsPwd;
                    sqlC.UserName = toMsUser;
                    sqlC.Server = toMsAddr;
                    sqlC.DatabaseName = toMsDb;
                    toSqlConnection = sqlC.IsConnect(toSqlConnection);
                    sqlC.InitDatabase(toSqlConnection);
                    MessageBox.Show("sucessfully connected to: " + fromMySqlConnection.DataSource + " and: " + fromSqlConnection.DataSource);
                    DataTable dt = dbC.ReadData(fromMySqlConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataFaces(toSqlConnection, (string)row["PlayerName"], (int)row["TypeID"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataMaps(toSqlConnection, (int)row["ID"], (string)row["WorldID"], (string)row["MapID"], (string)row["Variant"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataMarkerFiles(toSqlConnection, (string)row["FileName"], (string)row["Content"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataMarkerIcons(toSqlConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "SchemaVersion");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataSchemaVersion(toSqlConnection, (int)row["level"]);
                    }
                    dt = dbC.ReadData(fromMySqlConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataTiles(toSqlConnection, (int)row["MapID"], (int)row["x"], (int)row["y"], (int)row["zoom"], (byte[])row["Image"]);
                    }
                }
                if (from == "MSSQL" && to == "FileTree")
                {
                    sqlC.Password = fromMsPwd;
                    sqlC.UserName = fromMsUser;
                    sqlC.Server = fromMsAddr;
                    sqlC.DatabaseName = fromMsDb;
                    fromSqlConnection = sqlC.IsConnect(fromSqlConnection);
                    MessageBox.Show("sucessfully connected to: " + fromSqlConnection.DataSource);
                    UpdateStatus("Stage 1 of 5; Creating maps directories ");
                    Application.DoEvents();
                    List<string> mapNames = new List<string>();
                    mapNames.Add("initial offset");
                    DataTable dt = sqlC.ReadData(fromSqlConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        DirectoryInfo di = Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, (string)row["WorldID"], (string)row["MapID"]));
                        mapNames.Add(di.FullName);
                    }
                    UpdateStatus("Stage 2 of 5; Creating and filling Skins directory");
                    Application.DoEvents();
                    dt = sqlC.ReadData(fromSqlConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        switch ((int)row["TypeID"])
                        {
                            case 0:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "8x8"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "8x8", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write(row.Field<byte[]>("Image"), 0, row.Field<byte[]>("Image").Length);
                                }
                                break;
                            case 1:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "16x16"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "16x16", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write(row.Field<byte[]>("Image"), 0, row.Field<byte[]>("Image").Length);
                                }
                                break;
                            case 2:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "32x32"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "32x32", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write(row.Field<byte[]>("Image"), 0, row.Field<byte[]>("Image").Length);
                                }
                                break;
                            case 3:
                                Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "body"));
                                using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "faces", "body", (string)row["PlayerName"]), FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write(row.Field<byte[]>("Image"), 0, row.Field<byte[]>("Image").Length);
                                }
                                break;
                        }
                    }
                    UpdateStatus("Stage 3 of 5; importing Marker File data ");
                    Application.DoEvents();
                    dt = sqlC.ReadData(fromSqlConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "_markers_"));
                        File.WriteAllText(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "_markers_", "marker_" + (string)row["FileName"]) + ".json", (string)row["Content"]);
                    }
                    UpdateStatus("Stage 4 of 5; importing Marker Icon data ");
                    Application.DoEvents();
                    dt = sqlC.ReadData(fromSqlConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), toPath, "_markers_", (string)row["IconName"]) + ".png", FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(row.Field<byte[]>("Image"), 0, row.Field<byte[]>("Image").Length);
                        }
                        //dbC.SendDataMarkerIcons(toMySqlConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    UpdateStatus("Stage 5 of 5; importing Tiles data ");
                    dt = sqlC.ReadData(fromSqlConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        UpdateProgress(dt.Rows.IndexOf(row) * 100 / dt.Rows.Count);
                        Application.DoEvents();
                        Console.WriteLine(row.Field<int>("MapID").ToString());
                        Console.WriteLine(mapNames[row.Field<int>("MapID")]);
                        Console.WriteLine();
                        int imageFormat = 0;
                        switch ((int)row["Format"])
                        {
                            case 1:
                                imageFormat = 0;
                                break;
                            case 2:
                                imageFormat = 1;
                                break;
                            case 3:
                                imageFormat = 1;
                                break;
                            case 4:
                                imageFormat = 1;
                                break;
                            case 5:
                                imageFormat = 1;
                                break;
                            case 6:
                                imageFormat = 1;
                                break;
                            case 7:
                                imageFormat = 1;
                                break;
                            case 8:
                                imageFormat = 1;
                                break;
                            case 9:
                                imageFormat = 2;
                                break;
                            case 10:
                                imageFormat = 2;
                                break;
                            case 11:
                                imageFormat = 2;
                                break;
                            case 12:
                                imageFormat = 2;
                                break;
                            case 13:
                                imageFormat = 2;
                                break;
                            case 14:
                                imageFormat = 2;
                                break;
                            case 15:
                                imageFormat = 2;
                                break;
                            case 16:
                                imageFormat = 2;
                                break;
                        }
                        string subFolderName = string.Join("_", (int)row["x"] >> 5, (int)row["y"] >> 5);
                        DirectoryInfo tilesDi = Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), mapNames[row.Field<int>("MapID")], subFolderName));
                        using (FileStream fs = new FileStream(string.Join(Path.DirectorySeparatorChar.ToString(), tilesDi.FullName, (int)row["x"] + "_" + (int)row["y"] + "." + (ImageFormat)imageFormat), FileMode.Create, FileAccess.Write))
                        {
                            if ((byte[])row["Image"] != null)
                            {
                                fs.Write((byte[])row["Image"], 0, (int)row["ImageLen"]);
                            }
                            else
                            {
                                fs.Write((byte[])row["NewImage"], 0, (int)row["ImageLen"]);
                            }
                        }
                    }
                    UpdateStatus("Finished conversion");
                    UpdateProgress(100);
                }
                if (from == "MSSQL" && to == "SQLite")
                {
                    sqlC.Password = fromMsPwd;
                    sqlC.UserName = fromMsUser;
                    sqlC.Server = fromMsAddr;
                    sqlC.DatabaseName = fromMsDb;
                    fromSqlConnection = sqlC.IsConnect(fromSqlConnection);
                    MessageBox.Show("sucessfully connected to: " + toSQLiteConnection.FileName);
                    SQLite.InitDatabase(toSQLiteConnection);
                    DataTable dt = sqlC.ReadData(fromSqlConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataFaces(toSQLiteConnection, (string)row["PlayerName"], (int)row["TypeID"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataMaps(toSQLiteConnection, (int)row["ID"], (string)row["WorldID"], (string)row["MapID"], (string)row["Variant"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataMarkerFiles(toSQLiteConnection, (string)row["FileName"], (string)row["Content"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataMarkerIcons(toSQLiteConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "SchemaVersion");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataSchemaVersion(toSQLiteConnection, (int)row["level"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLite.SendDataTiles(toSQLiteConnection, (int)row["MapID"], (int)row["x"], (int)row["y"], (int)row["zoom"], (int)row["ImageLen"], (byte[])row["Image"]);
                    }
                }
                if (from == "MSSQL" && to == "MySQL")
                {
                    sqlC.Password = fromMsPwd;
                    sqlC.UserName = fromMsUser;
                    sqlC.Server = fromMsAddr;
                    sqlC.DatabaseName = fromMsDb;
                    fromSqlConnection = sqlC.IsConnect(fromSqlConnection);
                    dbC.Password = toMsPwd;
                    dbC.UserName = toMsUser;
                    dbC.Server = toMsAddr;
                    dbC.DatabaseName = toMsDb;
                    toMySqlConnection = dbC.IsConnect(toMySqlConnection);
                    dbC.InitDatabase(toMySqlConnection);
                    MessageBox.Show("sucessfully connected to: " + fromSqlConnection.DataSource + " and: " + toMySqlConnection.DataSource);
                    DataTable dt = sqlC.ReadData(fromSqlConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataFaces(toMySqlConnection, (string)row["PlayerName"], (int)row["TypeID"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMaps(toMySqlConnection, (int)row["ID"], (string)row["WorldID"], (string)row["MapID"], (string)row["Variant"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMarkerFiles(toMySqlConnection, (string)row["FileName"], (string)row["Content"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataMarkerIcons(toMySqlConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "SchemaVersion");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataSchemaVersion(toMySqlConnection, (int)row["level"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        dbC.SendDataTiles(toMySqlConnection, (int)row["MapID"], (int)row["x"], (int)row["y"], (int)row["zoom"], (byte[])row["Image"]);
                    }
                }
                if (from == "MSSQL" && to == "MSSQL")
                {
                    sqlC.Password = fromMsPwd;
                    sqlC.UserName = fromMsUser;
                    sqlC.Server = fromMsAddr;
                    sqlC.DatabaseName = fromMsDb;
                    fromSqlConnection = sqlC.IsConnect(fromSqlConnection);
                    sqlC.Password = toMsPwd;
                    sqlC.UserName = toMsUser;
                    sqlC.Server = toMsAddr;
                    sqlC.DatabaseName = toMsDb;
                    toSqlConnection = sqlC.IsConnect(toSqlConnection);
                    sqlC.InitDatabase(toSqlConnection);
                    MessageBox.Show("sucessfully connected to: " + fromSqlConnection.DataSource + " and: " + toSqlConnection.DataSource);
                    DataTable dt = sqlC.ReadData(fromSqlConnection, "Faces");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataFaces(toSqlConnection, (string)row["PlayerName"], (int)row["TypeID"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "Maps");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataMaps(toSqlConnection, (int)row["ID"], (string)row["WorldID"], (string)row["MapID"], (string)row["Variant"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "MarkerFiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataMarkerFiles(toSqlConnection, (string)row["FileName"], (string)row["Content"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "MarkerIcons");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataMarkerIcons(toSqlConnection, (string)row["IconName"], (byte[])row["Image"], (int)row["ImageLen"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "SchemaVersion");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataSchemaVersion(toSqlConnection, (int)row["level"]);
                    }
                    dt = sqlC.ReadData(fromSqlConnection, "Tiles");
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlC.SendDataTiles(toSqlConnection, (int)row["MapID"], (int)row["x"], (int)row["y"], (int)row["zoom"], (byte[])row["Image"]);
                    }
                }
            }
        }
    }

    public class SqlDBConnection
    {
        private SqlDBConnection()
        {
        }
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        private static SqlDBConnection _instance = null;
        public string prefix { get; set; }
        private string tableTiles;
        private string tableMaps;
        private string tableFaces;
        private string tableMarkerIcons;
        private string tableMarkerFiles;
        private string tableStandaloneFiles;
        private string tableSchemaVersion;
        public static SqlDBConnection Instance()
        {
            if (_instance == null)
                _instance = new SqlDBConnection();
            return _instance;
        }
        public SqlConnection IsConnect(SqlConnection mysql_conn)
        {
            if (mysql_conn == null)
            {
                // Create a new database connection:
                string connString = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                mysql_conn = new SqlConnection(connString);
                try
                {
                    mysql_conn.Open();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Cannot connect to server: " + ex);
                }
                return mysql_conn;
            }
            else { return mysql_conn; }
        }
        public bool InitDatabase(SqlConnection mysql_conn)
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
            SqlCommand checkCMD = new SqlCommand(checkTableExistsQuery, mysql_conn);
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

                    SqlCommand cmd = new SqlCommand(query, mysql_conn);
                    var result = cmd.ExecuteScalar();
                }
            }

            return true;
        }
        public bool SendDataFaces(SqlConnection mysql_conn, string playerName, int typeID, byte[] image, int imageLen)
        {
            string insertMapQuery = "INSERT INTO " + tableFaces + "(PlayerName, TypeID, Image, ImageLen) VALUES('" + playerName + "','" + typeID + "',?Images'" + imageLen + "');";
            SqlCommand mySqlCommand = new SqlCommand(insertMapQuery, mysql_conn);
            SqlParameter parImage = new SqlParameter
            {
                ParameterName = "?Images",
                SqlDbType = SqlDbType.Binary,
                Size = 3000000,
                Value = image//here you should put your byte []
            };
            mySqlCommand.Parameters.Add(parImage);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataMaps(SqlConnection mysql_conn, int ID, string worldID, string mapName, string variant = "STANDARD", long serverID = 0)
        {
            string insertMapQuery = "INSERT INTO " + tableMaps + "(ID, WorldID, MapID, Variant, ServerID) VALUES('" + ID + "','" + worldID + "','" + mapName + "','" + variant + "','" + serverID + "');";
            SqlCommand mySqlCommand = new SqlCommand(insertMapQuery, mysql_conn);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }

        public bool SendDataMarkerFiles(SqlConnection mysql_conn, string fileName, string content)
        {
            string insertMapQuery = "INSERT INTO " + tableMarkerFiles + "(FileName, Content) VALUES('" + fileName + "','" + content + "');";
            SqlCommand mySqlCommand = new SqlCommand(insertMapQuery, mysql_conn);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataMarkerIcons(SqlConnection mysql_conn, string iconName, byte[] image, int imageLen)
        {
            string insertMapQuery = "INSERT INTO " + tableMarkerIcons + "(IconName, Image, ImageLen) VALUES('" + iconName + "',?Images'" + imageLen + "');";
            SqlCommand mySqlCommand = new SqlCommand(insertMapQuery, mysql_conn);
            SqlParameter parImage = new SqlParameter
            {
                ParameterName = "?Images",
                SqlDbType = SqlDbType.Int,
                Size = 3000000,
                Value = image//here you should put your byte []
            };
            mySqlCommand.Parameters.Add(parImage);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataSchemaVersion(SqlConnection mysql_conn, int level)
        {
            string insertMapQuery = "INSERT INTO " + tableSchemaVersion + "(level) VALUES('" + level + "');";
            SqlCommand mySqlCommand = new SqlCommand(insertMapQuery, mysql_conn);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataTiles(SqlConnection mysql_conn, int mapID, int x, int y, int zoom, byte[] NewImage)
        {
            string insertMapQuery = "INSERT INTO " + tableTiles + "(mapID, x, y, HashCode, LastUpdate, Format, zoom, NewImage) VALUES('" + mapID + "','" + x + "','" + y + "',0,0,1,'" + zoom + "',?Images);";
            SqlCommand mySqlCommand = new SqlCommand(insertMapQuery, mysql_conn);
            SqlParameter parImage = new SqlParameter
            {
                ParameterName = "?Images",
                SqlDbType = SqlDbType.Int,
                Size = 3000000,
                Value = NewImage//here you should put your byte []
            };
            mySqlCommand.Parameters.Add(parImage);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public DataTable ReadData(SqlConnection mysql_conn, string table)
        {
            SqlDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd;
                cmd = mysql_conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM " + table;  //set the passed query
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return dt;
        }
        public void Close(SqlConnection mysql_conn)
        {
            mysql_conn.Close();
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
        public MySqlConnection IsConnect(MySqlConnection mysql_conn)
        {
            if (mysql_conn == null)
            {
                // Create a new database connection:
                string connString = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                mysql_conn = new MySqlConnection(connString);
                try
                {
                    mysql_conn.Open();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Cannot connect to server: " + ex);
                }
                return mysql_conn;
            }
            else { return mysql_conn; }
        }
        public bool InitDatabase(MySqlConnection mysql_conn)
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
            MySqlCommand checkCMD = new MySqlCommand(checkTableExistsQuery, mysql_conn);
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

                    MySqlCommand cmd = new MySqlCommand(query, mysql_conn);
                    var result = cmd.ExecuteScalar();
                }
            }

            return true;
        }
        public bool SendDataFaces(MySqlConnection mysql_conn, string playerName, int typeID, byte[] image, int imageLen)
        {
            string insertMapQuery = "INSERT INTO " + tableFaces + "(PlayerName, TypeID, Image, ImageLen) VALUES('" + playerName + "','" + typeID + "',?Images'" + imageLen + "');";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, mysql_conn);
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
        public bool SendDataMaps(MySqlConnection mysql_conn, int ID, string worldID, string mapName, string variant = "STANDARD", long serverID = 0)
        {
            string insertMapQuery = "INSERT INTO " + tableMaps + "(ID, WorldID, MapID, Variant, ServerID) VALUES('" + ID + "','" + worldID + "','" + mapName + "','" + variant + "','" + serverID + "');";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, mysql_conn);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }

        public bool SendDataMarkerFiles(MySqlConnection mysql_conn, string fileName, string content)
        {
            string insertMapQuery = "INSERT INTO " + tableMarkerFiles + "(FileName, Content) VALUES('" + fileName + "','" + content + "');";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, mysql_conn);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataMarkerIcons(MySqlConnection mysql_conn, string iconName, byte[] image, int imageLen)
        {
            string insertMapQuery = "INSERT INTO " + tableMarkerIcons + "(IconName, Image, ImageLen) VALUES('" + iconName + "',?Images'" + imageLen + "');";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, mysql_conn);
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
        public bool SendDataSchemaVersion(MySqlConnection mysql_conn, int level)
        {
            string insertMapQuery = "INSERT INTO " + tableSchemaVersion + "(level) VALUES('" + level + "');";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, mysql_conn);
            mySqlCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataTiles(MySqlConnection mysql_conn, int mapID, int x, int y, int zoom, byte[] NewImage)
        {
            string insertMapQuery = "INSERT INTO " + tableTiles + "(mapID, x, y, HashCode, LastUpdate, Format, zoom, NewImage) VALUES('" + mapID + "','" + x + "','" + y + "',0,0,1,'" + zoom + "',?Images);";
            MySqlCommand mySqlCommand = new MySqlCommand(insertMapQuery, mysql_conn);
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
        public DataTable ReadData(MySqlConnection mysql_conn, string table)
        {
            MySqlDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                MySqlCommand cmd;
                cmd = mysql_conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM " + table;  //set the passed query
                ad = new MySqlDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return dt;
        }
        public void Close(MySqlConnection mysql_conn)
        {
            mysql_conn.Close();
        }
    }
    interface iStorage
    {

    }
    public class SQLiteDBConnection
    {
        private static SQLiteDBConnection _instance = null;
        public static SQLiteDBConnection Instance()
        {
            if (_instance == null)
                _instance = new SQLiteDBConnection();
            return _instance;
        }
        public SQLiteConnection IsConnect(string dbfile, SQLiteConnection sqlite_conn)
        {
            if (sqlite_conn == null)
            {
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
            else { return sqlite_conn; }
        }
        public bool InitDatabase(SQLiteConnection conn)
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
        public bool SendDataFaces(SQLiteConnection conn, string playerName, int typeID, byte[] image, int imageLen)
        {
            string insertMapQuery = "INSERT INTO Faces(PlayerName, TypeID, Image, ImageLen) VALUES('" + playerName + "','" + typeID + "',?Images'" + imageLen + "');";
            SQLiteCommand SQLiteCommand = new SQLiteCommand(insertMapQuery, conn);
            SQLiteParameter parImage = new SQLiteParameter
            {
                ParameterName = "?Images",
                DbType = DbType.Binary,
                Size = 3000000,
                Value = image
            };
            SQLiteCommand.Parameters.Add(parImage);
            SQLiteCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataMaps(SQLiteConnection conn, int ID, string worldID, string mapName, string variant = "STANDARD", long serverID = 0)
        {
            string insertMapQuery = "INSERT INTO Maps (ID, WorldID, MapID, Variant, ServerID) VALUES('" + ID + "','" + worldID + "','" + mapName + "','" + variant + "','" + serverID + "');";
            SQLiteCommand SQLiteCommand = new SQLiteCommand(insertMapQuery, conn);
            SQLiteCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataMarkerFiles(SQLiteConnection conn, string fileName, string content)
        {
            string insertMapQuery = "INSERT INTO MarkerFiles (FileName, Content) VALUES('" + fileName + "','" + content + "');";
            SQLiteCommand SQLiteCommand = new SQLiteCommand(insertMapQuery, conn);
            SQLiteCommand.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataMarkerIcons(SQLiteConnection conn, string iconName, byte[] image, int imageLen)
        {
            string insertMapQuery = "INSERT INTO MarkerIcons (IconName, Image, ImageLen) VALUES('" + iconName + "',?Images'" + imageLen + "');";
            SQLiteCommand sqlite_cmd = new SQLiteCommand(insertMapQuery, conn);
            SQLiteParameter parImage = new SQLiteParameter
            {
                ParameterName = "@Images",
                DbType = DbType.Binary,
                Size = 3000000,
                Value = image//here you should put your byte []
            };
            sqlite_cmd.Parameters.Add(parImage);
            sqlite_cmd.ExecuteNonQueryAsync();
            //Int64 result = (long)mySqlCommand.ExecuteScalar();
            return true;
        }
        public bool SendDataSchemaVersion(SQLiteConnection conn, int level)
        {
            string insertMapQuery = "INSERT INTO SchemaVersion (level) VALUES('" + level + "');";
            SQLiteCommand SQLiteCommand = new SQLiteCommand(insertMapQuery, conn);
            SQLiteCommand.ExecuteNonQueryAsync();
            return true;
        }
        public bool SendDataTiles(SQLiteConnection conn, int mapID, int x, int y, int zoom, int imageLength, byte[] NewImage)
        {
            string insertMapQuery = "INSERT INTO Tiles(mapID, x, y, zoom, HashCode, LastUpdate, Format, Image, ImageLen) VALUES('" + mapID + "','" + x + "','" + y + "','" + zoom + "',0,0,1,@Images," + imageLength + ")";
            SQLiteCommand sqlite_cmd = new SQLiteCommand(insertMapQuery, conn);
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
}