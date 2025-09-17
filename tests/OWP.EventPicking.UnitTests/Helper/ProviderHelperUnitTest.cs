using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace OWP.EventPicking.UnitTests.Helper;

public class ProviderHelperUnitTest
{
    [Fact]
    public void BuildProviderByOtherProvider_ShouldReturnCorrectProvider()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        var otherProviderInString = "ProviderA/ProviderC";

        // Act
        var result = ProviderHelper.BuildProviderByOtherProvider(priorityProviders, otherProviderInString);

        // Assert
        Assert.Equal("ProviderA", result);
    }

    [Fact]
    public void BuildProviderByOtherProvider_ShouldReturnTVProvider_WhenNoPriorityProviderFound()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        var otherProviderInString = "ProviderC/ProviderATV";

        // Act
        var result = ProviderHelper.BuildProviderByOtherProvider(priorityProviders, otherProviderInString);

        // Assert
        Assert.Equal("ProviderATV", result);
    }

    [Fact]
    public void BuildProviderByOtherProvider_ShouldReturnEmptyString_WhenNoProviderFound()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        var otherProviderInString = "ProviderC/ProviderD";

        // Act
        var result = ProviderHelper.BuildProviderByOtherProvider(priorityProviders, otherProviderInString);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void BuildProviderByOtherProvider_ShouldHandleNullOtherProviderInString()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        string otherProviderInString = null!;

        // Act
        var result = ProviderHelper.BuildProviderByOtherProvider(priorityProviders, otherProviderInString);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void BuildProviderByOtherProvider_ShouldHandleEmptyOtherProviderInString()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        var otherProviderInString = string.Empty;

        // Act
        var result = ProviderHelper.BuildProviderByOtherProvider(priorityProviders, otherProviderInString);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void BuildProviderByOtherProvider_ShouldHandleNullPriorityProviders()
    {
        // Arrange
        List<string> priorityProviders = null!;
        var otherProviderInString = "ProviderA/ProviderB";

        // Act & Assert
        var exception = Record.Exception(() => ProviderHelper.BuildProviderByOtherProvider(priorityProviders, otherProviderInString));
        Assert.NotNull(exception);
    }

    [Fact]
    public void BuildProviderTv_ShouldReturnTVProvider()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        var otherProviders = new List<string> { "ProviderA", "ProviderBTV", "ProviderC" };

        // Act
        var result = ProviderHelper.BuildProviderTv(priorityProviders, otherProviders);

        // Assert
        Assert.Equal("ProviderBTV", result);
    }

    [Fact]
    public void BuildProviderTv_ShouldReturnEmptyString_WhenNoTVProviderFound()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        var otherProviders = new List<string> { "ProviderA", "ProviderB", "ProviderC" };

        // Act
        var result = ProviderHelper.BuildProviderTv(priorityProviders, otherProviders);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void BuildProviderTv_ShouldHandleNullOtherProviders()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        List<string> otherProviders = null!;

        // Act
        var result = ProviderHelper.BuildProviderTv(priorityProviders, otherProviders);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void AddTVBeforeProvider_ShouldReturnTV_WhenProviderIsEmptyAndHasTV()
    {
        // Arrange
        var match = new EventPickingModel
        {
            Provider = string.Empty,
            TV = "Channel1"
        };

        // Act
        var result = ProviderHelper.AddTVBeforeProvider(match);

        // Assert
        Assert.Equal("TV", result);
    }

    [Fact]
    public void AddTVBeforeProvider_ShouldReturnTVSlashProvider_WhenProviderIsNotEmptyAndHasTV()
    {
        // Arrange
        var match = new EventPickingModel
        {
            Provider = "ProviderA",
            TV = "Channel1"
        };

        // Act
        var result = ProviderHelper.AddTVBeforeProvider(match);

        // Assert
        Assert.Equal("TV/ProviderA", result);
    }

    [Fact]
    public void AddTVBeforeProvider_ShouldReturnProvider_WhenNoTV()
    {
        // Arrange
        var match = new EventPickingModel
        {
            Provider = "ProviderA",
            TV = string.Empty,
            Channel = string.Empty,
            Iptv = string.Empty
        };

        // Act
        var result = ProviderHelper.AddTVBeforeProvider(match);

        // Assert
        Assert.Equal("ProviderA", result);
    }

    [Fact]
    public void AddTVBeforeProvider_ShouldReturnTV_WhenHasChannel()
    {
        // Arrange
        var match = new EventPickingModel
        {
            Provider = string.Empty,
            Channel = "Channel1"
        };

        // Act
        var result = ProviderHelper.AddTVBeforeProvider(match);

        // Assert
        Assert.Equal("TV", result);
    }

    [Fact]
    public void AddTVBeforeProvider_ShouldReturnTV_WhenHasIptv()
    {
        // Arrange
        var match = new EventPickingModel
        {
            Provider = string.Empty,
            Iptv = "IPTV1"
        };

        // Act
        var result = ProviderHelper.AddTVBeforeProvider(match);

        // Assert
        Assert.Equal("TV", result);
    }

    [Fact]
    public void SortOtherProvider_ShouldSortProvidersByPriority()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB", "ProviderC" };
        var otherProviderInString = "ProviderC/ProviderA/ProviderB";

        // Act
        var result = ProviderHelper.SortOtherProvider(priorityProviders, otherProviderInString);

        // Assert
        Assert.Equal("ProviderA/ProviderB/ProviderC", result);
    }

    [Fact]
    public void SortOtherProvider_ShouldHandleTVProviders()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        var otherProviderInString = "ProviderBTV/ProviderA/ProviderCTV";

        // Act
        var result = ProviderHelper.SortOtherProvider(priorityProviders, otherProviderInString);

        // Assert
        Assert.Equal("ProviderA/ProviderCTV/ProviderBTV", result);
    }

    [Fact]
    public void SortOtherProvider_ShouldReturnEmptyString_WhenOtherProviderInStringIsEmpty()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        var otherProviderInString = string.Empty;

        // Act
        var result = ProviderHelper.SortOtherProvider(priorityProviders, otherProviderInString);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void SortOtherProvider_ShouldReturnEmptyString_WhenOtherProviderInStringIsNull()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        string otherProviderInString = null!;

        // Act
        var result = ProviderHelper.SortOtherProvider(priorityProviders, otherProviderInString);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void SortOtherProvider_ShouldHandleSingleProvider()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        var otherProviderInString = "ProviderA";

        // Act
        var result = ProviderHelper.SortOtherProvider(priorityProviders, otherProviderInString);

        // Assert
        Assert.Equal("ProviderA", result);
    }

    [Fact]
    public void SortOtherProvider_ShouldHandleProvidersNotInPriorityList()
    {
        // Arrange
        var priorityProviders = new List<string> { "ProviderA", "ProviderB" };
        var otherProviderInString = "ProviderC/ProviderD/ProviderA";

        // Act
        var result = ProviderHelper.SortOtherProvider(priorityProviders, otherProviderInString);

        // Assert
        Assert.Equal("ProviderA/ProviderC/ProviderD", result);
    }
}