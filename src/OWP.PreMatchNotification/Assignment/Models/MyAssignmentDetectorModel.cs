namespace OWP.PreMatchNotification.Assignment.Models
{
    public class MyAssignmentDetectorModel
    {
        public string LeagueName { get; set; }

        public string oldAssignment { get; set; }

        public string newAssignment { get; set; }

        public string BettypeName { get; set; }

        public int UserIdRemoved { get; set; }

        public int UserIdNewAssign { get; set; }

        public bool? IsDay { get; set; }
    }
}
