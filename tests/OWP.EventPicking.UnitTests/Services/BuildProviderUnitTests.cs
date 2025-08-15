using NSubstitute;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

public class BuildProviderUnitTests
{
    private readonly OWP.EventPicking.Repository repository;
    public BuildProviderUnitTests()
    {
        repository = Substitute.For<OWP.EventPicking.Repository>();
    }

    [Fact]
    public async Task Build_Always_ReturnCorrectProvider()
    {
        // Arrange
        repository.GetProviderArrangement().Returns(MockProviderManagement());
        var matches = MockMatches();
        var service = new BuildProviderImp(repository);

        // Act
        await service.Build(matches);

        // Assert
        Assert.Equal("TV", matches[0].Provider);
        Assert.Equal("TV/RB2", matches[1].Provider);
        Assert.Equal("RB2", matches[2].Provider);
        Assert.Equal(null, matches[3].Provider);
    }



    public List<string> MockProviderManagement() => ["RB2", "BG", "SM", "RTS", "BET", "FC"];

    public List<EventPickingModel> MockMatches()
        =>
        [
            new EventPickingModel
            {
                TV = "SomeTV",
                Channel = null,
                Iptv = null,
                Provider = null
            },

            new EventPickingModel
            {
                TV = "SomeTV",
                Channel = null,
                Iptv = null,
                Provider = "RB2"
            },

            new EventPickingModel
            {
                TV = null,
                Channel = null,
                Iptv = null,
                Provider = "RB2"
            },

            new EventPickingModel
            {
                TV = null,
                Channel = null,
                Iptv = null,
                Provider = null
            },
        ];

}