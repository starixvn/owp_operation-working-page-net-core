using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.OsLeagueSetting;

public class GetOfferLineOptionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetOfferLineOption";

    public override bool IsValid() => true;
}