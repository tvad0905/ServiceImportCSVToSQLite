using ImportToDataBase.Models.DiemDo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportToDataBase.Models.ThietBi.Base
{
    public abstract class ThietBiModel
    {
        public string Name { get; set; }
        public string Protocol { get; set; }
        public Dictionary<string, DiemDoModel> dsDiemDoGiamSat = new Dictionary<string, DiemDoModel>();

    }
}
