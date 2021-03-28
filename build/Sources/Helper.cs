using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.SQLite;

namespace GitSame
{
    public class Helper
    {
        public static string AppDataPath { get; set; }
        public static string JsonPath { get; set; }
        public static string SettingsPath { get; set; }
        public static string DbPath { get; set; }
        static Helper()
        {
            RestoreAppData();

        }
        static public void CleanData()
        {
            if (System.IO.Directory.Exists(AppDataPath))
                System.IO.Directory.Delete(AppDataPath, true);
            RestoreAppData();
        }
        static public void RestoreAppData()
        {
            AppDataPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GitSame");
            JsonPath = Path.Combine(AppDataPath, "json");
            SettingsPath = Path.Combine(AppDataPath, "settings.json");
            DbPath = Path.Combine(AppDataPath, "appdb.db");

            if (!Directory.Exists(AppDataPath))
                Directory.CreateDirectory(AppDataPath);

            if (!Directory.Exists(JsonPath))
                Directory.CreateDirectory(JsonPath);

            if (!File.Exists(DbPath))
                CreateDb();

            //GitApi.Manager.setToken("token");
        }
        static private void CreateDb()
        {
            var connString = new SQLiteConnectionStringBuilder(){ DataSource = DbPath, ForeignKeys = true }.ConnectionString;
            var conn = new SQLiteConnection(connString);
            conn.Open();
            string commandText = @"CREATE TABLE 'Files' ( 
                                    Source    TEXT,
	                                Path  TEXT NOT NULL,
	                                Hash  TEXT,
	                                Metainfo  TEXT,
	                                IsChecked INTEGER NOT NULL,
	                                CreationDate  INTEGER NOT NULL,
	                                Id    INTEGER,
	                                PRIMARY KEY(Id)
                                 );";
            var command = new SQLiteCommand(commandText, conn);
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE Sources (
                                    Name  TEXT NOT NULL,
	                                Path  TEXT NOT NULL,
	                                IsChecked INTEGER NOT NULL,
	                                Branch    TEXT,
	                                Type  INTEGER NOT NULL,
	                                LastCommitHash    TEXT,
	                                PRIMARY KEY(Path)
                                 );";
            command.ExecuteNonQuery();
            conn.Close();

        }
        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

    }
        public class ListToTextConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s in (List<string>)value) sb.AppendLine(s);
                return sb.ToString();
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                string[] lines = ((string)value).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                return lines.ToList<String>();
            }
        }
}
