namespace OWP.LiveDetail.MatchOdd;

using OWP.Extensions;

[XMLName("Row")]
public class MatchOddUpdateRow
{
    public int Choice { get; set; }

    public double Odds { get; set; }
}