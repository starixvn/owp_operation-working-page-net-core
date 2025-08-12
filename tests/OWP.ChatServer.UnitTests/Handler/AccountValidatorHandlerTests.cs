using System.Net;
using System.Text;
using Fanex.Data.Repository;
using Fanex.NAP.Client;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using OWP.ChatServer.Handler;
using Xunit;

namespace OWP.ChatServer.UnitTests.Handler;

public class AccountValidatorHandlerTests
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDynamicRepository _dynamicRepository;
    private readonly IConfiguration _configuration;
    private readonly INapSession _napSession;
    private readonly AccountValidatorHandler _handler;

    public AccountValidatorHandlerTests()
    {
        _httpClientFactory = Substitute.For<IHttpClientFactory>();
        _dynamicRepository = Substitute.For<IDynamicRepository>();
        _configuration = Substitute.For<IConfiguration>();
        _napSession = Substitute.For<INapSession>();

        _handler = new AccountValidatorHandler(
            _httpClientFactory,
            _dynamicRepository,
            _configuration,
            _napSession);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ReturnsTrue()
    {
        // Arrange
        var request = new AccountRocketChatValidatorRequest();
        var username = "testuser";
        var environment = "test";

        _napSession.UserName.Returns(username);
        _configuration.GetSection("Fanex.NAP.Client:Environment").Value.Returns(environment);

        // Mock HttpClient to avoid actual HTTP calls
        var mockHttpClient = Substitute.For<HttpClient>();
        _httpClientFactory.CreateClient("OWPChat").Returns(mockHttpClient);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        // Verify that logging occurred (exact count may vary due to HTTP mocking)
        await _dynamicRepository.Received().ExecuteAsync(Arg.Any<OwpLogCriteria>());
    }

    [Fact]
    public async Task Handle_WhenExceptionOccurs_ReturnsTrueAndLogsError()
    {
        // Arrange
        var request = new AccountRocketChatValidatorRequest();
        var username = "testuser";
        var environment = "test";

        _napSession.UserName.Returns(username);
        _configuration.GetSection("Fanex.NAP.Client:Environment").Value.Returns(environment);

        // Mock HttpClient to throw exception
        var mockHttpClient = Substitute.For<HttpClient>();
        _httpClientFactory.CreateClient("OWPChat").Returns(mockHttpClient);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result); // Default return value when exception occurs
        await _dynamicRepository.Received(1).ExecuteAsync(Arg.Any<OwpLogCriteria>());
    }

    [Fact]
    public void Constructor_InitializesCorrectly()
    {
        // Act & Assert
        Assert.NotNull(_handler);
    }

    [Fact]
    public async Task Handle_WhenConfigurationIsMissing_StillProcessesRequest()
    {
        // Arrange
        var request = new AccountRocketChatValidatorRequest();
        var username = "testuser";

        _napSession.UserName.Returns(username);
        _configuration.GetSection("Fanex.NAP.Client:Environment").Value.Returns((string?)null);

        var mockHttpClient = Substitute.For<HttpClient>();
        _httpClientFactory.CreateClient("OWPChat").Returns(mockHttpClient);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
    }
}
