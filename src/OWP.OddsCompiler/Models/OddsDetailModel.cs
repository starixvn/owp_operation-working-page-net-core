

namespace OWP.OddsCompiler.Models;
public class OddsDetailModel
{
    public int Id { get; set; }

    public int OWPMatchId { get; set; }

    public int Bettype { get; set; }

    public decimal? HDP { get; set; }

    public int? TeamChoice { get; set; }

    public short Choice { get; set; }

    public decimal? Odds { get; set; }
}