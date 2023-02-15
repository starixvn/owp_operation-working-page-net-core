namespace OWP.LiveDetail.UnitTests.Provider;

using Fanex.Data.Repository;
using LiveDetail.Provider;
using NSubstitute;
using Xunit;

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
        var providers = new List<Provider> { new() { ProviderId = 12, ProviderName = "provider" } };
        dynamicRepository.FetchAsync<Provider>(Arg.Compat.Any<GetAllCriteria>()).Returns(providers);

        // Act
        var actual = await providerRepository.GetAll();

        // Assert
        Assert.Equal(providers, actual);
        await dynamicRepository.Received().FetchAsync<Provider>(Arg.Compat.Any<GetAllCriteria>());
    }
}