namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class AddNewTicketIssueLeagueCriteria : NonQueryCommand
{
    public int TicketId { get; set; }

    public int OwpLeagueId { get; set; }

    public override string GetSettingKey() => "OWP_TicketIssueLeague_Create";

    public override bool IsValid()
        => TicketId > 0 && OwpLeagueId > 0;
}