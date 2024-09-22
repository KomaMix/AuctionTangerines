using AuctionTangerines.Data;
using AuctionTangerines.Models;
using AuctionTangerines.Options;
using Microsoft.Extensions.Options;

namespace AuctionTangerines.Services
{
    public class TangerineClearService : BackgroundService
    {
        private readonly TimeSpan _runTime;
        private readonly IServiceProvider _serviceProvider;

        public TangerineClearService(IServiceProvider serviceProvider, IOptions<TangerineClearServiceOptions> options)
        {
            _serviceProvider = serviceProvider;
            _runTime = options.Value.RunTime;
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = now.Date.Add(_runTime);

                if (now.TimeOfDay >= _runTime)
                {
                    nextRun = nextRun.AddDays(1); // Следующий запуск завтра
                }

                var delay = nextRun - now;

                if (delay.TotalMilliseconds > 0)
                {
                    await Task.Delay(delay, stoppingToken);
                }

                await TangerinesClear();
            }
        }

        private async Task TangerinesClear()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await dbContext.SaveChangesAsync();
            }
        }
        
    }
}
