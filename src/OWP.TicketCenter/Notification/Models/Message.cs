namespace OWP.TicketCenter.Notification.Models
{
    public class Message
    {
        public int PreMatchIssueId { get; set; }

        public int TicketId { get; set; }

        public string Comment { get; set; }

        public string TicketType { get; set; }

        public string Username { get; set; }
    }
}
