using Fanex.Data.Repository;

namespace OWP.AccountSetting.Profile.Criteria;

public class GetUserProfileCriteria : CriteriaBase
{
  public int UserId { get; set; }

  public override string GetSettingKey() => "OWP_AccountSetting_GetUserProfile";

  public override bool IsValid() => true;
}
