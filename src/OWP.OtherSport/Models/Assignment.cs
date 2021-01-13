using System.Collections.Generic;

namespace OWP.OtherSport.Models
{
    public class Assignment
    {
        public int No { get; set; }

        public int LeagueId { get; set; }

        public int PreMatchLeagueId { get; set; }

        public string LeagueName { get; set; }

        public string Status { get; set; }

        public string KOTime { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }

        public string Remark { get; set; }

        public IList<Trader> Traders { get; set; }

        public IList<Maxbet> MaxBets { get; set; }

        public string IpTv { get; set; }

        public string Streaming { get; set; }

        public string Provider { get; set; }

    }
}
