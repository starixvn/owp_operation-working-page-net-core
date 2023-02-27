using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.OsLeagueSetting;

public class GetRegionOptionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetRegionOption";

    public override bool IsValid() => true;
}