namespace OWP.LeagueManagement.Models;

using Newtonsoft.Json;
using System;
public class LeagueInfoResponse
{
    public bool HasLinkLeagueAndTeam { get; set; }

    public int? OWPLeagueId { get; set; }

    public int? GroupValue { get; set; }

    public string GroupLabel { get; set; }

    public string LeagueName { get; set; }

    public bool IsActive { get; set; }

    public bool IsBlackList { get; set; }

    public decimal? MinSpread { get; set; }

    public decimal? MaxSpread { get; set; }

    public decimal? MinMaxbetWD { get; set; }

    public decimal? MaxMaxbetWD { get; set; }

    public decimal? MinMaxbetWK { get; set; }

    public decimal? MaxMaxbetWK { get; set; }

    public decimal? MinFT1X2 { get; set; }

    public decimal? MaxFT1X2 { get; set; }

    public decimal? CN88MinSpread { get; set; }

    public decimal? CN88MaxSpread { get; set; }

    public decimal? CN88MinMaxbetWD { get; set; }

    public decimal? CN88MaxMaxbetWD { get; set; }

    public decimal? CN88MinMaxbetWK { get; set; }

    public decimal? CN88MaxMaxbetWK { get; set; }

    public decimal? CN88MinFT1X2 { get; set; }

    public decimal? CN88MaxFT1X2 { get; set; }

    public DateTime? LastUse { get; set; }
}