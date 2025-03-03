namespace OWP.EventPicking.Api.Criteria;
public class GetEventDateByInfoSiteId: ICriteria
{
    public int InfoSiteId { get; set; }

    public DateTime? EventDate { get; set; }

    public string GetSettingKey()=> "OWP_EventPicking_GetEventDateByInfoSiteId";


    public bool IsValid() => true;
   
}
