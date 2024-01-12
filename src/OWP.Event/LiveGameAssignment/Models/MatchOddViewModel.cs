namespace OWP.Event.LiveGameAssignment.Models;

using System.Collections.Generic;
using System.Xml.Serialization;

public class MatchOddViewModel
{
    public int Bettype { get; set; }

    public int TeamChoice { get; set; }

    public float Handicap { get; set; }

    public string RecommendOddsXml { get; set; }

    [XmlIgnore]
    public List<MatchOddUpdateRow> RecommendOdds { get; set; }
}