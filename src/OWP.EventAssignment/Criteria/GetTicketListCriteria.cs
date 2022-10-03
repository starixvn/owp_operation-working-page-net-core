using Fanex.Data.Repository;

namespace OWP.EventAssignment.Criteria;

public class GetTicketListCriteria : CriteriaBase
{
    public int OWPMatchId { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_GetTicketByOwpMatchId";

    public override bool IsValid() => OWPMatchId > 0;

}