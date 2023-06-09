using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class GetMatchInfoCriteria : CriteriaBase
{
    public int AdminSiteId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetMatchInfoByAdminSiteId";

    public override bool IsValid() => AdminSiteId > 0;
}