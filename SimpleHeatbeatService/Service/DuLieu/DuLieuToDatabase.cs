using Microsoft.Data.Sqlite;
using SimpleHeatbeatService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHeatbeatService.Service.DuLieu
{
    public static class DuLieuToDatabase
    {
        /// <summary>
        /// import dữ liệu vào database khi 1 file csv mới được sinh ra
        /// </summary>
        /// <param name="duongDanFileCSV">đường dẫn của file csv nhận vào để đọc dữ liệu</param>
        public static void Import(string duongDanFileCSV)
        {
            try
            {
                Thread.Sleep(2000);
                SqliteConnection sqlite_conn = new SqliteConnection(ConfigurationManager.AppSettings["pathOfDB"]);
                sqlite_conn.Open();
                string[] lines = FileLogReader.ReadAllLine(duongDanFileCSV);
                for (int i = 2; i < lines.Length - 1; i++)
                {
                    string[] rows = lines[i].Split(',');
                    ThongSoDuLieuModel thongSoDuLieu = ConvertArrayToThongSoDuLieu(rows, duongDanFileCSV);

                    SqliteCommand cmd = new SqliteCommand();
                    cmd.CommandText = @"insert into Data(TagName,DeviceName,Time,Value,Connected)
                                        values(@TagName,@DeviceName,@Time,@Value,@Connected)";
                    cmd.Connection = sqlite_conn;
                    cmd.Parameters.Add(new SqliteParameter("@TagName", thongSoDuLieu.Ten_du_lieu));
                    cmd.Parameters.Add(new SqliteParameter("@DeviceName", thongSoDuLieu.Thiet_bi));
                    cmd.Parameters.Add(new SqliteParameter("@Time", thongSoDuLieu.Thoi_gian));
                    cmd.Parameters.Add(new SqliteParameter("@Value", thongSoDuLieu.Gia_tri));
                    cmd.Parameters.Add(new SqliteParameter("@Connected", thongSoDuLieu.Trang_thai));

                    cmd.ExecuteNonQuery();
                }
                sqlite_conn.Close();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        ///Chuyển 1 dòng csv đọc được về Thông số dữ liệu model 
        /// </summary>
        /// <param name="rows">1 dòng nhận được từ file csv</param>
        /// <returns>1 đối tượng thông số dữ liệu hoàn chỉnh</returns>
        private static ThongSoDuLieuModel ConvertArrayToThongSoDuLieu(string[] rows,string duongDanFileCSV)
        {
            string tenFile = Path.GetFileName(duongDanFileCSV);
            int indexDauChamCuoiCung = rows[0].IndexOf('.');


            string tenDuLieu = rows[0].Substring(indexDauChamCuoiCung+1);
            string diemDo = rows[0].Substring(0,indexDauChamCuoiCung);
            string thoiGian = tenFile.Substring(tenFile.Length-23,19);
            string giaTri = rows[2];
            string trangThaiTiHieu = rows[3];
            

            ThongSoDuLieuModel thongSoDuLieu = new ThongSoDuLieuModel();
            thongSoDuLieu.Thiet_bi = diemDo;
            thongSoDuLieu.Thoi_gian = thoiGian;
            thongSoDuLieu.Ten_du_lieu = tenDuLieu;
            thongSoDuLieu.Don_vi_do = "de rong";
            thongSoDuLieu.Dia_chi = "de rong";
            thongSoDuLieu.Trang_thai = trangThaiTiHieu;
            thongSoDuLieu.Gia_tri = giaTri;
            return thongSoDuLieu;
        }
        
    }
}
