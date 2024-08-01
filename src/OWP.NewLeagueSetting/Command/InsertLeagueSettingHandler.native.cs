



using AutoMapper;

namespace OWP.NewLeagueSetting.Command;

public class InsertLeagueSettingRequest :LeagueSettingCore, Command<int>
{
    public string LeagueName { get; set; }

    public int OWPLeagueId { get; set; }

    public string Spread{ get; set; }

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

    public string FullConditionDescription => JsonConvert.SerializeObject(new LeagueSettingCondition
        {
           HasTv = HasTv,
           HasIpTv = HasIpTv,
           IpTv = IpTv,
           Round = Round,
           Division = Division,
           Remark = Remark
        });

    public int? LeagueGroupId { get; set; }
}

public class InsertLeagueSettingHandler(Repository repository, INapSession session, IMapper mapper) : CommandHandler<InsertLeagueSettingRequest, int>
{
    public async Task<int> Handle(InsertLeagueSettingRequest request, CancellationToken cancellationToken)
    { 
        StandardizedLeagueInfo.BuildMaxbet(request, request.MaxbetWD_HDP, request.MaxbetWK_HDP);
        StandardizedLeagueInfo.BuildNoHdp(request,request.WD_NoOfHDP, request.WK_NoOfHDP );
        StandardizedLeagueInfo.BuildSpread(request,request.Spread);
        var criteria = mapper.Map<InsertLeagueSettingCriteria>(request);
        await repository.InsertLeagueSetting(criteria, session.UserId);
       return criteria.LeagueSettingId;
    }
}
