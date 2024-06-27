namespace OWP.Event.LiveGameAssignment.Models;

using Extensions;

[XMLName("Row")]
public class MatchOddUpdateRow
{
    public int Choice { get; set; }

    public double Odds { get; set; }
}