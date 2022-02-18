namespace OWP.PreMatchReport.Models;

public class ReportModel
{
    public int UserId { get; set; }
    public string TraderName { get; set; }
    public int TotalMatches { get; set; }
    public int TotalLeague { get; set; }
    public ReportHDPLeagueModel ReportHDPLeague { get; set; }
    public ReportHDPMatchesModel ReportHDPMatches { get; set; }
    public ReportOULeagueModel ReportOULeague {get; set;}
    public ReportOUMatchesModel ReportOUMatches {get;set;}

}