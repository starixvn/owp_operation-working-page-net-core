using Newtonsoft.Json;
using OWP.AssigneeSuggestionList.Helper;
using OWP.Constants;

namespace OWP.AssigneeSuggestionList.Models;

public class MatchInfo
{
    public int OwpMatchId { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public DateTime KOTime { get; set; }

    public DateTime EventDate { get; set; }

    public DateTime EventDateTime { get; set; }


    public DateTime EndTime { get; set; }

    public string MatchNo { get; set; }

    public decimal MaxbetHDP { get; set; }

    public string MaxbetHDPInString => ScheduleHelper.FormatMaxbet(MaxbetHDP);

    public string SuggestionBetType { get; set; }

    public string Remark { get; set; }

    public bool IsMainMatch { get; set; }

    public int ParentMatchId { get; set; }

    public int MinorBettypeId { get; set; }

    public int BettypeId { get; set; }

    public bool IsSaba { get; set; }

    public string BettypeName => BettypeId switch
    {
        Bettype.Handicap => "FT HDP",
        Bettype.OverUnder => "FT OU",
        Bettype.FirstHalfHandicap => "1H HDP",
        Bettype.FirstHalfOverUnder => "1H OU",
        _ => ""
    };
    [JsonIgnore]
    public bool IsSingleMatch { get; set; }
    public DateTime FilterDate { get; set; }
}
