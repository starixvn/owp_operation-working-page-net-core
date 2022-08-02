using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.OsLeagueSetting
{
    public class GetRoundOptionCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "OWP_OSLeagueSetting_GetRoundOption";

        public override bool IsValid() => true;
    }
}