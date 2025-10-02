using OWP.Api.Configuration.Models;

namespace OWP.Api.UnitTests.Models;

public class ConfigurationModelsUnitTests
{
    [Fact]
    public void LiveOpsUrl_DefaultConstructor_InitializesProperties()
    {
        // Act
        var liveOpsUrl = new LiveOpsUrl();

        // Assert
        Assert.Null(liveOpsUrl.Os);
        Assert.Null(liveOpsUrl.Soccer);
    }

    [Fact]
    public void LiveOpsUrl_WithValues_SetsPropertiesCorrectly()
    {
        // Arrange
        var osUrls = new OsUrls();
        var soccerUrls = new SoccerUrls();

        // Act
        var liveOpsUrl = new LiveOpsUrl
        {
            Os = osUrls,
            Soccer = soccerUrls
        };

        // Assert
        Assert.Equal(osUrls, liveOpsUrl.Os);
        Assert.Equal(soccerUrls, liveOpsUrl.Soccer);
    }

    [Fact]
    public void OsUrls_DefaultConstructor_InitializesProperties()
    {
        // Act
        var osUrls = new OsUrls();

        // Assert
        Assert.Null(osUrls.General);
        Assert.Null(osUrls.Badminton);
        Assert.Null(osUrls.Cricket);
        Assert.Null(osUrls.Basketball);
        Assert.Null(osUrls.Tennis);
        Assert.Null(osUrls.Baseball);
    }

    [Fact]
    public void OsUrls_WithValues_SetsPropertiesCorrectly()
    {
        // Arrange
        var general = "https://general.example.com";
        var badminton = "https://badminton.example.com";
        var cricket = "https://cricket.example.com";
        var basketball = "https://basketball.example.com";
        var tennis = "https://tennis.example.com";
        var baseball = "https://baseball.example.com";

        // Act
        var osUrls = new OsUrls
        {
            General = general,
            Badminton = badminton,
            Cricket = cricket,
            Basketball = basketball,
            Tennis = tennis,
            Baseball = baseball
        };

        // Assert
        Assert.Equal(general, osUrls.General);
        Assert.Equal(badminton, osUrls.Badminton);
        Assert.Equal(cricket, osUrls.Cricket);
        Assert.Equal(basketball, osUrls.Basketball);
        Assert.Equal(tennis, osUrls.Tennis);
        Assert.Equal(baseball, osUrls.Baseball);
    }

    [Fact]
    public void SoccerUrls_DefaultConstructor_InitializesProperties()
    {
        // Act
        var soccerUrls = new SoccerUrls();

        // Assert
        Assert.Null(soccerUrls.LaTradingPage);
        Assert.Null(soccerUrls.TradingPage);
        Assert.NotNull(soccerUrls.Servers);
        Assert.Empty(soccerUrls.Servers);
    }

    [Fact]
    public void SoccerUrls_WithValues_SetsPropertiesCorrectly()
    {
        // Arrange
        var laTradingPage = "https://la-trading.example.com";
        var tradingPage = "https://trading.example.com";
        var servers = new List<ServerInfo>
        {
            new() { Id = 1, Name = "Server1", Url = "https://server1.example.com", LAPort = 8080, TradingPort = 8081 },
            new() { Id = 2, Name = "Server2", Url = "https://server2.example.com", LAPort = 8082, TradingPort = 8083 }
        };

        // Act
        var soccerUrls = new SoccerUrls
        {
            LaTradingPage = laTradingPage,
            TradingPage = tradingPage,
            Servers = servers
        };

        // Assert
        Assert.Equal(laTradingPage, soccerUrls.LaTradingPage);
        Assert.Equal(tradingPage, soccerUrls.TradingPage);
        Assert.Equal(servers.Count, soccerUrls.Servers.Count);
        Assert.Equal(servers[0].Id, soccerUrls.Servers[0].Id);
        Assert.Equal(servers[1].Id, soccerUrls.Servers[1].Id);
    }

    [Fact]
    public void ServerInfo_DefaultConstructor_InitializesProperties()
    {
        // Act
        var serverInfo = new ServerInfo();

        // Assert
        Assert.Equal(0, serverInfo.Id);
        Assert.Null(serverInfo.Name);
        Assert.Null(serverInfo.Url);
        Assert.Equal(0, serverInfo.LAPort);
        Assert.Null(serverInfo.TradingPort);
    }

    [Fact]
    public void ServerInfo_WithValues_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = 123;
        var name = "Test Server";
        var url = "https://test.example.com";
        var laPort = 8080;
        var tradingPort = 8081;

        // Act
        var serverInfo = new ServerInfo
        {
            Id = id,
            Name = name,
            Url = url,
            LAPort = laPort,
            TradingPort = tradingPort
        };

        // Assert
        Assert.Equal(id, serverInfo.Id);
        Assert.Equal(name, serverInfo.Name);
        Assert.Equal(url, serverInfo.Url);
        Assert.Equal(laPort, serverInfo.LAPort);
        Assert.Equal(tradingPort, serverInfo.TradingPort);
    }

    [Fact]
    public void ServerInfo_WithNullTradingPort_SetsTradingPortToNull()
    {
        // Arrange
        var serverInfo = new ServerInfo
        {
            Id = 1,
            Name = "Test Server",
            Url = "https://test.example.com",
            LAPort = 8080,
            TradingPort = null
        };

        // Assert
        Assert.Null(serverInfo.TradingPort);
    }
}
