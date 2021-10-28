using OWP.TicketCenter.Middleware.Models;

namespace OWP.TicketCenter.Models
{
    public class InsertOrRemoveSingleRecipientModel: PermissionCheckedModel
    {
        public int TicketId { get; set; }
        public string TicketTitle { get; set; }
        public int TicketEventType { get; set; }
        public string ActorUsername { get; set; }
        public int ActorId { get; set; }
        public int RecipientId { get; set; }
        public int RecipientType { get; set; }
    }
}