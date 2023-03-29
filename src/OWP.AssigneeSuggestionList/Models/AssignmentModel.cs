using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Models
{
    public record AssignmentModel
    {
        public int OWPMatchId { get; set; }

        public TimeSpan KOTime { get; set; }

        public DateTime EventDateTime { get; set; }

        public int BettypeId { get; set; }

        public string Remark { get; set; }

        public string LeagueName { get; set; }

        public int UserId { get; set; }

        public bool IsMajorLeague { get; set; }

        public bool IsMainMatch { get; set; }

        public bool IsFantasyMatch { get; set; }

        public bool IsCornerMatch { get; set; }

        public bool Is15Mins { get; set; }

        public int? ParentMatchId { get; set; }

        public string ColorCode { get; set; }

        public MatchType MatchType { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }
    }
}