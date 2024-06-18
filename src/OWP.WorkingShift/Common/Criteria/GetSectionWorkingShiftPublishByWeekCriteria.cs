namespace OWP.WorkingShift.Common.Criteria;

using System;
using Fanex.Data.Repository;

public class GetSectionWorkingShiftPublishByWeekCriteria : CriteriaBase
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string SectionIds { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_EmployeeWorkingShiftPublish_GetSectionByWeek";
}
