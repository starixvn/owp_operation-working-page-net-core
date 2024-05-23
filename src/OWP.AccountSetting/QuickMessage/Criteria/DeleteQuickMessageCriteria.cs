using Fanex.Data.Repository;

namespace OWP.AccountSetting.QuickMessage.Criteria;

public class DeleteQuickMessageCriteria : NonQueryCommand
{
    public int Id { get; set; }

    public override string GetSettingKey() => "OWP_AccountSetting_DeleteQuickMessage";

    public override bool IsValid() => true;
}

