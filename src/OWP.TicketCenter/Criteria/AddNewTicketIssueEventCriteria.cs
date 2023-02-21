using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class AddNewTicketIssueEventCriteria : NonQueryCommand
{
    public int TicketId { get; set; }

    public int InfoSiteMatchId { get; set; }

    public int OwpLeagueId { get; set; }

    public int OwpHomeId { get; set; }

    public int OwpAwayId { get; set; }

    public override string GetSettingKey() => "OWP_TicketIssueEvent_Create";

    public override bool IsValid()
        => TicketId > 0 &&
           InfoSiteMatchId > 0 &&
           IsValidEvent();

    public bool IsValidEvent() => OwpLeagueId > 0 &&
                                  OwpHomeId > 0 &&
                                  OwpAwayId > 0;
}