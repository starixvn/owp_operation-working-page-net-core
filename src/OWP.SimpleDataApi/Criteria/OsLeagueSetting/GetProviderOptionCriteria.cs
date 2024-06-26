using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.OsLeagueSetting;

public class GetProviderOptionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetProviderOption";

    public override bool IsValid() => true;
}