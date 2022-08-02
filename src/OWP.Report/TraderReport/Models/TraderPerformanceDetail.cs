namespace OWP.Report.TraderReport.Models
{
    public class TraderPerformanceDetail
    {
        public int TraderId { get; set; }

        public int BetType { get; set; }

        public decimal Turnover { get; set; }

        public decimal WinLoss { get; set; }
    }
}