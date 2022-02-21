namespace OWP.TicketCenter.Criteria
{
    using Fanex.Data.Repository;
    public class GetSubTypeCriteria : CriteriaBase
    {
        public int TicketTypeId { get; set; }
        public override string GetSettingKey() => "OWP_TicketCenter_GetSubTypeByTicketTypeId";
        public override bool IsValid() => true;
    }
}