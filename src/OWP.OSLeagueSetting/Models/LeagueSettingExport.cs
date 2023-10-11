
namespace OWP.OSLeagueSetting.Models;
public class LeagueSettingExport
{
    public bool IsActive { get; set; }

    public int No { get; set; }

    public string Status => IsActive ? "Active" : "Inactive";

    public string OSLeagueName { get; set; }

    public string Region { get; set; }

    public string CategoryName { get; set; }

    public string RoundName { get; set; }

    public string OfferLines { get; set; }

    public string BetDelays { get; set; }

    public string SummaryFirst { get; set; }

    public string SummarySecond { get; set; }

    public double? MaxBetHDP { get; set; }

    public double? MaxBetOU { get; set; }

    public string Remark { get; set; }

}
