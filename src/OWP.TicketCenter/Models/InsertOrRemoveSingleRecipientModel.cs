namespace OWP.TicketCenter.Models;

using Middleware.Models;

public class InsertOrRemoveSingleRecipientModel : PermissionCheckedModel
{
    public string TicketTitle { get; set; }

    public int TicketEventType { get; set; }

    public int RecipientId { get; set; }

    public int RecipientType { get; set; }
}