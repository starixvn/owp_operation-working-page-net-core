

using Fanex.NAP.Client.Helpers.Web;
using OWP.NewLeagueSetting.Helper;

namespace OWP.NewLeagueSetting.Command;

public class UpdateLeagueSettingRequest :LeagueSettingCore, IRequest<Unit>
{
    public int LeagueSettingId { get; set; }

    public int LeagueGroupId { get; set; }

    public int OWPLeagueId { get; set; }

    public string LeagueGroup { get; set; }

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
}

public class UpdateLeagueSettingHandler: IRequestHandler<UpdateLeagueSettingRequest, Unit>
{
    private readonly Repository repository;

    private readonly INapSession session;

    public UpdateLeagueSettingHandler(Repository repository, INapSession session)
    {
        this.repository = repository;
        this.session = session;
    }
    public async Task<Unit> Handle(UpdateLeagueSettingRequest request, CancellationToken cancellationToken)
    { 
        StandardizedLeagueInfo.BuildMaxbet(request, request.MaxbetWD_HDP, request.MaxbetWK_HDP);
        StandardizedLeagueInfo.BuildNoHdp(request,request.WD_NoOfHDP, request.WK_NoOfHDP );
        StandardizedLeagueInfo.BuildSpread(request,request.Spread);
        await repository.UpdateLeagueSetting(request, session.UserId);
       return Unit.Value;
    }
}
