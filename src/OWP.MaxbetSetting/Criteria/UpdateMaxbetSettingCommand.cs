namespace OWP.MaxbetSetting.Criteria;

using Fanex.Data.Repository;

public class UpdateMaxbetSettingCriteria : NonQueryCommand
{
    public decimal FT_HDP { get; set; }

    public string Column { get; set; }

    public decimal? Value { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_MaxbetSetting_Update";

    public override bool IsValid() => true;
}
