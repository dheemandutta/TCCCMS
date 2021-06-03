using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

using System.IO;

namespace TCCCMS.Admin.ImportData
{
    static class Program
    {
        private static IScheduler scheduler;
        static void Main(string[] args)
        {
            //string s = "~/TicketFiles/202105211145_teamError.JPG";
            //string name = Path.GetFileName(s);
            //string path = Path.GetDirectoryName(s);
            string n = Path.GetFileNameWithoutExtension("tem.jpeg");

            ImportScheduler.Start();
            Console.ReadKey();
            scheduler.Shutdown();
        }
    }
}
