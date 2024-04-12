namespace OWP.ProviderVerification.Criteria;

public class GetProviderVerificationCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey()
        => "OWP_ProviderVerification_Query";

    public override bool IsValid() => true;
}
