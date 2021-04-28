namespace OWP.OtherSport.Notification.Models
{
    using Submit.Models;

    public class ChangedMatchSnapshot
    {
        public MatchInfo OldMatch { get; set; }

        public MatchInfo NewMatch { get; set; }

        public AssignmentSnapshot OldSnapshot { get; set; }

        public AssignmentSnapshot NewSnapshot { get; set; }
    }
}