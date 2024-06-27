

namespace OWP.BackgroundWorker.Criteria;

public class GetNotificationQueueCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_GetNotificationQueue";
}