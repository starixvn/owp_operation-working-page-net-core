
namespace OWP.TodaySuggestionList.Models
{
    public record AssignmentModel
    {
        public int OWPMatchId { get; set; }

        public DateTime KOTime { get; set; }

        public DateTime EventDateTime { get; set; }

        public int BettypeId { get; set; }

        public string Remark { get; set; }

        public int LeagueName { get; set; }

        public int UserId { get; set; }

        public bool IsMajorLeague { get; set; }
    }
}