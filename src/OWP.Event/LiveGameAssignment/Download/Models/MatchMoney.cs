namespace OWP.Event.LiveGameAssignment.Download.Models;

public class MatchMoney
{
    public int OWPMatchId { get; set; }

    public int ParentMatchId { get; set; }

    public int Bettype { get; set; }

    public int? MinorBettypeId { get; set; }

    public decimal? MaxBet { get; set; }

    public decimal? Spread { get; set; }
}