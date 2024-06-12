namespace OWP.DailyWorkingShift.Models
{
    using System;

    public class QueryMatchInfo
    {
        public int OWPMatchId { get; set; }

        public string MatchNo { get; set; }

        public string OtherProvider { get; set; }

        public string Provider { get; set; }

        public DateTime EventDateTime { get; set; }

        public int CategoryId { get; set; }

        public string LeagueName { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }

        public float MaxBet { get; set; }

        public bool IsAllowedToViewDetail { get; set; }
    }
}