namespace OWP.OtherSport.Submit.Models
{
    public class MatchAssignment
    {
        public int OSMatchId { get; set; }

        public int BetTypeName { get; set; }

        public bool isMainTrader { get; set; }

        public int UserId { get; set; }

        public string AssignmentNote { get; set; }
    }
}
