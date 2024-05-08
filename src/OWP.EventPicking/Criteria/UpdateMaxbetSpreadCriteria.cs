using Fanex.Data.Repository;

namespace OWP.EventPicking.Criteria;
public class UpdateMaxbetSpreadCriteria : NonQueryCommand
{
    public string Xml { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_UpdateMaxbetSpread";

    public override bool IsValid() => true;
}

