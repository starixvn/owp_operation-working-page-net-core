namespace OWP.EventPicking.Criteria;

using Fanex.Data.Repository;

public class GetProviderCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "OWP_ProviderArrangement_GetAllProvider";

    public override bool IsValid() => true;
}