using Fanex.Data.Repository;
using NSubstitute;
using OWP.LiveDetail.Television;
using Xunit;

namespace OWP.LiveDetail.UnitTests.Television;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly TelevisionRepository televisionRepository;

    public RepositoryUnitTests()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
        televisionRepository = new TelevisionRepositoryImp(dynamicRepository);
    }

    [Fact]
    public async Task GetAll_Always_ReturnTelevisionList()
    {
        // Arrange
        var televisions = new List<LiveDetail.Television.Television> { new() { TVId = 1, TVCode = "tv1", Color = "#as" } };
        dynamicRepository.FetchAsync<LiveDetail.Television.Television>(Arg.Compat.Any<GetAllCriteria>()).Returns(televisions);

        // Act
        var actual = await televisionRepository.GetAll();

        // Assert
        Assert.Equal(televisions, actual);
        await dynamicRepository.Received().FetchAsync<LiveDetail.Television.Television>(Arg.Compat.Any<GetAllCriteria>());
    }
}