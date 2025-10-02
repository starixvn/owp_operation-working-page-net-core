namespace OWP.NewLeagueSetting.Models;

public class LeagueSettingChangeModel
{
    public string League { get; set; }
    public string LeagueGroup { get; set; }
    public bool NoOfferLive { get; set; }
    public bool IsBlackList { get; set; }
    public bool HasTv { get; set; }
    public bool HasIpTv { get; set; }
    public List<Condition> IpTv { get; set; }
    public List<Condition> Round { get; set; }
    public List<Condition> Division { get; set; }
    public string Remark { get; set; }
    public string Spread { get; set; }
    public string MaxbetWD_HDP { get; set; }
    public string WD_NoOfHDP { get; set; }
    public string MaxbetWK_HDP { get; set; }
    public string WK_NoOfHDP { get; set; }
    public string LstNote { get; set; }
    public string Remark2 { get; set; }
    
}
