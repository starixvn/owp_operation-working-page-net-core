using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.TicketCenter
{
    public class GetAllRecipientCriteria : CriteriaBase
    {
        public bool IncludeGroup { get; set; }

        public int SiteId { get; set; }

        public override string GetSettingKey() => "OWP_TicketSetting_GetAllRecipients";

        public override bool IsValid() => true;
    }
}