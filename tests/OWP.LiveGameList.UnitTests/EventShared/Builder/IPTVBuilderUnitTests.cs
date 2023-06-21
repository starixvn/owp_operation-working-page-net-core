using OWP.Event.EventShared.Builder;
using OWP.Event.EventShared.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Event.UnitTests.EventShared.Builder;

public class IPTVBuilderUnitTests
{
    [Fact]
    public void Build_Always_ReturnExpect()
    {
        const string expected = "PTQ/IPTV/Liam/stream76";
        var result = IPTVBuilder.Build(new IPTVModel
        { Channel = "Liam", IpTv = "IPTV", Stream = "stream76", Television = "PTQ" });
        DeepAssert.Equal(result, expected);

    }

    [Fact]
    public void Build_ModelNull_ReturnEmpty()
    {
        const string expected = "";
        var result = IPTVBuilder.Build(new IPTVModel());
        DeepAssert.Equal(result, expected);
    }
}

