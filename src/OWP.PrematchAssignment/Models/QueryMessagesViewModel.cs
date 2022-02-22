namespace OWP.PreMatchAssignment.Models
{
    public class QueryMessagesViewModel
    {
        public int PreMatchIssueId { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
}