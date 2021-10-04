using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class GetRecipientIdInManyGroupCriteria: CriteriaBase
    {
        public string GroupIds { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketCenter_GetRecipientIdInManyGroup";


        public override bool IsValid()
            => true;
    }
}
