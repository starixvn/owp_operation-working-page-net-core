using OWP.TaskList.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.TaskList.UnitTests.Builder;


public class IPTVBuilderUnitTests
{
    [Fact]
    public void Build_Always_ReturnExpect()
    {
        var expected = "PTQ/IPTV/Liam/stream76";
        var result = IPTVBuilder.Build(new IPTVModel
            { Channel = "Liam", IpTv = "IPTV", Stream = "stream76", TeleVision = "PTQ" });
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

