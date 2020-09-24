using SimpleHeatbeatService.Service.ThietBi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHeatbeatService.Service.FileWatcher
{
    public class JsonConfigDeviceWatcher
    {
        public JsonConfigDeviceWatcher(string duongDanThuMucJsonConfigDevice)
        {
            try
            {
                FileSystemWatcher fileLogCSVwatcher;
                fileLogCSVwatcher = new FileSystemWatcher
                {
                    Path = Path.GetDirectoryName(duongDanThuMucJsonConfigDevice), // We have to specify the path which has to monitor
                    EnableRaisingEvents = true,
                    // This property specifies which are the events to be monitored
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.FileName,
                    Filter = "*.json", // Only watch text files.

                };
                // Add event handlers for specific change events...

                //watcher.Changed += new FileSystemEventHandler(OnChanged);
                fileLogCSVwatcher.Created += new FileSystemEventHandler(OnCreated);
                fileLogCSVwatcher.Deleted += new FileSystemEventHandler(OnDelete);
                fileLogCSVwatcher.Renamed += new RenamedEventHandler(OnRenamed);
                fileLogCSVwatcher.Changed += new FileSystemEventHandler(OnChanged);
                // Begin watching
                fileLogCSVwatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("duong dan file json khong ton tai");
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                ThietBiToDatabase.Import(ConfigurationManager.AppSettings["pathOfDeviceAndDataJsonFile"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("source" + ex.Source);
                Console.WriteLine("message" + ex.Message);
                Console.WriteLine("Target" + ex.TargetSite);
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
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        }
    }
}
