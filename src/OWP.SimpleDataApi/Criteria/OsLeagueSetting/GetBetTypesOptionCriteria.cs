namespace OWP.SimpleDataApi.Criteria.OsLeagueSetting;

using Fanex.Data.Repository;

internal class GetBetTypesOptionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetBetTypesOption";

    public override bool IsValid() => true;
}
