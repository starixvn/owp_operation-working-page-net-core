using Fanex.Data.Repository;

namespace OWP.ProviderArrangement.Criteria;

public class SaveUserProfileCriteria : NonQueryCommand
{
    public int FileId { get; set; }

    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_AccountSetting_SaveUserProfile";

    public override bool IsValid() => true;
}

