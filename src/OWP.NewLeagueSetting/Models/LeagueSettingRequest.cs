namespace OWP.NewLeagueSetting.Models;

public class LeagueSettingRequest : LeagueSettingCore
{
    public int LeagueSettingId { get; set; }

    public int LeagueGroupId { get; set; }

    public string League { get; set; }

    public int OWPLeagueId { get; set; }

    public string LeagueGroup { get; set; }

    public string Spread { get; set; }

    public string MaxbetWD_HDP { get; set; }

    public string WD_NoOfHDP { get; set; }

    public string MaxbetWK_HDP { get; set; }

    public string WK_NoOfHDP { get; set; }

    public string LstNote { get; set; }

    public string Remark2 { get; set; }

    public string Comment { get; set; }

    public bool HasTv { get; set; }

    public bool HasIpTv { get; set; }

    public List<Condition> IpTv { get; set; }

    public List<Condition> Round { get; set; }

    public List<Condition> Division { get; set; }

    public string Remark { get; set; }

    public bool IsBlackList { get; set; }

    public int NoOfferLive { get; set; }

    public string FullConditionDescription => JsonConvert.SerializeObject(new LeagueSettingCondition
    {
        HasTv = HasTv,
        HasIpTv = HasIpTv,
        IpTv = IpTv,
        Round = Round,
        Division = Division,
        Remark = Remark
    });
}
