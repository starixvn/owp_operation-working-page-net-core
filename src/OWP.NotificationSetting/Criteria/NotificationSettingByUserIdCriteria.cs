using Fanex.Data.Repository;

namespace OWP.NotificationSetting.Criteria;
public class NotificationSettingByUserIdCriteria : CriteriaBase
{
    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_NotificationSetting_GetByUserId";

    public override bool IsValid() => UserId > 0;
}