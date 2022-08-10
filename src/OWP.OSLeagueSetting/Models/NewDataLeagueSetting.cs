namespace OWP.OSLeagueSetting.Models
{
    public class NewDataLeagueSetting
    {
        public string LeagueName { get; set; }
        public string IsActive { get; set; }
        public string Region { get; set; }
        public string Category { get; set; }
        public string Round { get; set; }
        public string WeekMatch { get; set; }
        public string OfferLines { get; set; }
        public string BetDelay { get; set; }
        public string Provider { get; set; }
        public string KOFromTime { get; set; }
        public string KOToTime { get; set; }
        public string SpreadHDP { get; set; }
        public string SpreadOU { get; set; }
        public string MaxBetHDP { get; set; }
        public string MaxBetOU { get; set; }
        public string Remark { get; set; }
        public string IsManualUpdateScore { get; set; }
        public string IsIpTvOnly { get; set; }
        public string BetType { get; set; }
    }
}