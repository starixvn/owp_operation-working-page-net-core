using Fanex.Data.Repository;
using NSubstitute;
using OWP.LiveDetail.Provider;
using Xunit;

namespace OWP.LiveDetail.UnitTests.Provider;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly ProviderRepository providerRepository;

    public RepositoryUnitTests()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
        providerRepository = new ProviderRepositoryImp(dynamicRepository);
    }

    [Fact]
    public async Task GetAll_Always_ReturnProviderList()
    {
        // Arrange
        var providers = new List<LiveDetail.Provider.Provider> { new() { ProviderId = 12, ProviderName = "provider" } };
        dynamicRepository.FetchAsync<LiveDetail.Provider.Provider>(Arg.Compat.Any<GetAllCriteria>()).Returns(providers);

        // Act
        var actual = await providerRepository.GetAll();

        // Assert
        Assert.Equal(providers, actual);
        await dynamicRepository.Received().FetchAsync<LiveDetail.Provider.Provider>(Arg.Compat.Any<GetAllCriteria>());
    }
}