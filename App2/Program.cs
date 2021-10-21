using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using Services;

namespace App2
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            // Code for adding migrations
            // await Host.CreateDefaultBuilder(args)
            //    .RunConsoleAsync();
            

            const string pathToLogFile = "C:/Users/epavlova/RiderProjects/App2/App2/nlog.config";
            var logger = NLogBuilder.ConfigureNLog(pathToLogFile).GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");

                // Init Startup
                var startup = new Startup();
                IServiceCollection services = new ServiceCollection();
                startup.ConfigureServices(services);

                // Init Services
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var responseService = serviceProvider.GetService<IResponseService>();

                // Listen messages
                logger.Info("start listening...");
                if (responseService != null)
                    await responseService.ListenAsync();

                Console.ReadLine();
                logger.Info("end listening.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    var startup = new Startup();
                    startup.ConfigureServices(services);
                });
    }
}