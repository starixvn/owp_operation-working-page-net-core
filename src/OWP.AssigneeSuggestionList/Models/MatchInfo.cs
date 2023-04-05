using OWP.AssigneeSuggestionList.Helper;

namespace OWP.AssigneeSuggestionList.Models;

public class MatchInfo
{
    public string OwpMatchId { get; set; }
    public string LeagueName { get; set; }
    public string HomeName { get; set; }
    public string AwayName { get; set; }
    public DateTime KOTime { get; set; }
    public DateTime EventDate { get; set; }
    public string MatchNo { get; set; }
    public decimal MaxbetHDP { get; set; }
    public string MaxbetHDPInString => ScheduleHelper.FormatMaxbet(MaxbetHDP);
    public string SuggestionBetType { get; set; }
    public int MinorBettypeId { get; set; }
}
