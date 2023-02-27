using Fanex.Data.Repository;
using NSubstitute;
using OWP.LiveDetail.OtherProvider;
using Xunit;

namespace OWP.LiveDetail.UnitTests.OtherProvider;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly OtherProviderRepository otherProviderRepository;

    public RepositoryUnitTests()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
        otherProviderRepository = new OtherProviderRepositoryImp(dynamicRepository);
    }

    [Fact]
    public async Task GetAll_Always_ReturnOtherProviderList()
    {
        // Arrange
        var otherProviders = new List<LiveDetail.OtherProvider.OtherProvider> { new() { OtherProviderId = 19, OtherProviderName = "pro" } };
        dynamicRepository.FetchAsync<LiveDetail.OtherProvider.OtherProvider>(Arg.Compat.Any<GetAllCriteria>()).Returns(otherProviders);

        // Act
        var actual = await otherProviderRepository.GetAll();

        // Assert
        Assert.Equal(otherProviders, actual);
        await dynamicRepository.Received().FetchAsync<LiveDetail.OtherProvider.OtherProvider>(Arg.Compat.Any<GetAllCriteria>());
    }
}