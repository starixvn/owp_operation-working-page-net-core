
namespace OWP.PreMatchAssignment.Models
{
    using System.Collections.Generic;

    public class QueryMessagesViewModel
    {
        public int PreMatchIssueId { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
}