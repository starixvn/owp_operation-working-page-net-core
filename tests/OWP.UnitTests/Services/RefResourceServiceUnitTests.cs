using Fanex.Data.Repository;
using NSubstitute;
using OWP.Criteria;
using OWP.Models;
using OWP.Services;
using Xunit;

namespace OWP.UnitTests.Services;

public class RefResourceServiceUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly RefResourceService refResourceService;

    public RefResourceServiceUnitTests()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
        refResourceService = new RefResourceServiceImp(dynamicRepository);
    }

    [Fact]
    public void GetAllLocations_ShouldReturnAllLocations()
    {
        // Act
        var result = refResourceService.GetAllLocations();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(11, result.Count);

        var expectedLocations = new[]
        {
            "CSC", "CTI", "DBS", "NTX", "SMS",
            "WAH HCMC", "WAH MAL", "WAH MNL", "WGI", "XPC", "None"
        };

        foreach (var expectedLocation in expectedLocations)
        {
            Assert.Contains(result, x => x.LocationCode == expectedLocation);
        }
    }

    [Fact]
    public void GetAllLocations_ShouldReturnCorrectLocationNames()
    {
        // Act
        var result = refResourceService.GetAllLocations();

        // Assert
        var cscLocation = result.First(x => x.LocationCode == "CSC");
        Assert.Equal("CSC", cscLocation.LocationName);

        var wahHcmcLocation = result.First(x => x.LocationCode == "WAH HCMC");
        Assert.Equal("WAH HCMC", wahHcmcLocation.LocationName);

        var noneLocation = result.First(x => x.LocationCode == "None");
        Assert.Equal("None", noneLocation.LocationName);
    }

    [Fact]
    public async Task GetAllReferenceResource_ShouldCallRepositoryWithCorrectCriteria()
    {
        // Arrange
        var expectedResources = new List<ReferenceResource>
        {
            new() { ResourceId = 1, Name = "Test1" },
            new() { ResourceId = 2, Name = "Test2" }
        };

        dynamicRepository.FetchAsync<ReferenceResource>(Arg.Any<GetAllRefResourceCriteria>())
            .Returns(expectedResources);

        // Act
        var result = await refResourceService.GetAllReferenceResource();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(expectedResources, result);

        await dynamicRepository.Received(1).FetchAsync<ReferenceResource>(Arg.Any<GetAllRefResourceCriteria>());
    }

    [Fact]
    public async Task GetRefResourcesByName_ShouldCallRepositoryWithCorrectCriteria()
    {
        // Arrange
        var refName = "TestRef";
        var expectedResources = new List<ReferenceResource>
        {
            new() { ResourceId = 1, Name = refName }
        };

        dynamicRepository.FetchAsync<ReferenceResource>(Arg.Any<GetRefResourceByNameCriteria>())
            .Returns(expectedResources);

        // Act
        var result = await refResourceService.GetRefResourcesByName(refName);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(expectedResources, result);

        await dynamicRepository.Received(1).FetchAsync<ReferenceResource>(
            Arg.Is<GetRefResourceByNameCriteria>(x => x.RefName == refName));
    }

    [Fact]
    public async Task GetRefResourcesByName_WithEmptyName_ShouldCallRepositoryWithEmptyName()
    {
        // Arrange
        var refName = string.Empty;
        var expectedResources = new List<ReferenceResource>();

        dynamicRepository.FetchAsync<ReferenceResource>(Arg.Any<GetRefResourceByNameCriteria>())
            .Returns(expectedResources);

        // Act
        var result = await refResourceService.GetRefResourcesByName(refName);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        await dynamicRepository.Received(1).FetchAsync<ReferenceResource>(
            Arg.Is<GetRefResourceByNameCriteria>(x => x.RefName == refName));
    }

    [Fact]
    public async Task GetRefResourcesByName_WithNullName_ShouldCallRepositoryWithNullName()
    {
        // Arrange
        string refName = null;
        var expectedResources = new List<ReferenceResource>();

        dynamicRepository.FetchAsync<ReferenceResource>(Arg.Any<GetRefResourceByNameCriteria>())
            .Returns(expectedResources);

        // Act
        var result = await refResourceService.GetRefResourcesByName(refName);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        await dynamicRepository.Received(1).FetchAsync<ReferenceResource>(
            Arg.Is<GetRefResourceByNameCriteria>(x => x.RefName == refName));
    }
}
