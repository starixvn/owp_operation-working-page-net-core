namespace OWP.EventPicking.Api.Criteria;

public class InsertOrUpdateEventChangeKoCriteria : INonQueryCommand
{
    public int InfoSiteId { get; set; }

    public DateTime NewEventDate { get; set; }

    public string GetSettingKey() => "OWP_InfoSite_InsertOrUpdateEventChangeKo";

    public bool IsValid() => true;
}
