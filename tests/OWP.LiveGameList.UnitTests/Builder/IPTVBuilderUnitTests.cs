using OWP.LiveGameList.Builder;
using OWP.LiveGameList.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.LiveGameList.UnitTests.Builder;

public class IPTVBuilderUnitTests
{
    [Fact]
    public void Build_Always_ReturnExpect()
    {
        var expected = "PTQ/IPTV/Liam/stream76";
        var result = IPTVBuilder.Build(new IPTVModel
        { Channel = "Liam", IpTv = "IPTV", Stream = "stream76", Television = "PTQ" });
        DeepAssert.Equal(result, expected);

    }

    [Fact]
    public void Build_ModelNull_ReturnEmpty()
    {
        var expected = "";
        var result = IPTVBuilder.Build(new IPTVModel());
        DeepAssert.Equal(result, expected);
    }
}

