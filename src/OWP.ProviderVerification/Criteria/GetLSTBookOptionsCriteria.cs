namespace OWP.ProviderVerification.Criteria;

public class GetLSTBookOptionsCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_ProviderVerification_GetLSTBookOptions";

    public override bool IsValid() => true;
}
