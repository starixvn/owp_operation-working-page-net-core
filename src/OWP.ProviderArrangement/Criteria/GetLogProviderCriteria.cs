using Fanex.Data.Repository;

namespace OWP.ProviderArrangement.Criteria;

public class GetLogProviderCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "OWP_ProviderArrangement_GetLogProvider";

    public override bool IsValid() => true;
}

