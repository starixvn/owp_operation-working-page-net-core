using Fanex.Data.Repository;

namespace OWP.Event.TaskList.Criteria;

public class CreateTicketForRCDiscussionCriteria : CriteriaBase
{
    public int TicketType { get; set; }

    public int? OwpMatchId { get; set; }

    public bool IsRC { get; set; }

    public string DiscussionName { get; set; }

    public string ChannelRC { get; set; }

    public override string GetSettingKey() => "OWP_TicketIssue_Create";

    public override bool IsValid()
        => true;
}
