using Fanex.Data.Repository;

namespace OWP.AccountSetting.Profile.Criteria;

public class GetUserSettingByKeyCriteria : CriteriaBase
{
  public string Key { get; set; }
  public int UserId { get; set; }

  public override string GetSettingKey() => "OWP_UserSetting_GetByUserKey";

  public override bool IsValid() => !string.IsNullOrEmpty(Key);
}
