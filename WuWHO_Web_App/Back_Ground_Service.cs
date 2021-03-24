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
                        var FiveMinAgo = DateTime.Now.AddHours(-0.1);
                        var devices_in_last_5_minutes = cur_context.tbl_environment_4.Where(m => m.MAC_ID != "end of statement").Where(m => m.time_rec >= FiveMinAgo).Select(m => m.MAC_ID).Distinct().Count();

                        var total_detect = cur_context.tbl_environment_4.Where(m => m.MAC_ID != "end of statement").Count();

                        if (devices_in_last_5_minutes <= 1)
                        {
                            Twitter.Sendtweet("Pretty vacant with only " + devices_in_last_5_minutes + " people detected with Total Detections = " + total_detect.ToString());
                        }
                        else if(devices_in_last_5_minutes >= 10 || devices_in_last_5_minutes <= 15)
                        {
                            Twitter.Sendtweet("Getting fairly crowded with " + devices_in_last_5_minutes + " people detected with Total Detections = " + total_detect.ToString());
                        }
                        else if(devices_in_last_5_minutes >15 || devices_in_last_5_minutes <=19)
                        {
                            Twitter.Sendtweet("Almost full with " + devices_in_last_5_minutes + " people detected with Total Detections = " + total_detect.ToString());
                        }
                        else if(devices_in_last_5_minutes >=20) 
                        {
                            Twitter.Sendtweet("Maximum Capacity reached with " + devices_in_last_5_minutes + " people detected with Total Detections = " + total_detect.ToString());
                        }
                        else
                        {
                            Twitter.Sendtweet("error " + devices_in_last_5_minutes);
                        }
                       
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