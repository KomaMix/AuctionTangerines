using AuctionTangerines.Data;
using AuctionTangerines.DTOs.Bet;
using AuctionTangerines.Interfaces;
using AuctionTangerines.Mappers;
using AuctionTangerines.Models;
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


        public HomeController(
			ILogger<HomeController> logger, 
			AppDbContext dbContext,
			UserManager<AppUser> userManager,
            IEmailSenderService emailSenderService)
		{
			_dbContext = dbContext;
			_logger = logger;
			_userManager = userManager;
			_emailSenderService = emailSenderService;
		}

		public async Task<IActionResult> Index()
		{
			// Получаем список мандаринок из базы данных
			var tangerines = await _dbContext.Tangerines
				.Where(t => t.Status == Enums.TangerineStatus.OnSale)
				.ToListAsync();

			// Передаем список в представление
			return View(tangerines);
		}

		[HttpPost]
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
                    _emailSenderService?.SendEmailAsync(
                    toEmail: tangerine.UserBuyout.Email,
                    subject: "Перекрытие ставки",
                    body: $"Вашу ставку перебили на {createBet.Cost - tangerine.CostBuyout}"
                );
                }
            }

            tangerine.UserBuyoutId = curUser.Id;
            // tangerine.UserBuyout = curUser; нужно ли
            tangerine.CostBuyout = createBet.Cost;

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
