﻿using AuctionTangerines.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionTangerines.Models
{
    public class Tangerine
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public TangerineStatus Status { get; set; } = TangerineStatus.OnSale;
        public DateTime CreatedOn { get; set; } = DateTime.Now;


        [Column(TypeName = "decimal(18,2)")]
        public decimal CostBuyout { get; set; }
        public DateTime? TimeBuyout { get; set; }

        public string? UserBuyoutId { get; set; }

        // Навигационное свойство для связи с AppUser
        [ForeignKey(nameof(UserBuyoutId))]
        public AppUser? UserBuyout { get; set; }
        public List<Bet> Bets { get; set; } = new List<Bet>();
    }
}
