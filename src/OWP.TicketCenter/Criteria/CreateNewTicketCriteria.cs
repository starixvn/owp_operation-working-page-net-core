using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class CreateNewTicketCriteria : CriteriaBase
{
    public int TicketType { get; set; }

    public int? SubType { get; set; }

    public int? AdminSiteId { get; set; }

    public string Subject { get; set; }

    public string Status { get; set; }

    public int? OwpMatchId { get; set; }

    public int? OwpLeagueId { get; set; }

    public int? OwpHomeId { get; set; }

    public int? OwpAwayId { get; set; }

    public int? InfoSiteMatchId { get; set; }

    public int? EventType { get; set; }

    public override string GetSettingKey() => "OWP_TicketIssue_Create";

    public override bool IsValid()
        => true;
}