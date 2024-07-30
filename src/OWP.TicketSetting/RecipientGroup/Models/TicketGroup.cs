namespace OWP.TicketSetting.RecipientGroup.Models;

public class TicketGroup
{
    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public List<Recipient> Recipients { get; set; }
}

public class RecipientGroupModel
{
    public int RecipientGroupId { get; set; }

    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; }
}