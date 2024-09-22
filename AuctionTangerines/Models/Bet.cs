using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionTangerines.Models
{
    public class Bet
    {
        public int Id { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // Внешний ключ на Tangerine
        public int TangerineId { get; set; }
        public Tangerine Tangerine { get; set; }

        // Внешний ключ на AppUser

        [ForeignKey(nameof(Models.AppUser))]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
