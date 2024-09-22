using AuctionTangerines.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AuctionTangerines.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Tangerine>()
                .Property(t => t.Status)
                .HasConversion<string>();  // Это преобразует enum в строку в базе данных
        }

        public DbSet<Bet> Bets { get; set; }

		public DbSet<Tangerine> Tangerines { get; set; }

	}
}
