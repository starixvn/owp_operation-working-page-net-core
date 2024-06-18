using Fanex.Data.Repository;

namespace OWP.NotificationSetting.Criteria;
public class NotificationSettingUpdateCriteria : NonQueryCommand
{
    public int UserId { get; set; }

    public string Value { get; set; }

    public int NotificationSettingId { get; set; }

    public override string GetSettingKey() => "OWP_NotificationSetting_Update";

    public override bool IsValid() => UserId > 0 && NotificationSettingId > 0;
}