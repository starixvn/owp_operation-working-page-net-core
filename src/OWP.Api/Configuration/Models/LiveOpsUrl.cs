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
    public string Saba { get; set; }
}
