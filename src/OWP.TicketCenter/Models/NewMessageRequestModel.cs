namespace OWP.TicketCenter.Models
{
    public class NewMessageRequestModel
    {
        public int TicketId { get; set; }

        public int CreatorId { get; set; }

        public string Message { get; set; }

        public int? FileId { get; set; }
    }
}