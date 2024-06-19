namespace OWP.SimpleDataApi.Criteria.PreMatchAssignment;

public class GetStartTimeCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_GetStartTimeForNightShift";

    public override bool IsValid() => true;
}