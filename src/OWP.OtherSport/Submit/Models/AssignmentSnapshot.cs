namespace OWP.OtherSport.Submit.Models
{
    using System;
    using System.Collections.Generic;

    public class AssignmentSnapshot
    {
        public Dictionary<int, MatchInfo> Matches { get; set; }

        public Dictionary<int, List<MatchMaxBet>> MaxBet { get; set; }

        public Dictionary<int, List<MatchAssignment>> MatchAssignments { get; set; }

        public AssignmentSnapshot()
        {
            Matches = new Dictionary<int, MatchInfo>();
            MaxBet = new Dictionary<int, List<MatchMaxBet>>();
            MatchAssignments = new Dictionary<int, List<MatchAssignment>>();
        }
    }

    public class MatchInfo
    {
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
    }
}
