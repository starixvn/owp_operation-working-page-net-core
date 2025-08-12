namespace OWP.ProviderVerification.Criteria;

public class GetProviderVerificationCriteria : CriteriaBase
{
    public DateTime StartDate{ get; set; }

    public DateTime EndDate { get; set; }

    public bool OnlyShowIsMismatchEvent { get; set; }

    public string ConfirmProvider { get; set; }

    public string LSTBookProvider { get; set; }

    public override string GetSettingKey() => "OWP_ProviderVerification_Query";

    public override bool IsValid() => true;
}
