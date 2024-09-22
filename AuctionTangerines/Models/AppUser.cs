using Microsoft.AspNetCore.Identity;

namespace AuctionTangerines.Models
{
	public class AppUser : IdentityUser
	{
        public List<Bet> Bets { get; set; } = new List<Bet>();
        public List<Tangerine> Tangerines { get; set; } = new List<Tangerine>();
    }
}
