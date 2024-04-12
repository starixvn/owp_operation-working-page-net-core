namespace OWP.ProviderVerification.Criteria;

public class GetAgentOptionsCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_ProviderVerification_GetAgentOptions";

    public override bool IsValid() => true;
}
