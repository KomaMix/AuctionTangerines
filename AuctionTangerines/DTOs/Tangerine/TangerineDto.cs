namespace AuctionTangerines.DTOs.Tangerine
{
    public class TangerineDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? TimeBuyout { get; set; }
        public string? UserBuyoutName { get; set; }
        public decimal? CostBuyout { get; set; }
    }
}
