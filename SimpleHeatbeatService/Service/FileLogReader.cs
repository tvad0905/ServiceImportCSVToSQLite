using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHeatbeatService.Service
{
    public static class FileLogReader
    {
        /// <summary>
        /// trả về 1 array các dòng đọc được từ CSV file
        /// </summary>
        /// <param name="duongDan">đường dẫn thư mục chứa file .csv</param>
        /// <returns>Array dòng đọc được</returns>
        public static string[] ReadAllLine(string duongDan)
        {
            StreamReader reader = new StreamReader(duongDan);
            string paragraph = reader.ReadToEnd();
            reader.Close();
            return paragraph.Split('\n');
        }
    }
}
