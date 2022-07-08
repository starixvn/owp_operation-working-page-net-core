using Fanex.Data.Repository;

namespace OWP.OtherSport.Submit.Criteria
{
    public class GetMatchIdsByDateListCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey() => "OWP_OSAssignmentUpload_GetMatchIdsByDateList";

        public override bool IsValid() => true;
    }
}