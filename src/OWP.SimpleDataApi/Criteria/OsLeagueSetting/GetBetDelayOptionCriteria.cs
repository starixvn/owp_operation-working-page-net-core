namespace OWP.SimpleDataApi.Criteria.OsLeagueSetting;

using Fanex.Data.Repository;

public class GetBetDelayOptionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetBetDelayOption";

    public override bool IsValid() => true;
}