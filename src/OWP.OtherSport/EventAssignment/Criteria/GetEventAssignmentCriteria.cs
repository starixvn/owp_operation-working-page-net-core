namespace OWP.OtherSport.EventAssignment.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetEventAssignmentCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Sports { get; set; }

        public string MatchStatuses { get; set; }

        public override string GetSettingKey() => "OWP_OSLiveGameAssignment_Query";

        public override bool IsValid() => true;
    }
}
