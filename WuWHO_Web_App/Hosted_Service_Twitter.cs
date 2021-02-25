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
    public class Hosted_Service_Twitter : IHostedService
    {
        
        private readonly IServiceProvider _context;

        public Hosted_Service_Twitter(IServiceProvider context)
        {
            _context = context;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            while (!cancellationToken.IsCancellationRequested)
            {
                using (IServiceScope scope = _context.CreateScope())
                {
                    var cur_context = scope.ServiceProvider.GetRequiredService<WuWHO_Context>();

                    try
                    {
                        int total_detect = cur_context.tbl_environment_4.Where(m => m.MAC_ID != "end of statement").Count();
                        Twitter.Sendtweet("Total Detections: " + total_detect.ToString());
                        await Task.Delay(TimeSpan.FromSeconds(300), cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                    }
                }
            }

            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;

        }
    }
}
