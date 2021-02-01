using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            HostBuilder builder = new HostBuilder();
            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddAzureStorage();
                b.AddWarmup();
            });
            builder.ConfigureAppConfiguration(cb =>
            {
                cb.AddJsonFile("appsettings.json");
            });
            IHost host = builder.Build();
            using (host)
            {
                host.Start();
                IJobHost jobHost = (IJobHost)host.Services.GetService(typeof(IJobHost));
                jobHost.CallAsync(nameof(Functions.MyContiniousMethod)).Wait();
            }
        }
    }

    public class Functions
    {
        [NoAutomaticTrigger]
        public void MyContiniousMethod()
        {
            try
            {
                while (true != false)
                {
                    Thread.Sleep(1000);
                    Random rand = new Random();
                    if (rand.NextDouble() > 0.75)
                        throw new Exception("Random Kill to end job");
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // If this ends the whole thing ends
            }
        }
    }
}
