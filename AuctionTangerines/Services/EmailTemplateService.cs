using AuctionTangerines.Models;

namespace AuctionTangerines.Services
{
    public class EmailTemplateService
    {
        public string GeneratePurchaseTemplate(Tangerine tangerine)
        {
            return $@"
            <html>
            <body>
                <h1>Поздравляем с покупкой!</h1>
                <p>Вы приобрели мандаринку <strong>#{tangerine.Id}</strong> за <strong>{tangerine.CostBuyout} руб.</strong>.</p>
                <p>Дата покупки: {tangerine.TimeBuyout?.ToShortDateString()}</p>
                <p>Благодарим за покупку!</p>
            </body>
            </html>";
        }

        public string GenerateBetOvertakenTemplate(Tangerine tangerine, decimal newBet)
        {
            return $@"
            <html>
            <body>
                <h1>Ваша ставка была перебита!</h1>
                <p>На мандаринку <strong>#{tangerine.Id}</strong> была сделана новая ставка в размере <strong>{newBet} руб.</strong>.</p>
                <p>Предыдущая ставка: {tangerine.CostBuyout} руб.</p>
                <p>Сделайте новую ставку, чтобы победить!</p>
            </body>
            </html>";
        }
    }
}
