namespace OWP.ProviderVerification.Criteria;

public class SaveAgentConfirmCriteria : NonQueryCommand
{
    public int OwpMatchId { get; set; }

    public int ActorId { get; set; }

    public string AgentSelected { get; set; }

    public override string GetSettingKey() => "OWP_ProviderVerification_SaveAgentConfirm";

    public override bool IsValid() => true;
}
