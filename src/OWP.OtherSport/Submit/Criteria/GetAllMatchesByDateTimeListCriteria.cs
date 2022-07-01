namespace OWP.OtherSport.Submit.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllMatchesByDateTimeListCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey() => "OWP_OSAssignmentUpload_GetAllMatchesByDateList";

        public override bool IsValid() => true;
    }
}