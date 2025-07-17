namespace OWP.Api.Configuration.Models;

[Serializable]
public class LiveOpsUrl
{
    public OsUrls Os { get; set; }
    public SoccerUrls Soccer { get; set; }
}


[Serializable]
public class OsUrls
{
    public string General { get; set; }
    public string Badminton { get; set; }
    public string Cricket { get; set; }
    public string Basketball { get; set; }
    public string Tennis { get; set; }
}

[Serializable]
public class SoccerUrls
{
    public string LaTradingPage { get; set; }

    public string TradingPage { get; set; }

    public List<ServerInfo> Servers { get; set; } = new List<ServerInfo>();
}

[Serializable]
public class ServerInfo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public int LAPort { get; set; }
    public int? TradingPort { get; set; }
}
