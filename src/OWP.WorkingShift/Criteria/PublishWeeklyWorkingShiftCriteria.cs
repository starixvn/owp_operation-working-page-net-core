namespace OWP.WorkingShift.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class PublishWeeklyWorkingShiftCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public int SectionId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsPublished { get; set; }

        public override bool IsValid() => SectionId > 0;

        public override string GetSettingKey() => "OWP_EmployeeWorkingShiftPublish_ManualPublish";
    }
}