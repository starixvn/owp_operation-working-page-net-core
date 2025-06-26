namespace OWP.Event.LiveGameAssignment.Criteria;

using System;
using System.Globalization;
using Fanex.Data.Repository;

public class EditMainMatchToMinorMatchCriteria : NonQueryCommand
{
    public int OWPMatchId { get; set; }

    public int? ParentMatchId { get; set; }

    public int? MinorBettypeId { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public string MatchNo { get; set; }

    public int ActorId { get; set; }

    public string SessionId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_EditMainMatchToMinorMatch";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(LeagueName)
        && !string.IsNullOrWhiteSpace(HomeName)
        && !string.IsNullOrWhiteSpace(AwayName);
}