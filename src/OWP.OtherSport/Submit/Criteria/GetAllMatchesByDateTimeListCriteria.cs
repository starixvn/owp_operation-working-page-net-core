namespace OWP.OtherSport.Submit.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetAllMatchesByDateTimeListCriteria: CriteriaBase
    {
        public string EventDates { get; set; }

        public override string GetSettingKey() => "OWP_OSAssignmentUpload_GetAllMatchesByDateList";

        public override bool IsValid() => true;
    }
}
