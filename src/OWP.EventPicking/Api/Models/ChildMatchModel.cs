namespace OWP.EventPicking.Api.Models;
public class ChildMatchModel
{
    public int ParentMatchId { get; set; }

    public int MinorBettypeId { get; set; }

    public decimal? MaxbetHDP { get; set; }

    public decimal? MaxbetOU { get; set; }

    public decimal? SpreadHDP { get; set; }

    public decimal? SpreadOU { get; set; }

    public string NoOfHDP { get; set; }

}