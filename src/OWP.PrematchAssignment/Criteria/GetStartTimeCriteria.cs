namespace OWP.PreMatchAssignment.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetStartTimeCriteria : CriteriaBase
    {
        public DateTime EventDate { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchAssignment_GetStartTimeForNightShift";

        public override bool IsValid() => true;
    }
}
