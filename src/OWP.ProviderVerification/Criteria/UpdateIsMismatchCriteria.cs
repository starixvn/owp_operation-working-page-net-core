namespace OWP.ProviderVerification.Criteria;

public class UpdateIsMismatchCriteria : NonQueryCommand
{
    public DateTime EventDate { get; set; }

    public string OwpMatchIds { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_ProviderVerification_UpdateIsMismatch";

    public override bool IsValid() => true;
}