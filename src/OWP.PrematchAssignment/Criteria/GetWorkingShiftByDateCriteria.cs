namespace OWP.PreMatchAssignment.Criteria;

using Fanex.Data.Repository;

public class GetWorkingShiftByDateCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public int UserId { get; set; }

    public override bool IsValid() => UserId > 0;

    public override string GetSettingKey() => "OWP_PreMatchAssignment_GetWorkingShift";
}