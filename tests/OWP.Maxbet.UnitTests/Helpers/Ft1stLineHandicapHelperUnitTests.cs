namespace OWP.Maxbet.UnitTests.Helpers;

using Maxbet.Helpers;
using Xunit;

public class Ft1stLineHandicapHelperUnitTests
{
    [Fact]
    public void BuildFt1stLineHandicap_Return_Ft1stLineHandicap()
    {
        var expected = 0;

        var actual = Ft1stLineHandicapHelper.BuildFt1stLineHandicap();

        Assert.Equal(expected, actual);
    }
}