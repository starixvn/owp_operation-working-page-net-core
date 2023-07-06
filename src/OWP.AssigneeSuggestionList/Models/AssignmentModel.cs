using Newtonsoft.Json;
using OWP.AssigneeSuggestionList.Helper;
using OWP.Extensions;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Models;

public class AssignmentModel
{
    public int OWPMatchId { get; set; }

    public TimeSpan KOTime { get; set; }

    public DateTime EventDateTime { get; set; }

    public DateTime EventDate { get; set; }

    public int BettypeId { get; set; }

    public string Remark { get; set; }

    public string LeagueName { get; set; }

    public int UserId { get; set; }

    public bool IsMajorLeague { get; set; }

    public bool IsMainMatch { get; set; }

    public bool IsFantasyMatch => LeagueName.IsContains("Fantasy ");

    public bool IsCornerMatch { get; set; }

    public bool Is15Mins { get; set; }

    public MatchType MatchType { get; set; } = MatchType.Other;

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public int? ParentMatchId { get; set; }

    public bool IsSaba => LeagueName.IsContains("SABA ");

    public decimal? MaxbetHDP { get; set; }

    public decimal? SpreadHDP { get; set; }

    public DateTime EndTime { get; set; }

    public int? MinorBettypeId { get; set; }

    public string AssignmentNote { get; set; }

    [JsonIgnore]
    public List<AssignmentModel> GroupAssignmentModels { get; set; } = new();

    public bool IsSingleMatch => SingleMatchHelper.IsSingleMatch(MaxbetHDP, SpreadHDP, LeagueName);
}