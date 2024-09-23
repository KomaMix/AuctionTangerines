using AuctionTangerines.Data;
using AuctionTangerines.Enums;
using AuctionTangerines.Interfaces;
using AuctionTangerines.Models;
using AuctionTangerines.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuctionTangerines.Services
{
    public class TangerineClearService : BackgroundService
    {
        private readonly TimeSpan _runTime;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailSenderService _emailSenderService;
        private readonly EmailTemplateService _emailTemplateService;

        public TangerineClearService(
            IServiceProvider serviceProvider, 
            IOptions<TangerineClearServiceOptions> options,
            IEmailSenderService emailSenderService,
            EmailTemplateService emailTemplateService)
        {
            _serviceProvider = serviceProvider;
            _runTime = options.Value.RunTime;
            _emailSenderService = emailSenderService;
            _emailTemplateService = emailTemplateService;
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

                var tangerinesOnSale = await dbContext.Tangerines
                    .Where(t => t.Status == TangerineStatus.OnSale)
                    .Include(t => t.UserBuyout) // Включаем данные о владельце
                    .ToListAsync();

                // Проходим по каждому объекту
                foreach (var tangerine in tangerinesOnSale)
                {
                    if (tangerine.UserBuyout != null)
                    {
                        // Если есть владелец, меняем статус на Sold
                        tangerine.Status = TangerineStatus.Sold;

                        var emailTemplate = _emailTemplateService.GeneratePurchaseTemplate(tangerine);

                        await _emailSenderService.SendEmailAsync(
                            toEmail: tangerine.UserBuyout.Email,
                            subject: "Вы купили мандаринку",
                            body: emailTemplate
                        );
                    }
                    else
                    {
                        // Если владельца нет, меняем статус на Bad
                        tangerine.Status = TangerineStatus.Bad;
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }
        
    }
}
