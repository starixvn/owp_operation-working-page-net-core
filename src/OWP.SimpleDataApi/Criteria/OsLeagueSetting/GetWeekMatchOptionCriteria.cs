namespace OWP.SimpleDataApi.Criteria.OsLeagueSetting;

using Fanex.Data.Repository;

public class GetWeekMatchOptionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetWeekMatchOption";

    public override bool IsValid() => true;
}