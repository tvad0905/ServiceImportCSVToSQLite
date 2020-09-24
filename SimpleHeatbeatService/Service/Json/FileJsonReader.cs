using ImportToDataBase.Models.ThietBi;
using ImportToDataBase.Models.ThietBi.Base;
using Newtonsoft.Json.Linq;
using SimpleHeatbeatService.Service.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHeatbeatService.Service.Json
{
    public static class FileJsonReader
    {
        /// <summary>
        ///lấy các thông số của thiết bị: điểm đo,category dữ liệu,các đặc tính của thiết bị
        /// </summary>
        /// <returns>dictionary thiết bị với đầy đủ thuộc tính</returns>
        public static Dictionary<string, ThietBiModel> GetThongSoThietBi(string duongDanFileJsonDeviceConfig)
        {

            Dictionary<string, ThietBiModel> dsThietBi = new Dictionary<string, ThietBiModel>();
            try
            {
                var duongDanFileJsonDeviceAndData = duongDanFileJsonDeviceConfig ;
                dsThietBi.Clear();
                JObject jsonObj = JObject.Parse(File.ReadAllText(duongDanFileJsonDeviceAndData));
                Dictionary<string, ThietBiTCPIP> deviceIP = jsonObj.ToObject<Dictionary<string, ThietBiTCPIP>>();
                foreach (var deviceIPUnit in deviceIP)
                {

                    dsThietBi.Add(deviceIPUnit.Key, deviceIPUnit.Value);
                }
                Dictionary<string, ThietBiCOM> deviceCom = jsonObj.ToObject<Dictionary<string, ThietBiCOM>>();
                foreach (var deviceComUnit in deviceCom)
                {
                    if (deviceComUnit.Value.Protocol == "Serial Port")
                    {

                        dsThietBi.Add(deviceComUnit.Key, deviceComUnit.Value);
                    }
                }
            }
            catch { }
            return dsThietBi;
        }
    }
}
