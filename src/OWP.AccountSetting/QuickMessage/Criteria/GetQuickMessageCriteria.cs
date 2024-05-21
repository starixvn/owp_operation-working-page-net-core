using Fanex.Data.Repository;

namespace OWP.AccountSetting.Profile.Criteria;

public class GetQuickMessageCriteria : CriteriaBase
{
    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_AccountSetting_GetQuickMessage";

    public override bool IsValid() => true;
}
