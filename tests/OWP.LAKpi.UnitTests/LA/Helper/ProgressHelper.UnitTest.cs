using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Models;
using Xunit;

namespace OWP.LAKpi.UnitTests.LA.Helper;
public class ProgressHelperUnitTest
{

    [Fact]
    public void IsGreaterThanOrEqualTo_AllPropertiesMatch_ReturnsTrue()
    {
        TotalGameInfo totalGame = new TotalGameInfo
        {
            MultipleMatches = 150,
            VirtualMatches = 80
        };

        TotalGameInfo condition = new TotalGameInfo
        {
            MultipleMatches = 100,
            VirtualMatches = 50
        };

        bool result = ProgressHelper.IsGreaterThanOrEqualTo(totalGame, condition);

        Assert.True(result);
    }
}
