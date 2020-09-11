using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ImportToDataBase
{
    class CSVFileImportToDataBase
    {

        static FileSystemWatcher watcher;
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]

        private void TimerElapsed(/*object sender, ElapsedEventArgs e*/)
        {

            watcher = new FileSystemWatcher
            {
                EnableRaisingEvents = false,
                // We have to specify the path which has to monitor

                Path = ConfigurationManager.AppSettings["pathOfCSVFolder"],

                // This property specifies which are the events to be monitored
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*" // Only watch text files.
            };

            // Add event handlers for specific change events...

            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.Deleted += new FileSystemEventHandler(OnDelete);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            // Begin watching
            watcher.EnableRaisingEvents = true;

        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
        }

        private void OnDelete(object sender, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            
            try
            {
                Thread.Sleep(2000);
                StreamReader reader = new StreamReader(e.FullPath);
                string paragraph = reader.ReadToEnd();
                DataTable dt = new DataTable();
                SqliteConnection sqlite_conn = new SqliteConnection(ConfigurationManager.AppSettings["pathOfDB"]);
                sqlite_conn.Open();
                dt.Columns.Add("Thoi_gian", typeof(DateTime));
                dt.Columns.Add("Thiet_bi", typeof(string));
                dt.Columns.Add("Ten_du_lieu", typeof(string));
                dt.Columns.Add("Don_vi_do", typeof(string));
                dt.Columns.Add("Dia_chi", typeof(string));
                dt.Columns.Add("Trang_thai", typeof(int));
                dt.Columns.Add("Gia_tri", typeof(int));
                string[] lines = paragraph.Split('\n');

            
                //connect.Open();
                for (int i = 1; i < lines.Length-1; i++)
                {
                    string[] words = lines[i].Split(',');
                    string ins = "insert into SampleTable(Thoi_gian,Thiet_bi,Ten_du_lieu,Don_vi_do,Dia_chi,Trang_thai,Gia_tri) " +
                        "values('" + DateTime.Parse(words[0]) + "','" + words[1] + "','" + words[2] + "','" + words[3] + "','" + words[4] + "'," + int.Parse(words[5]) + "," + int.Parse(words[6]) + ")";
                    var cmd = new SqliteCommand(sqlite_conn.ToString());
                    cmd = sqlite_conn.CreateCommand();
                    cmd.CommandText = ins;
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine($"File:  create to {e.FullPath}");
                reader.Close();
                sqlite_conn.Close();
            }
           
            catch (Exception ex)
            {
                Console.WriteLine("source"+ex.Source);
                Console.WriteLine("message"+ex.Message);
                Console.WriteLine("Target"+ex.TargetSite);
            }
        }

        public void Start()
        {
            TimerElapsed();
          
        }
        public void Stop()
        {
           
        }
    }
}
