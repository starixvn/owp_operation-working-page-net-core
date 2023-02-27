using OWP.Extensions;

namespace OWP.LiveDetail.MatchOdd;

[XMLName("Row")]
public class MatchOddUpdateRow
{
    public int Choice { get; set; }

    public double Odds { get; set; }
}