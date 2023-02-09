namespace OWP.SimpleDataApi.Criteria.OsLeagueSetting;

using Fanex.Data.Repository;

public class GetProviderOptionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetProviderOption";

    public override bool IsValid() => true;
}