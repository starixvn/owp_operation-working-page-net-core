namespace OWP.AutoAssignment.Criteria.PublicApi;

using Fanex.Data.Repository;

public class SaveEventSenderCriteria : NonQueryCommand
{
    public string Xml { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_AutoAssignment_SaveEventHistory";
}