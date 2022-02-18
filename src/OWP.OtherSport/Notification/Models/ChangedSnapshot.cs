namespace OWP.OtherSport.Notification.Models
{
    
    using Submit.Models;

    public class ChangedSnapshot
    {
        public List<int> DeletedMatches { get; set; }

        public List<int> InsertedMatches { get; set; }

        public List<int> ChangedMatches { get; set; }

        public AssignmentSnapshot OldSnapshot { get; set; }

        public AssignmentSnapshot NewSnapshot { get; set; }
    }
}