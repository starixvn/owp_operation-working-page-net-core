namespace OWP.Event.LiveGameAssignment.Download.Criteria.EventAssignment;

using Fanex.Data.Repository;

public class GetNotifyMessagesCriteria : CriteriaBase
{
    public string SessionId { get; set; }

    public override bool IsValid() => !string.IsNullOrWhiteSpace(SessionId);

    public override string GetSettingKey() => "OWP_MessageGetBySessionId";
}