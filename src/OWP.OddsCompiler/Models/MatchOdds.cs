

namespace OWP.OddsCompiler.Models;
public class MatchOdds
{
    public int? Id { get; set; }

    public int OWPMatchId { get; set; }

    public int Bettype { get; set; }

    public int? TeamChoice { get; set; } = null;

    public decimal? HDP { get; set; }

    public decimal? Odds1 { get; set; }

    public decimal? Odds2 { get; set; }

    public decimal? Odds3 { get; set; }
}