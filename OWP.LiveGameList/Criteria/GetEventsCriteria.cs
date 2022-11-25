using Fanex.Data.Repository;

namespace OWP.LiveGameList.Criteria
{
    public class GetEventsCriteria : CriteriaBase
    {
        public DateTime EventDate { get; set; }

        public int Status { get; set; }

        public override bool IsValid()
        {
            return EventDate > DateTime.MinValue;
        }

        public override string GetSettingKey() => "OWP_EventInfo_Get";

    }
}
