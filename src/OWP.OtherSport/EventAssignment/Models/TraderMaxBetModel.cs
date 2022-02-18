using OWP.OtherSport.Submit.Models;


namespace OWP.OtherSport.EventAssignment.Models
{
    public class TraderMaxBetModel
    {
        public string BetTypeName { get; set; }
        public TraderAssignment TraderAssignment { get; set; } = new TraderAssignment();
        public List<MatchSettingModel> MatchSettings { get; set; } = new List<MatchSettingModel>();
    }
}