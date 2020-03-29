using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Lpb.Gateway.Web.Startup
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    var host = new WebHostBuilder()
        //        .UseKestrel()
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .UseIISIntegration()
        //        .UseStartup<Startup>()
        //        .Build();

        //    host.Run();
        //}

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((logging) =>
                {
                    // 过滤掉 System 和 Microsoft 开头的命名空间下的组件产生的警告级别以下的日志
                    logging.AddFilter("System", LogLevel.Warning);
                    logging.AddFilter("Microsoft", LogLevel.Warning);
                    logging.AddFilter("Abp", LogLevel.Warning);
                    logging.AddFilter("Quartz", LogLevel.Warning);
                    logging.AddFilter("DotNetCore", LogLevel.Warning);
                    //logging.AddLog4Net();
                })
                .UseStartup<Startup>()
                .Build();
        }
    }
}
