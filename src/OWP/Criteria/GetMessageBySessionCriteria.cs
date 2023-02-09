namespace OWP.Criteria;

using Fanex.Data.Repository;

public class GetMessageBySessionCriteria : CriteriaBase
{
    public string SessionId { get; set; }

    public override string GetSettingKey() => "OWP_MessageGetBySessionId";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(SessionId);
}