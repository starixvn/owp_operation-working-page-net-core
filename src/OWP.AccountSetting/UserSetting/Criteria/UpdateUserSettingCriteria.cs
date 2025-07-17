using Fanex.Data.Repository;

namespace OWP.AccountSetting.UserSetting.Criteria;

public class UpdateUserSettingCriteria : NonQueryCommand
{
    public string Key { get; set; }
    public string Value { get; set; }
    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_UserSetting_Update";

    public override bool IsValid() => !string.IsNullOrEmpty(Key) && !string.IsNullOrEmpty(Value);
}
