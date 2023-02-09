using NSubstitute;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

public class BuildProviderUnitTests
{
    private readonly Repository repository;
    public BuildProviderUnitTests()
    {
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task Build_Always_ReturnCorrectProvider()
    {
        repository.GetProviderArrangement().Returns(MockProviderManagement());
        var matches = MockMatches();
        var service = new BuildProviderImp(repository);

        await service.Build(matches);

        Assert.Equal("", matches[0].Provider);
        Assert.Equal("rb2", matches[1].Provider);
        Assert.Equal("FC", matches[2].Provider);
        Assert.Equal("RB2TV", matches[3].Provider);
        Assert.Equal("abcRB2TVdef", matches[4].Provider);
        Assert.Equal("abcRB2aabbTV", matches[5].Provider);
    }


    public List<string> MockProviderManagement()
        => new List<string> { "RB2", "BG", "SM", "RTS", "BET", "FC" };

    public List<EventPickingModel> MockMatches()
        => new List<EventPickingModel>
        {
            new()
            {
                OtherProvider = ""
            },
            new()
            {
                OtherProvider = "rb2/ok/oke"
            },
            new()
            {
                OtherProvider = "FC/FCTV"

            },
            new()
            {
                OtherProvider = "RB2TV"
            },
            new()
            {
                OtherProvider = "abcRB2TVdef/aaa"
            },
            new()
            {
                OtherProvider = "abcRB2aabbTV/aaa"
            }
        };

}