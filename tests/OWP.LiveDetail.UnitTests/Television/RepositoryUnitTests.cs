namespace OWP.LiveDetail.UnitTests.Television;

using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using LiveDetail.Television;
using NSubstitute;
using Xunit;

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
        var televisions = new List<Television> { new Television { TVId = 1, TVCode = "tv1", Color = "#as" } };
        dynamicRepository.FetchAsync<Television>(Arg.Compat.Any<GetAllCriteria>()).Returns(televisions);

        // Act
        var actual = await televisionRepository.GetAll();

        // Assert
        Assert.Equal(televisions, actual);
        await dynamicRepository.Received().FetchAsync<Television>(Arg.Compat.Any<GetAllCriteria>());
    }
}