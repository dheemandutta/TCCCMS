using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Quartz;
using Quartz.Impl;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace TCCCMS.Ship.ExportData
{
    public class ExportScheduler
    {
        public static async void Start()
        {
            //IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            await scheduler.Start();

            int intervalTimeInSecond = int.Parse(ConfigurationManager.AppSettings["SchedulerIntervalTime"].ToString());

            IJobDetail job = JobBuilder.Create<Export>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     //s.WithIntervalInHours(24)
                     s.WithIntervalInSeconds(intervalTimeInSecond)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                  )
                .Build();

            //scheduler.ScheduleJob(job, trigger);

            await scheduler.ScheduleJob(job, trigger);


        }
    }
}
