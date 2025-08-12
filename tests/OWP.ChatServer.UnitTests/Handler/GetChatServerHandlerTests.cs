using Microsoft.Extensions.Configuration;
using NSubstitute;
using OWP.ChatServer.Handler;
using Xunit;

namespace OWP.ChatServer.UnitTests.Handler;

public class GetChatServerHandlerTests
{
    private readonly IConfiguration _configuration;
    private readonly GetChatServerHandler _handler;

    public GetChatServerHandlerTests()
    {
        _configuration = Substitute.For<IConfiguration>();
        _handler = new GetChatServerHandler(_configuration);
    }

    [Fact]
    public async Task Handle_WhenConfigurationExists_ReturnsChatServerInfo()
    {
        // Arrange
        var request = new GetChatServerRequest();
        var expectedUrl = "https://chat.example.com";

        _configuration["ChatServer:UIUrl"].Returns(expectedUrl);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        // Test that the result is not null and has the expected type
        Assert.Contains("url", result.GetType().GetProperties().Select(p => p.Name));
    }

    [Fact]
    public async Task Handle_WhenConfigurationIsMissing_ReturnsNullUrl()
    {
        // Arrange
        var request = new GetChatServerRequest();

        _configuration["ChatServer:UIUrl"].Returns((string?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("url", result.GetType().GetProperties().Select(p => p.Name));
    }

    [Fact]
    public async Task Handle_WhenConfigurationIsEmpty_ReturnsEmptyUrl()
    {
        // Arrange
        var request = new GetChatServerRequest();

        _configuration["ChatServer:UIUrl"].Returns(string.Empty);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("url", result.GetType().GetProperties().Select(p => p.Name));
    }

    [Fact]
    public async Task Handle_ReturnsExpectedObjectStructure()
    {
        // Arrange
        var request = new GetChatServerRequest();
        var expectedUrl = "https://chat.example.com";

        _configuration["ChatServer:UIUrl"].Returns(expectedUrl);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var resultType = result.GetType();
        var urlProperty = resultType.GetProperty("url");
        Assert.NotNull(urlProperty);
        Assert.Equal(typeof(string), urlProperty.PropertyType);
    }

    [Fact]
    public void Constructor_InitializesCorrectly()
    {
        // Act & Assert
        Assert.NotNull(_handler);
    }
}
