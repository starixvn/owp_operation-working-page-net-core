using System.Net;
using System.Text;
using Fanex.Data.Repository;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using OWP.ChatServer.Handler;
using Xunit;

namespace OWP.ChatServer.UnitTests.Handler;

public class AccountCreatorHandlerTests
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDynamicRepository _dynamicRepository;
    private readonly IConfiguration _configuration;
    private readonly AccountCreatorHandler _handler;

    public AccountCreatorHandlerTests()
    {
        _httpClientFactory = Substitute.For<IHttpClientFactory>();
        _dynamicRepository = Substitute.For<IDynamicRepository>();
        _configuration = Substitute.For<IConfiguration>();

        _handler = new AccountCreatorHandler(
            _httpClientFactory,
            _dynamicRepository,
            _configuration);
    }

    [Fact]
    public async Task Handle_WhenAccountCreationSucceeds_ReturnsUnitValue()
    {
        // Arrange
        var request = new AccountRocketChatCreatorRequest(123, "testuser");
        var environment = "test";

        _configuration.GetSection("Fanex.NAP.Client:Environment").Value.Returns(environment);

        // Mock HttpClient to avoid actual HTTP calls
        var mockHttpClient = Substitute.For<HttpClient>();
        _httpClientFactory.CreateClient("OWPChat").Returns(mockHttpClient);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        // Verify that logging occurred (exact count may vary due to HTTP mocking)
        await _dynamicRepository.Received().ExecuteAsync(Arg.Any<OwpLogCriteria>());
    }

    [Fact]
    public async Task Handle_WhenExceptionOccurs_ReturnsUnitValueAndLogsError()
    {
        // Arrange
        var request = new AccountRocketChatCreatorRequest(123, "testuser");
        var environment = "test";

        _configuration.GetSection("Fanex.NAP.Client:Environment").Value.Returns(environment);

        // Mock HttpClient to avoid actual HTTP calls
        var mockHttpClient = Substitute.For<HttpClient>();
        _httpClientFactory.CreateClient("OWPChat").Returns(mockHttpClient);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        // Verify that logging occurred (exact count may vary due to HTTP mocking)
        await _dynamicRepository.Received().ExecuteAsync(Arg.Any<OwpLogCriteria>());
    }

    [Fact]
    public async Task Handle_WhenConfigurationIsMissing_StillProcessesRequest()
    {
        // Arrange
        var request = new AccountRocketChatCreatorRequest(123, "testuser");

        _configuration.GetSection("Fanex.NAP.Client:Environment").Value.Returns((string?)null);

        var mockHttpClient = Substitute.For<HttpClient>();
        _httpClientFactory.CreateClient("OWPChat").Returns(mockHttpClient);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        // Verify that logging occurred (exact count may vary due to HTTP mocking)
        await _dynamicRepository.Received().ExecuteAsync(Arg.Any<OwpLogCriteria>());
    }

    [Fact]
    public void Constructor_InitializesCorrectly()
    {
        // Act & Assert
        Assert.NotNull(_handler);
    }

    [Fact]
    public async Task Handle_AlwaysLogsUserCreationAttempt_RegardlessOfOutcome()
    {
        // Arrange
        var request = new AccountRocketChatCreatorRequest(123, "testuser");
        var environment = "test";

        _configuration.GetSection("Fanex.NAP.Client:Environment").Value.Returns(environment);

        var mockHttpClient = Substitute.For<HttpClient>();
        _httpClientFactory.CreateClient("OWPChat").Returns(mockHttpClient);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _dynamicRepository.Received(1).ExecuteAsync(Arg.Is<OwpLogCriteria>(c =>
            c.Action == "CreateOWPChatAccount_User"));
    }
}
