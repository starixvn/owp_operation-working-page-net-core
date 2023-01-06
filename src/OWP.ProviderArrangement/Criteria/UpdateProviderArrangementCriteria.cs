using Fanex.Data.Repository;

namespace OWP.ProviderArrangement.Criteria;

public class UpdateProviderArrangementCriteria : NonQueryCommand
{
    public string Providers { get; set; }

    public override string GetSettingKey()
        => "OWP_ProviderArrangement_UpdateOderProvider";

    public override bool IsValid() => true;
}

