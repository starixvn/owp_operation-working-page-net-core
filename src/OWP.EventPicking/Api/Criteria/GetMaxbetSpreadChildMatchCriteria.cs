namespace OWP.EventPicking.Api.Criteria;
public class GetMaxbetSpreadChildMatchCriteria: ICriteria
{
    public string ChildMatchInfo { get; set; }
    public string GetSettingKey() => "OWP_EventPicking_SpecialProduct_GetMaxbetSpread";

    public bool IsValid() => true;

}
