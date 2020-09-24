using Microsoft.Data.Sqlite;
using SimpleHeatbeatService.Models;
using SimpleHeatbeatService.Service.DuLieu;
using SimpleHeatbeatService.Service.FileWatcher;
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
    class ImportToDataBase
    {

       
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]

        private void TimerElapsed(/*object sender, ElapsedEventArgs e*/)
        {
            CsvWatcher fileCsvWatcher = new CsvWatcher(ConfigurationManager.AppSettings["pathOfCSVFolder"]);
            JsonConfigDeviceWatcher jsonConfigDeviceWatcher = new JsonConfigDeviceWatcher(ConfigurationManager.AppSettings["pathOfDeviceAndDataJsonFile"]);
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
