using Fanex.Data.Repository;

namespace OWP.Criteria;

public class GetNotificationQueueCriteria : CriteriaBase
{
    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey()
    {
        return "OWP_GetNotificationQueue";
    }
}