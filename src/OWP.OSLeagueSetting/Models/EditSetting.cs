namespace OWP.OSLeagueSetting.Models;

public class EditSetting : AddOrEditSettingCommon
{
    public int LeagueSettingId { get; set; }


    public bool IsActive { get; set; }
}