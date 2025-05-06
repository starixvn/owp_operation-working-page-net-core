using Fanex.Data.Repository;

namespace OWP.Api.Chat.Criteria;
public class InsertUserToOWPCommand(int userId) : NonQueryCommand
{
    public int UserId { get; set; } = userId;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; } = string.Empty;

    public override string GetSettingKey() => "OWP_UserInsert";

    public override bool IsValid() => UserId > 0;
}
