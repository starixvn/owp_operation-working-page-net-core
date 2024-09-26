namespace OWP.WorkingShift.DailyWorkingShift.Criteria;

using System;
using Fanex.Data.Repository;

public class QueryCriteria : CriteriaBase
{
    public DateTime? Date { get; set; }

    public string User { get; set; }

    public string AllUserJson { get; set; }

    public string SectionId { get; set; }

    public string DepartmentId { get; set; }

    public string ShiftId { get; set; }

    public string LocationCode { get; set; }

    public bool ExcludeClosedAccount { get; set; }

    public override string GetSettingKey() => "OWP_EmployeeWorkingShift_QueryDaily";

    public override bool IsValid() => Date.HasValue
        && !string.IsNullOrWhiteSpace(SectionId)
        && !string.IsNullOrWhiteSpace(LocationCode);
}

public class AllUser
{
    public int UserId { get; set; }

    public bool IsActive {
        get;
        set;
    }
}