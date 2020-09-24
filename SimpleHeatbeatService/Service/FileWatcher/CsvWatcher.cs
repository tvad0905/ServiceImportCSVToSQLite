using SimpleHeatbeatService.Service.DuLieu;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHeatbeatService.Service.FileWatcher
{
    public class CsvWatcher
    {
        /// <summary>
        /// giám sát sự thay đổi trong thư mục chứa file csv
        /// </summary>
        /// <param name="duongDanThuMucCSV">đường dẫn thư mục chứa file csv</param>
        public  CsvWatcher(string duongDanThuMucCSV)
        {
            try
            {
                FileSystemWatcher fileLogCSVwatcher;
                fileLogCSVwatcher = new FileSystemWatcher
                {
                    EnableRaisingEvents = false,
                    // We have to specify the path which has to monitor

                    Path = duongDanThuMucCSV,

                    // This property specifies which are the events to be monitored
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Filter = "*.*" // Only watch text files.
                };

                // Add event handlers for specific change events...

                //watcher.Changed += new FileSystemEventHandler(OnChanged);
                fileLogCSVwatcher.Created += new FileSystemEventHandler(OnCreated);
                fileLogCSVwatcher.Deleted += new FileSystemEventHandler(OnDelete);
                fileLogCSVwatcher.Renamed += new RenamedEventHandler(OnRenamed);
                // Begin watching
                fileLogCSVwatcher.EnableRaisingEvents = true;
            }catch(Exception ex)
            {
                Console.WriteLine("duong dan thu muc csv khong ton tai");
            }
           
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
                DuLieuToDatabase.Import(e.FullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("source" + ex.Source);
                Console.WriteLine("message" + ex.Message);
                Console.WriteLine("Target" + ex.TargetSite);
            }
        }

    }
}
