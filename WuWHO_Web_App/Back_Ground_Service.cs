using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WuWHO_Web_App.Data;

namespace WuWHO_Web_App
{
    public class Back_Ground_Service : BackgroundService

    {



        private readonly IServiceProvider _context;

        public Back_Ground_Service(IServiceProvider context)
        {
            _context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await BackgroundProcessing(stoppingToken);

        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = _context.CreateScope())
                {
                    var cur_context = scope.ServiceProvider.GetRequiredService<WuWHO_Context>();

                    try
                    {
                        int total_detect = cur_context.tbl_environment_4.Where(m => m.MAC_ID != "end of statement").Count();
                        Twitter.Sendtweet("Total Detections: " + total_detect.ToString());
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}