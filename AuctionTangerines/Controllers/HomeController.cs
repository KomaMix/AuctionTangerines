using AuctionTangerines.Data;
using AuctionTangerines.DTOs.Bet;
using AuctionTangerines.DTOs.Tangerine;
using AuctionTangerines.Interfaces;
using AuctionTangerines.Mappers;
using AuctionTangerines.Models;
using AuctionTangerines.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuctionTangerines.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _dbContext;
		private readonly UserManager<AppUser> _userManager;
		private readonly IEmailSenderService _emailSenderService;
		private readonly EmailTemplateService _emailTemplateService;


        public HomeController(
			ILogger<HomeController> logger, 
			AppDbContext dbContext,
			UserManager<AppUser> userManager,
            IEmailSenderService emailSenderService,
			EmailTemplateService emailTemplateService)
		{
			_dbContext = dbContext;
			_logger = logger;
			_userManager = userManager;
			_emailSenderService = emailSenderService;
			_emailTemplateService = emailTemplateService;
		}

		public async Task<IActionResult> Index()
		{
            var tangerines = await _dbContext.Tangerines
        .Where(t => t.Status == Enums.TangerineStatus.OnSale)
        .Include(t => t.UserBuyout)
        .Select(t => new TangerineDto
        {
            Id = t.Id,
			Url = t.Url,
			CreatedOn = t.CreatedOn,
            Status = t.Status.ToString(),
			TimeBuyout = t.TimeBuyout,
            UserBuyoutName = t.UserBuyout.UserName,  // Только безопасные данные
            CostBuyout = t.CostBuyout
        })
        .ToListAsync();

            return View(tangerines);
        }

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateBet(CreateBetDto createBet)
		{
			var tangerine = await _dbContext.Tangerines
				.Where(t => t.Id == createBet.TangerineId)
				.Include(t => t.UserBuyout)
				.FirstOrDefaultAsync();

			if (tangerine == null)
			{
				return NotFound("Мандаринка не найдена");
            }

            var curUser = await _userManager.GetUserAsync(User);

			if (curUser == null)
			{
				throw new Exception("Пользователь без регистрации или не удалось найти пользователя");
			}

			if (tangerine.UserBuyout != null)
			{
                if (createBet.Cost <= tangerine.CostBuyout)
                {
                    ModelState.AddModelError("", $"Введите сумму, большую {tangerine.CostBuyout}");
                    return View();
                } else
				{
					var emailTemplate = _emailTemplateService.GenerateBetOvertakenTemplate(tangerine, createBet.Cost);

                    _emailSenderService?.SendEmailAsync(
                    toEmail: tangerine.UserBuyout.Email,
                    subject: "Перекрытие ставки",
                    body: emailTemplate
                );
                }
            }

            tangerine.UserBuyoutId = curUser.Id;
            // tangerine.UserBuyout = curUser; нужно ли
            tangerine.CostBuyout = createBet.Cost;

			tangerine.TimeBuyout = DateTime.Now;

			var bet = createBet.ToBetFromCreateBetDto(curUser.Id);

            _dbContext.Bets.Add(bet);

			await _dbContext.SaveChangesAsync();

			return RedirectToAction("Index");
		}

        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
