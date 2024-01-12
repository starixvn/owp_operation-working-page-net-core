namespace OWP.Event.LiveGameAssignment.Criteria;

using System;
using Fanex.Data.Repository;

public class ValidateToAddEventCriteria : NonQueryCommand
{
    public DateTime EventDate { get; set; }

    public TimeSpan KickOffTime { get; set; }

    public string MatchStatus { get; set; }

    public string MatchNo { get; set; }

    public int? InfoSiteMatchId { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public string MatchSettingBettypeIds { get; set; }

    public int? ExistedMatchId { get; set; }

    public int? MinorBettypeId { get; set; }

    public int? ParentMatchId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_ValidateToAddEvent";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(LeagueName)
        && !string.IsNullOrWhiteSpace(HomeName)
        && !string.IsNullOrWhiteSpace(AwayName)
        && !string.IsNullOrWhiteSpace(MatchStatus);
}