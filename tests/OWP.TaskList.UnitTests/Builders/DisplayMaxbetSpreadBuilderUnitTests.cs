using OWP.TaskList.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.TaskList.UnitTests.Builder;

public class DisplayMaxbetSpreadBuilderUnitTests
{
    [Fact]
    public void BuildMaxBet_AlwaysReturns_MaxBetString()
    {
        var expected = "20K (15%)";
        var actual = DisplayMaxbetSpreadBuilder.Build(20000, (decimal)0.15);
        Assert.Equal(expected, actual);
    }
}
