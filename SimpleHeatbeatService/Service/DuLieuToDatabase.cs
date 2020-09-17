using Microsoft.Data.Sqlite;
using SimpleHeatbeatService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHeatbeatService.Service
{
    public static class DuLieuToDatabase
    {
        public static void Import(string duongDanThuMucFileCSV)
        {
            try
            {
                Thread.Sleep(2000);
                SqliteConnection sqlite_conn = new SqliteConnection(ConfigurationManager.AppSettings["pathOfDB"]);
                sqlite_conn.Open();
                string[] lines = FileLogReader.ReadAllLine(duongDanThuMucFileCSV);
                for (int i = 1; i < lines.Length - 1; i++)
                {
                    string[] rows = lines[i].Split(',');
                    ThongSoDuLieuModel thongSoDuLieu = ConvertArrayToThongSoDuLieu(rows);

                    SqliteCommand cmd = new SqliteCommand();
                    cmd.CommandText = @"insert into SampleTable(Thoi_gian,Thiet_bi,Ten_du_lieu,Don_vi_do,Dia_chi,Trang_thai,Gia_tri)
                                        values(@Thoi_gian,@Thiet_bi,@Ten_du_lieu,@Don_vi_do,@Dia_chi,@Trang_thai,@Gia_tri)";
                    cmd.Connection = sqlite_conn;
                    cmd.Parameters.Add(new SqliteParameter("@Thoi_gian", thongSoDuLieu.Thoi_gian));
                    cmd.Parameters.Add(new SqliteParameter("@Thiet_bi", thongSoDuLieu.Thiet_bi));
                    cmd.Parameters.Add(new SqliteParameter("@Ten_du_lieu", thongSoDuLieu.Ten_du_lieu));
                    cmd.Parameters.Add(new SqliteParameter("@Don_vi_do", thongSoDuLieu.Don_vi_do));
                    cmd.Parameters.Add(new SqliteParameter("@Dia_chi", thongSoDuLieu.Dia_chi));
                    cmd.Parameters.Add(new SqliteParameter("@Trang_thai", thongSoDuLieu.Trang_thai));
                    cmd.Parameters.Add(new SqliteParameter("@Gia_tri", thongSoDuLieu.Gia_tri));

                    cmd.ExecuteNonQuery();
                }
                sqlite_conn.Close();
            }
            catch
            {
                throw;
            }


            
        }
        /// <summary>
        ///Chuyển 1 dòng csv đọc được về Thông số dữ liệu model 
        /// </summary>
        /// <param name="rows">1 dòng nhận được từ file csv</param>
        /// <returns>1 đối tượng thông số dữ liệu hoàn chỉnh</returns>
        private static ThongSoDuLieuModel ConvertArrayToThongSoDuLieu(string[] rows)
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
    }
}
