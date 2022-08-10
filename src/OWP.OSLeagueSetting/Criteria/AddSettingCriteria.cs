namespace OWP.OSLeagueSetting.Criteria;

public class AddSettingCriteria : AddOrEditSettingCommonCriteria
{

    public override string GetSettingKey() => "OWP_OSLeagueSetting_Add";

    public override bool IsValid() => LeagueName?.Length > 0
    && UserId > 0;
}