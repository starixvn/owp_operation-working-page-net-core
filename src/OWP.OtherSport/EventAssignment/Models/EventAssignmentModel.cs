namespace OWP.OtherSport.EventAssignment.Models
{
    using System;
    using System.Collections.Generic;

    public class EventAssignmentModel
    {
        public double MatchNo { get; set; }

        public int OSMatchId { get; set; }

        public string MatchStatus { get; set; }

        public string Sport { get; set; }

        public DateTime DateTime { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public string League { get; set; }

        public string Category { get; set; }

        public string NoHDP { get; set; }

        public string MatchOffer { get; set; }

        public string AutoLink { get; set; }

        public string Provider { get; set; }

        public string OtherProvider { get; set; }

        public string Service { get; set; }

        public string IPTV { get; set; }

        public string WB { get; set; }

        public string Remark { get; set; }

        public IList<TraderMaxBetModel> TraderMaxbetModels { get; set; } = new List<TraderMaxBetModel>();
    }
}
