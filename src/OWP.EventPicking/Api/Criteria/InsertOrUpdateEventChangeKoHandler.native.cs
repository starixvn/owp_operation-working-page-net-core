namespace OWP.EventPicking.Api.Criteria;

public class InsertOrUpdateEventChangeKoCriteria : INonQueryCommand
{
    public int InfoSiteId { get; set; }

    public DateTime NewKo { get; set; }

    public string GetSettingKey() => "OWP_EventPicking_InsertOrUpdateEventChangeKo";

    public bool IsValid() => true;
}
