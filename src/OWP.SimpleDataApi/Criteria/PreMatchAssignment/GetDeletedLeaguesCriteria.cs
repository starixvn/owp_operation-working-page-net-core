namespace OWP.SimpleDataApi.Criteria.PreMatchAssignment;

internal class GetDeletedLeaguesCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public bool IsDay { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_PreMatchAssignment_GetDeletedLeagues";
}