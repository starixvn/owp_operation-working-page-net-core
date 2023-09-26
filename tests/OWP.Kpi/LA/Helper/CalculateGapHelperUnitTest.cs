using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Kpi.UnitTests.LA.Helper;
public class CalculateGapHelperUnitTest
{
    [Fact]
    public void CalculateGap_TargetIdEqual1_ReturnExpect()
    {
        var totalGame = new TotalGameInfo
        {
            MultipleMatches = 2335,
            VirtualMatches = 1000,
        };

        var result = CalculateGapHelper.CalculateGap(1, totalGame);

        var expect = new TotalGameInfo
        {
            MultipleMatches = 335,
            VirtualMatches = 950,

        };
        DeepAssert.Equal(expect, result);
    }


    [Fact]
    public void CalculateGap_TargetIdNull_ReturnExpect()
    {
        var totalGame = new TotalGameInfo
        {
            MultipleMatches = 2335,
            VirtualMatches = 1000,
        };

        var result = CalculateGapHelper.CalculateGap(null, totalGame);

        DeepAssert.Equal(new TotalGameInfo(), result);
    }
}
