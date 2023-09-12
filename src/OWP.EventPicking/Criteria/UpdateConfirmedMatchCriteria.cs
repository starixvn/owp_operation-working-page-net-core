using Fanex.Data.Repository;

namespace OWP.EventPicking.Criteria;

public class UpdateConfirmedMatchCriteria : NonQueryCommand
{
    public IList<string> InfoSiteMatchIds { get; set; }

    public string ModifiedBy { get; set; }

    public bool Confirmed { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_UpdateConfirmed";

    public override bool IsValid() => true;
}
