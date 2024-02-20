namespace OWP.Event.LiveGameAssignment.Criteria;

using System;
using Fanex.Data.Repository;

public class ValidateToEditEventCriteria : NonQueryCommand
{
    public TimeSpan KickOffTime { get; set; }

    public string MatchNo { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public int OWPMatchId { get; set; }

    public int? MinorBettypeId { get; set; }

    public int? ParentMatchId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_ValidateToEditEvent";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(LeagueName)
        && !string.IsNullOrWhiteSpace(HomeName)
        && !string.IsNullOrWhiteSpace(AwayName)
        && OWPMatchId > 0;
}