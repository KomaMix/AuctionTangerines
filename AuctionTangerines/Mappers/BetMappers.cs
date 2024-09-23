using AuctionTangerines.DTOs.Bet;
using AuctionTangerines.Models;

namespace AuctionTangerines.Mappers
{
    public static class BetMappers
    {
        public static Bet ToBetFromCreateBetDto(this CreateBetDto commentDto, string appUserId)
        {
            return new Bet
            {
                Cost = commentDto.Cost,
                TangerineId = commentDto.TangerineId,
                AppUserId = appUserId
            };
        }
    }
}
