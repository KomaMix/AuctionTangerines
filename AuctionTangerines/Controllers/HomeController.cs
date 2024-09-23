using AuctionTangerines.Data;
using AuctionTangerines.DTOs.Bet;
using AuctionTangerines.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AuctionTangerines.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
		{
			_dbContext = dbContext;
			_logger = logger;
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

        public async Task<IActionResult> CreateBet(CreateBetDto createBet)
		{
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
