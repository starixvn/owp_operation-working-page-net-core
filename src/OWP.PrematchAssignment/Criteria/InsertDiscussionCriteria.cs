using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class InsertDiscussionCriteria : NonQueryCommand
{
    public string DiscussionInfoXML { get; set; }

    public int ActorId { get; set; }

    public int AdminSiteMatchId { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchDiscussion_InsertDiscussion";

    public override bool IsValid() => ActorId > 0 && AdminSiteMatchId > 0 && !string.IsNullOrWhiteSpace(DiscussionInfoXML);
}