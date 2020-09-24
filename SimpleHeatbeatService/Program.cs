using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace ImportToDataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<ImportToDataBase>(S =>
                {
                    S.ConstructUsing(importToDB => new ImportToDataBase());
                    S.WhenStarted(importToDB => importToDB.Start());
                    S.WhenStopped(importToDB => importToDB.Stop());
                });

                x.RunAsLocalSystem();
                x.SetServiceName("ImportToDataBaseService");
                x.SetDisplayName("Import To DataBase Service");
                x.SetDescription("this is simple Import To DataBase Service.");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
