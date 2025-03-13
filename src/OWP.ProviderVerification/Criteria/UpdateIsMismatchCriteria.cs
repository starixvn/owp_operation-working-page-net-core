namespace OWP.ProviderVerification.Criteria;

public class UpdateIsMismatchCriteria : NonQueryCommand
{
    public string OwpMatchIds { get; set; }

    public override string GetSettingKey() => "OWP_ProviderVerification_UpdateIsMismatch";

    public override bool IsValid() => true;
}