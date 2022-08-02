using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.OsLeagueSetting
{
    public class GetStatusOptionCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "OWP_OSLeagueSetting_GetStatusOption";

        public override bool IsValid() => true;
    }
}