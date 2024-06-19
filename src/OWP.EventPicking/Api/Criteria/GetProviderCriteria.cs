using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;

public class GetProviderCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "OWP_ProviderArrangement_GetAllProvider";

    public override bool IsValid() => true;
}