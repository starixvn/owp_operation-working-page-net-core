using Fanex.Data.Repository;


namespace OWP.TicketCenter.Criteria;
public class GetAllMessageThreeDayAgoCriteria : CriteriaBase
{
    public int ActorId { get; set; }

    public int? SubtypeId { get; set; }

    public int TicketTypeId { get; set; }

    public override string GetSettingKey()
        => "OWP_TicketCenter_GetAllMessageThreeDayAgo";

    public override bool IsValid()
        => true;
}

