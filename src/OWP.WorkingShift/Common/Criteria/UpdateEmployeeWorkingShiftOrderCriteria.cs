namespace OWP.WorkingShift.Common.Criteria;

using Fanex.Data.Repository;

public class UpdateEmployeeWorkingShiftOrderCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int OrderUserId { get; set; }

    public int SectionId { get; set; }

    public string UserIds { get; set; }

    public override string GetSettingKey() => "OWP_EmployeeWorkingShift_SaveOrder";

    public override bool IsValid() => ActorId > 0 && SectionId > 0 && OrderUserId >= 0 && !string.IsNullOrEmpty(UserIds);
}