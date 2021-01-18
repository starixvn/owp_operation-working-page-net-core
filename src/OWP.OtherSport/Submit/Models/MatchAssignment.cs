namespace OWP.OtherSport.Submit.Models
{
    public interface TraderMaxBet
    {
        public int OSMatchId { get; set; }

        public string BetTypeName { get; set; }
    }
    public class MatchAssignment : TraderMaxBet
    {
        public int OSMatchId { get; set; }

        public string BetTypeName { get; set; }

        public int AssignmentType { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public string AssignmentNote { get; set; }
    }
}
