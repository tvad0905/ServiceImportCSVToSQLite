using Microsoft.Data.Sqlite;
using SimpleHeatbeatService.Models;
using SimpleHeatbeatService.Service.DuLieu;
using SimpleHeatbeatService.Service.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHeatbeatService.Service.ThietBi
{
    public static class ThietBiToDatabase
    {
    /// <summary>
    ///import các device đọc được từ file json vào bảng device trong GS.db 
    /// </summary>
        public static void Import(string duongDanFileJsonDeviceConfig)
        {
            try
            {
                Thread.Sleep(2000);
                SqliteConnection sqlite_conn = new SqliteConnection(ConfigurationManager.AppSettings["pathOfDB"]);
                sqlite_conn.Open();
                foreach (var thietBi in FileJsonReader.GetThongSoThietBi(duongDanFileJsonDeviceConfig))
                {
                    SqliteCommand cmd = new SqliteCommand();
                    cmd.CommandText = @"insert into device(name,BranchOrProtocol,NameShow)
                                        values(@name,@BranchOrProtocol,@NameShow)";
                    cmd.Connection = sqlite_conn;
                    cmd.Parameters.Add(new SqliteParameter("@name", thietBi.Value.Name));
                    cmd.Parameters.Add(new SqliteParameter("@BranchOrProtocol", thietBi.Value.Protocol));
                    cmd.Parameters.Add(new SqliteParameter("@NameShow", thietBi.Value.Name));
                    cmd.ExecuteNonQuery();
                }
                sqlite_conn.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
