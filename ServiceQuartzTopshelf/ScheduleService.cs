using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace ServiceQuartzTopshelf
{
    public class ScheduleService
    {
        private readonly IScheduler scheduler;

        public ScheduleService()
        {
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" },
                { "quartz.scheduler.instanceName", "MyScheduler" },
                { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
                { "quartz.threadPool.threadCount", "3" }
            };

            StdSchedulerFactory factory = new StdSchedulerFactory(props);


            //StdSchedulerFactory factory = new StdSchedulerFactory();
            scheduler = factory.GetScheduler().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void Start()
        {
            scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();

            ScheduleJobs();
        }

        public void ScheduleJobs()
        {

            string tiempoEjecucion = "0 0/1 * 1/1 * ? *";

            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("job1", "group1")
                .Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("trigger1", "group1")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInSeconds(10)
            //        .RepeatForever())
            //    .Build();

            ICronTrigger trigger2 = (ICronTrigger)TriggerBuilder.Create()
                                                      .WithIdentity("trigger2", "group1")
                                                      .WithCronSchedule(tiempoEjecucion)
                                                      .Build();


            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger2).ConfigureAwait(false).GetAwaiter().GetResult();

            scheduler.Start();
        }

        public void Stop()
        {
            scheduler.Shutdown().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
