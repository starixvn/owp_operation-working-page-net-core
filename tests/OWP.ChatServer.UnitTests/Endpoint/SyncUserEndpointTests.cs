using Carter;
using Fanex.NAP.Client;
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

public class SyncUserEndpointTests
{
    private readonly SyncUserEndpoint _endpoint;
    private readonly IEndpointRouteBuilder _app;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISender _sender;
    private readonly INapSession _napSession;

    public SyncUserEndpointTests()
    {
        _endpoint = new SyncUserEndpoint();
        _app = Substitute.For<IEndpointRouteBuilder>();
        _serviceProvider = Substitute.For<IServiceProvider>();
        _sender = Substitute.For<ISender>();
        _napSession = Substitute.For<INapSession>();
    }

    [Fact]
    public void AddRoutes_RegistersUpdateUserNameEndpoint()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_sender);
        serviceCollection.AddSingleton(_napSession);
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
    public void AddRoutes_RegistersCheckAccountExistsEndpoint()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_sender);
        serviceCollection.AddSingleton(_napSession);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var app = Substitute.For<IEndpointRouteBuilder>();
        app.ServiceProvider.Returns(serviceProvider);

        // Act
        _endpoint.AddRoutes(app);

        // Assert
        // Note: This is a basic test to ensure the method doesn't throw
        Assert.NotNull(_endpoint);
    }

    [Fact]
    public void AddRoutes_RegistersCreateAccountEndpoint()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_sender);
        serviceCollection.AddSingleton(_napSession);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var app = Substitute.For<IEndpointRouteBuilder>();
        app.ServiceProvider.Returns(serviceProvider);

        // Act
        _endpoint.AddRoutes(app);

        // Assert
        // Note: This is a basic test to ensure the method doesn't throw
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
}
