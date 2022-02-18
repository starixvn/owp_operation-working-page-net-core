namespace OWP.PreMatchAssignment.Models
{
    

    public class Log
    {
        public string LeagueName { get; set; }

        public string Action { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public DateTime LastModified { get; set; }

        public string LastModifiedInString { get; set; }

        public string ModifiedBy { get; set; }
    }
}