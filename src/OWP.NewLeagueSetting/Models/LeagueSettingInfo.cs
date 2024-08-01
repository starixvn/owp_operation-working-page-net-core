
namespace OWP.NewLeagueSetting.Models;

[Serializable]
public class LeagueSettingInfo : LeagueSettingCore
{
    public string LeagueGroup { get; set; } = default!;

    public string LeagueGroupNote { get; set; } = default!;

    public string League { get; set; } = default!;

    public string FullConditionDescription { get; set; } = default!;

    public string Comment { get; set; } = default!;

    public string LstNote { get; set; } = default!;

    public string Remark2 { get; set; } = default!;
}
