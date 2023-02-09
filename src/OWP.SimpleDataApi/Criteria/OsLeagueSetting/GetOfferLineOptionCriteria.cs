namespace OWP.SimpleDataApi.Criteria.OsLeagueSetting;

using Fanex.Data.Repository;

public class GetOfferLineOptionCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetOfferLineOption";

    public override bool IsValid() => true;
}