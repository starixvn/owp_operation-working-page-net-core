namespace OWP.SimpleDataApi.Criteria.EmployeeWorkingShift;

using Fanex.Data.Repository;

public class UserRemarkUpdateCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int UserId { get; set; }

    public int Category { get; set; }

    public string Remark { get; set; }

    public override bool IsValid() => ActorId > 0 && UserId > 0 && Category > 0 && Remark != null;

    public override string GetSettingKey() => "OWP_UserRemark_Update";
}
