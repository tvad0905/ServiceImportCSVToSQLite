using ImportToDataBase.Models.DuLieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ImportToDataBase.Models.DiemDo
{
    public class DiemDoModel
    {
        string tenDiemDo;
        public Dictionary<string, DuLieuModel> DsDulieu = new Dictionary<string, DuLieuModel>();

        public DiemDoModel(string tenDiemDo, Dictionary<string, DuLieuModel> DsDulieu)
        {
            this.tenDiemDo = tenDiemDo;
            this.DsDulieu = DsDulieu;
        }
        public string TenDiemDo
        {
            get
            {
                return tenDiemDo;
            }
            set
            {
                tenDiemDo = value;
            }
        }


    }
}
