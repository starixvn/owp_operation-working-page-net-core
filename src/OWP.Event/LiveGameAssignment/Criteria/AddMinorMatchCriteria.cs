namespace OWP.Event.LiveGameAssignment.Criteria;

using System;
using System.Globalization;
using Fanex.Data.Repository;

public class AddMinorMatchCriteria : NonQueryCommand
{
    public int ParentMatchId { get; set; }

    public int MinorBettypeId { get; set; }

    public DateTime EventDate { get; set; }

    public string MatchStatus { get; set; }

    public string MatchNo { get; set; }

    public TimeSpan KickOffTime { get; set; }

    public TimeSpan? OtherTime { get; set; }

    public int? InfoSiteMatchId { get; set; }

    public string Website1 { get; set; }

    public string Website2 { get; set; }

    public string Website3 { get; set; }

    public string Website4 { get; set; }

    public int ActorId { get; set; }

    public string SessionId { get; private set; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public string Channel { get; set; }

    public string WB { get; set; }

    public string Phone { get; set; }

    public string Remark { get; set; }

    public int? NoOfHDPId { get; set; }

    public bool IsSoloMatch { get; set; }

    public string Round { get; set; }

    public string Division { get; set; }

    public string Venue { get; set; }

    public string TVIds { get; set; }

    public string OtherProvider { get; set; }

    public string Provider { get; set; }

    public string AgentIds { get; set; }

    public string Market { get; set; }

    public string MatchOfferIds { get; set; }

    public int MinorMatchId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_AddMinorMatch";

    public override bool IsValid() => ParentMatchId > 0 && MinorBettypeId != 0;
}