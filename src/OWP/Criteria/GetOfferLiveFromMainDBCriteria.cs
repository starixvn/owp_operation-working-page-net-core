using Fanex.Data.Repository;

namespace OWP.Criteria;

public class GetOfferLiveFromMainDBCriteria : CriteriaBase
{
    public string AdminSiteIds { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_GetMatchStatusFromMainDB";

    public override bool IsValid() => true;
}