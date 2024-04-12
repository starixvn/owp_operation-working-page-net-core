using Fanex.Data.Repository;

namespace OWP.ProviderVerification.Criteria;

public class SaveEstNoteCriteria : NonQueryCommand
{
    public int OwpMatchId { get; set; }

    public string EstNote { get; set; }

    public override string GetSettingKey() => "OWP_ProviderVerification_SaveEstNote";

    public override bool IsValid() => true;
}


