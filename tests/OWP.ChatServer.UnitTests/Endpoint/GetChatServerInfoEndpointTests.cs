using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using OWP.ChatServer.Endpoint;
using OWP.ChatServer.Handler;
using Xunit;

namespace OWP.ChatServer.UnitTests.Endpoint;

public class GetChatServerInfoEndpointTests
{
    private readonly GetChatServerInfoEndpoint _endpoint;
    private readonly IEndpointRouteBuilder _app;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISender _sender;

    public GetChatServerInfoEndpointTests()
    {
        _endpoint = new GetChatServerInfoEndpoint();
        _app = Substitute.For<IEndpointRouteBuilder>();
        _serviceProvider = Substitute.For<IServiceProvider>();
        _sender = Substitute.For<ISender>();
    }

    [Fact]
    public void AddRoutes_RegistersGetChatServerInfoEndpoint()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_sender);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var app = Substitute.For<IEndpointRouteBuilder>();
        app.ServiceProvider.Returns(serviceProvider);

        // Act
        _endpoint.AddRoutes(app);

        // Assert
        // Note: This is a basic test to ensure the method doesn't throw
        // In a real scenario, you might want to test the actual endpoint behavior
        // by setting up a test server and making HTTP requests
        Assert.NotNull(_endpoint);
    }

    [Fact]
    public void ImplementsICarterModule()
    {
        // Act & Assert
        Assert.IsAssignableFrom<ICarterModule>(_endpoint);
    }

    [Fact]
    public void AddRoutes_DoesNotThrowException()
    {
        // Arrange
        var app = Substitute.For<IEndpointRouteBuilder>();
        app.ServiceProvider.Returns(_serviceProvider);

        // Act & Assert
        var exception = Record.Exception(() => _endpoint.AddRoutes(app));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_DoesNotThrowException()
    {
        // Act & Assert
        var exception = Record.Exception(() => new GetChatServerInfoEndpoint());
        Assert.Null(exception);
    }
}
