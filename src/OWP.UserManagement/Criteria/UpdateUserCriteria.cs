namespace OWP.UserManagement.Criteria;

using Fanex.Data.Repository;

public class UpdateUserCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int UserId { get; set; }

    public string ColumnName { get; set; }

    public string Value { get; set; }

    public override string GetSettingKey() => "OWP_UserUpdate";

    public override bool IsValid() => ActorId > 0 && UserId > 0;
}