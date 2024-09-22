using AuctionTangerines.Data;
using AuctionTangerines.Enums;
using AuctionTangerines.Models;
using AuctionTangerines.Options;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;

namespace AuctionTangerines.Services
{
    public class TangerineGenerateService : BackgroundService
    {
        private readonly int _tangerineCount;
        private readonly TimeSpan _runTime;
        private readonly IServiceProvider _serviceProvider;

        public TangerineGenerateService(IServiceProvider serviceProvider, IOptions<TangerineServiceOptions> options)
        {
            _serviceProvider = serviceProvider;
            _tangerineCount = options.Value.TangerineCount;
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

                await TangerinesGenerate();
            }
        }

        private async Task TangerinesGenerate()
        {
            // Логика работы с мандаринами
            List<Tangerine> tangerines = new List<Tangerine>();

            for (int i = 0; i < _tangerineCount; i++)
            {
                tangerines.Add(new Tangerine
                {
                    Url = $"https://avatars.mds.yandex.net/i?id=b4fa01b573a6829ace045558f0d74cdefb58f5e0-10148308-images-thumbs&n=13"
                });
            }


            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await dbContext.Tangerines.AddRangeAsync(tangerines);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
