using Fanex.Data.Repository;

namespace OWP.Criteria;

public class GetMessageBySessionCriteria : CriteriaBase
{
    public string SessionId { get; set; }

    public override string GetSettingKey()
    {
        return "OWP_MessageGetBySessionId";
    }

    public override bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(SessionId);
    }
}