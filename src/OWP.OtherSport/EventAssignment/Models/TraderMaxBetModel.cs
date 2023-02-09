namespace OWP.OtherSport.EventAssignment.Models;

public class TraderMaxBetModel
{
    public string BetTypeName { get; set; }
    public TraderAssignment TraderAssignment { get; set; } = new();
    public List<MatchSettingModel> MatchSettings { get; set; } = new();
}