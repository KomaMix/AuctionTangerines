using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionTangerines.DTOs.Bet
{
    public class CreateBetDto
    {
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Cost { get; set; }

        [Required]
        public int TangerineId { get; set; }
    }
}
