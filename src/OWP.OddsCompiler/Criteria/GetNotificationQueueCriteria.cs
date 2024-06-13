

namespace OWP.OddsCompiler.Criteria;
using Fanex.Data.Repository;

public class GetNotificationQueueCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_GetNotificationQueue";
}