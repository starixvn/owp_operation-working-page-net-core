namespace OWP.LiveDetail.MatchSetting;

using System;
using System.Globalization;

public class CreateModel
{
    public string SessionId { get; private set; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public int UserId { get; set; }

    public int? MinorBettypeId { get; set; }

    public int MatchId { get; set; }

    public int BettypeId { get; set; }

    public string Bettype { get; set; }

    public decimal? Maxbet { get; set; }

    public decimal? Spread { get; set; }
}