namespace OWP.TicketCenter.Models
{
    public class MessageModel
    {
        public int TicketId { get; set; }

        public int UserId { get; set; }

        public  string Message { get; set; }

        public bool IsSaved { get; set; }
    }
}
