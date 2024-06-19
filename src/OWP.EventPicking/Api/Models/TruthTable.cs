namespace OWP.EventPicking.Api.Models;

public class TruthTable
{
    public int LeagueSettingId { get; set; }

    public int TV { get; set; }

    public int Round { get; set; }

    public int Division { get; set; }

    public int NumberOfTrue { get; set; }

    public decimal? MaxbetWD_HDP { get; set; }

    public decimal? MaxbetWK_HDP { get; set; }
}
