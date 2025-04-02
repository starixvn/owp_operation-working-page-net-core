namespace OWP.EventPicking.Api.Criteria;
public class UpdateEventDateTimeCriteria: NonQueryCommand
{
    public DateTime NewEventDate { get; set; }

    public int InfoSiteId { get; set; }

    public override string GetSettingKey() => "OWP_Infosite_Update_EventDateTime";

    public override bool IsValid() => true;
}

