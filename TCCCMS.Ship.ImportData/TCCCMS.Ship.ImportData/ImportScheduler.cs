using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Quartz;
using Quartz.Impl;

namespace TCCCMS.Ship.ImportData
{
    public class ImportScheduler
    {
        public static async void Start()
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            int intervalTimeInSecond = int.Parse(ConfigurationManager.AppSettings["SchedulerIntervalTime"].ToString());

            IJobDetail job = JobBuilder.Create<Import>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                    // s.WithIntervalInHours(24)
                    //s.WithIntervalInSeconds(120)
                    s.WithIntervalInSeconds(intervalTimeInSecond)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                  )
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
