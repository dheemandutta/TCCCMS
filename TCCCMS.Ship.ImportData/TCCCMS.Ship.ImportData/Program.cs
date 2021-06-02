using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace TCCCMS.Ship.ImportData
{
    static class Program
    {
        private static IScheduler scheduler;
        static void Main(string[] args)
        {
            ImportScheduler.Start();
            Console.ReadKey();
            scheduler.Shutdown();
        }
    }
}
