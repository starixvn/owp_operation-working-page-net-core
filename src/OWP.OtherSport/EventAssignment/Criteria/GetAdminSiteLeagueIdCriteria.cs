using Fanex.Data.Repository;

namespace OWP.OtherSport.EventAssignment.Criteria;
public class GetAdminSiteLeagueIdCriteria : CriteriaBase
{
    public int AdminSiteMatchId { get; set; }

    public override string GetSettingKey() => "OWP_OtherSport_GetAdminSiteLeagueId";

    public override bool IsValid() => true;
}
