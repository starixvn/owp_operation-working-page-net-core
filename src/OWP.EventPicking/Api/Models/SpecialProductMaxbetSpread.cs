namespace OWP.EventPicking.Api.Models;
public class SpecialProductMaxbetSpread
{
    public int ParentId { get; set; }

    public  int MinorBettypeId { get; set; }

    public decimal? SpreadHdp { get; set; }

    public decimal? SpreadOu { get; set; }

    public decimal? MaxbetHdp { get; set; }


    public string NoOfHDP { get; set; }
}
