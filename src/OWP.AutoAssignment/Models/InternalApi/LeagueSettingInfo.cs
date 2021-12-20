namespace OWP.AutoAssignment.Models.InternalApi
{
    public class LeagueSettingInfo
    {
        public int MatchId { get; set; }

        public string MatchOffer { get; set; }

        public string LSTNote { get; set; }

        public double? MaxBetHDP { get; set; }

        public double? SpreadHDP { get; set; }

        public double? SpreadOU { get; set; }

        public double? MaxBetOU { get; set; }

        public double? MaxBet1HHDP { get; set; }

        public double? MaxBet1HOU { get; set; }

        public double? Spread1HHDP { get; set; }

        public double? Spread1HOU { get; set; }

        public string NoHDP { get; set; }

        public double? MaxBet1X2 { get; set; }

        public double? Spread1X2 { get; set; }

        public double? MaxBet1H1X2 { get; set; }

        public double? Spread1H1X2 { get; set; }
    }
}
