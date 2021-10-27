namespace OWP.TicketCenter.Models
{
    public class InsertOrRemoveSingleRecipientModel
    {
        public int TicketId { get; set; }

        public string TicketTitle { get; set; }

        public string ActorUsername { get; set; }

        public int RecipientId { get; set; }

        public int RecipientType { get; set; }
    }
}