namespace OWP.ProviderVerification.Criteria;

public class SaveEstAgentCriteria : NonQueryCommand
{
    public int OwpMatchId { get; set; }

    public int ActorId { get; set; }

    public string EstAgent { get; set; }

    public override string GetSettingKey() => "OWP_ProviderVerification_SaveEstAgent";

    public override bool IsValid() => true;
}


