using Fanex.Data.Repository;

namespace OWP.OtherSport.Submit.Criteria
{
    public class GetMatchIdsByDateListCriteria : CriteriaBase
    {
        public string EventDates { get; set; }

        public override string GetSettingKey() => "OWP_OSAssignmentUpload_GetMatchIdsByDateList";

        public override bool IsValid() => true;
    }
}