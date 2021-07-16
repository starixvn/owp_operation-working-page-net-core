namespace OWP.PreMatchNotification.LinkedMatch.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetLGAMatchesByTimeRangeCriteria: CriteriaBase
    {
        public DateTime EventDate { get; set; }

        public override string GetSettingKey() => "OWP_AssignmentUpload_AllMatchesByTimeRange";

        public override bool IsValid() => true;
    }
}
