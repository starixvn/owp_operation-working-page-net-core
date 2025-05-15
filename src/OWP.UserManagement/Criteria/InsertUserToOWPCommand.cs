using Fanex.Data.Repository;

namespace OWP.UserManagement.Criteria;
public class InsertUserToOWPCommand : NonQueryCommand
{
    public int UserId { get; set; }

    public string Username { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; } = string.Empty;

    public override string GetSettingKey() => "OWP_SyncUserFromNAP";

    public override bool IsValid() => UserId > 0;
}
