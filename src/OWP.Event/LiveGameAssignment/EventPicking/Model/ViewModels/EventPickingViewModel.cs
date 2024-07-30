namespace OWP.Event.LiveGameAssignment.EventPicking.Model.ViewModels;

using System;
using System.Collections.Generic;

public class Pagination
{
    public int ItemsPerPage { get; set; }

    public int CurrentPage { get; set; }

    public int TotalItem { get; set; }
}

public class League
{
    public int Id { get; set; }

    public string Name { get; set; }
}

public class EventPickingViewModel : Pagination
{
    private const int Thousand = 1000;
    private const int Hundred = 100;

    public EventPickingViewModel()
    {
        Leagues = new List<League>();
        ErrorMessages = [];
    }

    public DateTime Date { get; set; }

    public DateTime? FromTime { get; set; }

    public DateTime? ToTime { get; set; }

    public decimal? FromMaxBet { get; set; }

    public decimal? ToMaxBet { get; set; }

    public decimal? FromSpread { get; set; }

    public decimal? ToSpread { get; set; }

    public string LeagueIds { get; set; }

    public IEnumerable<League> Leagues { get; set; }

    public decimal? SqlParamFromMaxBet => FromMaxBet * Thousand;

    public decimal? SqlParamToMaxBet => ToMaxBet.HasValue ? ToMaxBet.Value * Thousand : null;

    public decimal? SqlParamFromSpread => FromSpread.HasValue ? FromSpread.Value / Hundred : null;

    public decimal? SqlParamToSpread => ToSpread / Hundred;

    public List<string> ErrorMessages { get; set; }
}