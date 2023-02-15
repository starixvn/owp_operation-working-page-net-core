namespace OWP.ProviderArrangement.Criteria;

using Fanex.Data.Repository;

public class GetLogProviderCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "OWP_ProviderArrangement_GetLogProvider";

    public override bool IsValid() => true;
}

