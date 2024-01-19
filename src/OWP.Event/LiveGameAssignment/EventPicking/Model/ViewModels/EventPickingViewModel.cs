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
        ErrorMessages = new List<string>();
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

    public decimal? SqlParamFromMaxBet
    {
        get
        {
            return FromMaxBet * Thousand;
        }
    }

    public decimal? SqlParamToMaxBet => ToMaxBet.HasValue ? ToMaxBet.Value * Thousand : null;

    public decimal? SqlParamFromSpread => FromSpread.HasValue ? FromSpread.Value / Hundred : null;

    public decimal? SqlParamToSpread
    {
        get
        {
            return ToSpread / Hundred;
        }
    }

    public List<string> ErrorMessages { get; set; }

    public bool IsValid()
    {
        var isValid = true;

        CheckKickOffTime(ref isValid);
        CheckMaxBet(ref isValid);
        CheckSpread(ref isValid);

        return isValid;
    }

    private void CheckKickOffTime(ref bool isValid)
    {
        if (!FromTime.HasValue && ToTime.HasValue
            || FromTime.HasValue && !ToTime.HasValue
            || FromTime.HasValue && ToTime.HasValue && DateTime.Compare(FromTime.Value, ToTime.Value) > 0)
        {
            ErrorMessages.Add("'From Kick Off Time' must be less than or equal to 'To Kick Off Time'");
            isValid = false;
        }
    }

    private void CheckMaxBet(ref bool isValid)
    {
        if (!FromMaxBet.HasValue && ToMaxBet.HasValue
            || FromMaxBet.HasValue && !ToMaxBet.HasValue
            || FromMaxBet.HasValue && ToMaxBet.HasValue && FromMaxBet.Value > ToMaxBet.Value)
        {
            ErrorMessages.Add("'From Max Bet' must be less than or equal to 'To Max Bet'");
            isValid = false;
        }
    }

    private void CheckSpread(ref bool isValid)
    {
        if (!FromSpread.HasValue && ToSpread.HasValue
            || FromSpread.HasValue && !ToSpread.HasValue
            || FromSpread.HasValue && ToSpread.HasValue && FromSpread.Value > ToSpread.Value)
        {
            ErrorMessages.Add("'From Spread' must be less than or equal to 'To Spread'");
            isValid = false;
        }
    }
}