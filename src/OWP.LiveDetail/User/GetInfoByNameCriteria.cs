using Fanex.Data.Repository;

namespace OWP.LiveDetail.User;

public class GetInfoByNameCriteria : CriteriaBase
{
    public string Name { get; set; }

    public override bool IsValid() => !string.IsNullOrWhiteSpace(Name);

    public override string GetSettingKey() => "OWP_UserGetByName";
}