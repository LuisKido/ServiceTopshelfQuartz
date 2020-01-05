using Serilog;
using System;
using System.IO;
using Topshelf;

namespace ServiceQuartzTopshelf
{
    class Program
    {
        public static void Main()
        {
            var rc = HostFactory.Run(x =>
            {
                x.Service<ScheduleService>(s =>
                {
                    s.ConstructUsing(name => new ScheduleService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.File($"{Path.GetTempPath()}logs\\myapp.txt", rollingInterval: RollingInterval.Day) //"%TEMP%/logs/myapp.txt"
                    .CreateLogger();
                x.UseSerilog(Log.Logger);

                x.SetDescription("Topshelf with Quartz and Serilog");
                x.SetDisplayName("Topshelf with Quartz");
                x.SetServiceName("Topshelf-Quartz");
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
