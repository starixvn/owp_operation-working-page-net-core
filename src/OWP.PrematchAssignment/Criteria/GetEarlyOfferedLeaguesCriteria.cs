using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;
public class GetEarlyOfferedLeaguesCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_GetEarlyOfferedLeagues";

    public override bool IsValid() => true;
}
