using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class AddNewTicketIssueLogWorkCriteria : NonQueryCommand
{
    public int TicketId { get; set; }

    public int? AdminSiteId { get; set; }

    public string Subject { get; set; }

    public string Status { get; set; }

    public override string GetSettingKey() => "OWP_TicketIssueLogWork_Create";
    
    public override bool IsValid() => TicketId > 0;
    
}