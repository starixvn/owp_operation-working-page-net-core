namespace OWP.WorkingShift.Common.Criteria;
public class UpdateTimezoneCriteria : NonQueryCommand
{
    public int UserId { get; set; }

    public byte SettingId { get; set; }

    public override bool IsValid() => UserId > 0 && SettingId > 0;

    public override string GetSettingKey() => "OWP_EmployeeWorkingShiftTimeZone_UpdateUserSetting";
}
