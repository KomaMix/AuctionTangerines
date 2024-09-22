using AuctionTangerines.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionTangerines.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
		{
		}

		public DbSet<Bet> Bets { get; set; }

		public DbSet<Tangerine> Tangerines { get; set; }

	}
}
