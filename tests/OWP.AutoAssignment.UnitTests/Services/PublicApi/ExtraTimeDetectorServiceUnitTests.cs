using OWP.AutoAssignment.Services.PublicApi;
using Xunit;

namespace OWP.AutoAssignment.UnitTests.Services.PublicApi;

public class ExtraTimeDetectorServiceUnitTests
{
    [Theory]
    [InlineData("ETPK", "ETPK")]
    [InlineData("Pls cfm this game have ET, PK or not", "ETPK")]
    [InlineData("PK", "PK")]
    public void DetectEtPkByExtraTime_ReturnEtPkCorrect(string extraTime, string expected)
    {
        var service = new ExtraTimeDetectorServiceImp();
        var actual = service.DetectExtraTime(extraTime);
        Assert.Equal(expected, actual);
    }
}