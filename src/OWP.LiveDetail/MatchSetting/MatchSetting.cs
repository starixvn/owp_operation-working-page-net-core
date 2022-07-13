namespace OWP.LiveDetail.MatchSetting
{
    public class MatchSetting
    {
        public int OWPMatchId { get; set; }

        public int Bettype { get; set; }

        public int MinorBettypeId { get; set; }

        public string DisplayName { get; set; }

        public float? Spread { get; set; }

        public float? Maxbet { get; set; }

        public bool IsFromMaxbetSetting { get; set; }
    }
}