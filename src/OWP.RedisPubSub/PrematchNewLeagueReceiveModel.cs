namespace OWP.RedisPubSub;
public class PrematchNewLeagueReceiveModel
{
    public string? SessionId { get; set; }

    public DateTime FromDate { get; set; }

    public string? RoutingKey { get;  set; }
}
