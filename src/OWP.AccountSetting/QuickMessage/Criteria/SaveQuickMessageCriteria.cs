using Fanex.Data.Repository;

namespace OWP.ProviderArrangement.Criteria;

public class SaveQuickMessageCriteria : NonQueryCommand
{
    public int Id { get; set; }

    public string Shortcut { get; set; }

    public string Message { get; set; }

    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_AccountSetting_SaveQuickMessage";

    public override bool IsValid() => true;
}

