namespace OWP.ProviderVerification.Criteria;

public class SaveRemarkByCriteria : NonQueryCommand
{
    public int OwpMatchId { get; set; }

    public string RemarkBy { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_ProviderVerification_SaveRemarkBy";

    public override bool IsValid() => true;
}