namespace OWP.ProviderArrangement.Criteria;

using Fanex.Data.Repository;

public class UpdateProviderArrangementCriteria : NonQueryCommand
{
    public string Providers { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey()
        => "OWP_ProviderArrangement_UpdateOderProvider";

    public override bool IsValid() => true;
}

