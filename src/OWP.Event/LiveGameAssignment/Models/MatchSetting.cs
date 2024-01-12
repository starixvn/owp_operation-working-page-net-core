namespace OWP.Event.LiveGameAssignment.Models;

using Utils;

public class MatchSetting
{
    public int OWPMatchId { get; set; }

    public int? ParentMatchId { get; set; }

    public int Bettype { get; set; }

    public int? MinorBettypeId { get; set; }

    public decimal? Spread { get; set; }

    public decimal? MaxBet { get; set; }

    public string MaxBetToDisplay => MaxBet.ToOWPMaxBetFormat();

    public string SpreadToDisplay => Spread.HasValue ? Spread.ToOWPSpreadFormat() + "%" : string.Empty;
}