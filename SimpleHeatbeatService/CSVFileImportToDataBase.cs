using Microsoft.Data.Sqlite;
using SimpleHeatbeatService.Models;
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
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        }
        /// <summary>
        /// 2 s cập nhật dữ liệu 1 lần
        /// kêt nối tới database
        /// thêm cột cho data table
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreated(object sender, FileSystemEventArgs e)
        {

            try
            {
                Thread.Sleep(2000);
                StreamReader reader = new StreamReader(e.FullPath);
                string paragraph = reader.ReadToEnd();

                SqliteConnection sqlite_conn = new SqliteConnection(ConfigurationManager.AppSettings["pathOfDB"]);
                sqlite_conn.Open();

                DataTable dt = new DataTable();
                dt.Columns.Add("Tagname", typeof(string));
                dt.Columns.Add("TimeStamp", typeof(string));
                dt.Columns.Add("Value", typeof(string));
                dt.Columns.Add("DataQuality", typeof(string));

                string[] lines = paragraph.Split('\n');

                for (int i = 1; i < lines.Length - 1; i++)
                {
                    string[] rows = lines[i].Split(',');
                    //string ins = "insert into SampleTable(Thoi_gian,Thiet_bi,Ten_du_lieu,Don_vi_do,Dia_chi,Trang_thai,Gia_tri) " +
                    //    "values('" + DateTime.Parse(words[0]) + "','" + words[1] + "','" + words[2] + "','" + words[3] + "','" + words[4] + "'," + int.Parse(words[5]) + "," + int.Parse(words[6]) + ")";
                    //var cmd = new SqliteCommand(sqlite_conn.ToString());
                    //cmd = sqlite_conn.CreateCommand();
                    //cmd.CommandText = ins;
                    //cmd.ExecuteNonQuery();



                    ThongSoDuLieuModel thongSoDuLieu = ConvertArrayToThongSoDuLieu(rows);

                    SqliteCommand cmd = new SqliteCommand();
                    cmd.CommandText = @"insert into SampleTable(Thoi_gian,Thiet_bi,Ten_du_lieu,Don_vi_do,Dia_chi,Trang_thai,Gia_tri)
                                        values(@Thoi_gian,@Thiet_bi,@Ten_du_lieu,@Don_vi_do,@Dia_chi,@Trang_thai,@Gia_tri)";
                    cmd.Connection = sqlite_conn;
                    cmd.Parameters.Add(new SqliteParameter("@Thoi_gian", thongSoDuLieu.Thoi_gian));
                    cmd.Parameters.Add(new SqliteParameter("@Thiet_bi", thongSoDuLieu.Thiet_bi));
                    cmd.Parameters.Add(new SqliteParameter("@Ten_du_lieu", thongSoDuLieu.Ten_du_lieu));
                    cmd.Parameters.Add(new SqliteParameter("@Don_vi_do", thongSoDuLieu.Don_vi_do));
                    cmd.Parameters.Add(new SqliteParameter("@Dia_chi",thongSoDuLieu.Dia_chi));
                    cmd.Parameters.Add(new SqliteParameter("@Trang_thai", thongSoDuLieu.Trang_thai));
                    cmd.Parameters.Add(new SqliteParameter("@Gia_tri", thongSoDuLieu.Gia_tri));

                    sqlite_conn.Open();
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine($"File:  create to {e.FullPath}");
                reader.Close();
                sqlite_conn.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine("source" + ex.Source);
                Console.WriteLine("message" + ex.Message);
                Console.WriteLine("Target" + ex.TargetSite);
            }
        }
        private ThongSoDuLieuModel ConvertArrayToThongSoDuLieu(string[] rows)
        {
            ThongSoDuLieuModel thongSoDuLieu = new ThongSoDuLieuModel();
            thongSoDuLieu.Thoi_gian = rows[0];
            thongSoDuLieu.Thiet_bi = rows[1];
            thongSoDuLieu.Ten_du_lieu = rows[2];
            thongSoDuLieu.Don_vi_do = rows[3];
            thongSoDuLieu.Dia_chi = rows[4];
            thongSoDuLieu.Trang_thai = rows[5];
            thongSoDuLieu.Gia_tri = rows[6];
            return thongSoDuLieu;
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
