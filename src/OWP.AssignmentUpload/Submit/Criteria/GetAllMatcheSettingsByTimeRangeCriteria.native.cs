namespace OWP.AssignmentUpload.Submit.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetAllMatcheSettingsByTimeRangeCriteria : CriteriaBase
    {
        public DateTime EventDate { get; set; }

        public override string GetSettingKey() => "OWP_AssignmentUpload_AllMatchSettingsByTimeRange";

        public override bool IsValid() => true;
    }
}
