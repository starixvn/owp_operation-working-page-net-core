namespace OWP.SimpleDataApi.Criteria.EmployeeWorkingShift;

using System;
using Fanex.Data.Repository;

public class WorkingShiftQueryByUserCriteria : CriteriaBase
{
    public string UserIds { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public override bool IsValid() => !string.IsNullOrEmpty(UserIds) && FromDate.HasValue && ToDate.HasValue;

    public override string GetSettingKey() => "OWP_EmployeeWorkingShift_QueryWeeklyByUser";
}
