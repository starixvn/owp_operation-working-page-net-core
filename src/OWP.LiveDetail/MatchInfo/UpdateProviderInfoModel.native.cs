namespace OWP.LiveDetail.MatchInfo;

public class UpdateProviderInfoModel
{
    public int OWPMatchId { get; set; }

    public string TVIds { get; set; }

    public bool IsUpdateTVs { get; set; }

    public string Channel { get; set; }

    public bool IsUpdateChannel { get; set; }

    public string IpTv { get; set; }

    public bool IsUpdateIpTv => IsUpdateChannel;

    public string WB { get; set; }

    public bool IsUpdateWB { get; set; }

    public string Provider { get; set; }

    public bool IsUpdateProviders { get; set; }

    public string OtherProvider { get; set; }

    public bool IsUpdateOtherProviders { get; set; }

    public string AgentIds { get; set; }

    public bool IsUpdateAgents { get; set; }

    public string Phone { get; set; }

    public bool IsUpdatePhone { get; set; }
}