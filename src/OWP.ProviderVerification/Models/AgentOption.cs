namespace OWP.ProviderVerification.Models;

public class AgentOption
{
    public int AgentId { get; set; }

    public string AgentName { get; set; }
}

public class AgentConfirmOption
{
    public int Value { get; set; }

    public string Label { get; set; }
}

public class AgentConfirmModel
{
    public int OwpMatchId { get; set; }

    public int AgentId { get; set; }

    public string AgentName { get; set; }
}