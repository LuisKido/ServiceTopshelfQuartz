using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Serilog;

namespace ServiceQuartzTopshelf
{
    public class HelloJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var lastRun = context.NextFireTimeUtc.ToString();
            Log.Information("Greetings qlo from HelloJob!   Previous run: {lastRun}", lastRun);

            Console.WriteLine("Hola " + DateTime.Now.ToLongTimeString());
            
            return Task.CompletedTask;
        }
    }
}
