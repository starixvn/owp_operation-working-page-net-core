namespace OWP.OtherSport.Submit.Models
{
    public interface TraderMatBex
    {
        public int OSMatchId { get; set; }

        public string BetTypeName { get; set; }
    }
    public class MatchAssignment : TraderMatBex
    {
        public int OSMatchId { get; set; }

        public string BetTypeName { get; set; }

        public int AssignmentType { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string AssignmentNote { get; set; }
    }
}
