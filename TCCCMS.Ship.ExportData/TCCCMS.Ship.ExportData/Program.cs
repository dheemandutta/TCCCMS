using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Ship.ExportData
{
    static class Program 
    {

        private static IScheduler scheduler;
        static void Main(string[] args)
        {
            ExportScheduler.Start();
            Console.ReadKey();
            scheduler.Shutdown();
        }
    }
}
