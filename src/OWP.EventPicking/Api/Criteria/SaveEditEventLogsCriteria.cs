using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;
public class SaveEditEventLogsCriteria : NonQueryCommand
{
    public string Xml { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_SaveEditLogs";

    public override bool IsValid() => true;
}

