using Fanex.Data.Repository;

namespace OWP.EventPicking.Criteria;

public class GetProviderCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "OWP_ProviderArrangement_GetAllProvider";

    public override bool IsValid() => true;
}