using MediatR;
using OWP.Api.Maxbet.Constants;
using OWP.Api.Maxbet.Handlers;
using OWP.Api.Maxbet.Helpers;
using OWP.Api.Maxbet.Models;

namespace OWP.Api.Maxbet.Services;

public interface MainMatchService
{
    Task<int> GetMatchIdByAdminSiteId(int adminsiteId);

    Task<MaxbetInfo> GetMaxbetInfo(int owpMatchId, int adminsiteId);

    Task<MaxbetInfo> GetMaxbetInfoET(int owpMatchId, int adminsiteId);

    Task<MaxbetInfo> GetMaxbetInfoPEN(int owpMatchId, int adminsiteId);

}

public class MainMatchServiceImp : MainMatchService
{
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly OddService oddService;

    private readonly IMediator mediator;

    private readonly ServiceCommon serviceCommon;

    public MainMatchServiceImp(MaxbetRepository syncMaxbetRepository,
        IMediator mediator,
        ServiceCommon serviceCommon,
        OddService oddService)
    {
        this.syncMaxbetRepository = syncMaxbetRepository;
        this.mediator = mediator;
        this.serviceCommon = serviceCommon;
        this.oddService = oddService;
    }
    public async Task<int> GetMatchIdByAdminSiteId(int adminsiteId)
            => await syncMaxbetRepository.GetMatchIdByAdminSiteId(adminsiteId);

    public async Task<MaxbetInfo> GetMaxbetInfo(int owpMatchId, int adminsiteId)
    {
        var maxbetInfo = await serviceCommon.BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

        await oddService.BuildOdd(maxbetInfo, owpMatchId);

        var numberOfLines = await mediator.Send(new GetNumberOfLinesHandler.Request { OWPMatchId = owpMatchId });

        maxbetInfo.NumberOfLines = numberOfLines;
        return maxbetInfo;
    }

    public async Task<MaxbetInfo> GetMaxbetInfoET(int owpMatchId, int adminsiteId)
    {
        var maxbetInfo = await serviceCommon.BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

        UpdateMaxbetET(maxbetInfo.Maxbet);
        maxbetInfo.NumberOfLines = NumberOfLineHelper.UpdateNumberOfLine();
        maxbetInfo.Ft1stLineHandicap = Ft1stLineHandicapHelper.BuildFt1stLineHandicap();
        return maxbetInfo;
    }

    public async Task<MaxbetInfo> GetMaxbetInfoPEN(int owpMatchId, int adminsiteId)
    {
        var maxbetInfo = await serviceCommon.BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

        MaxbetHelper.SetDefaultMaxbetInfo(maxbetInfo);

        MaxbetHelper.UpdateMaxbetPEN(maxbetInfo.Maxbet);

        MaxbetHelper.UpdateSpreadPEN(maxbetInfo.Spread, MaxBetSetting.SPREAD_HDP_PEN, MaxBetSetting.SPREAD_OU_PEN);
        return maxbetInfo;
    }

    private static void UpdateMaxbetET(MaxbetSpread maxbet)
    {
        if (maxbet.Handicap != MaxBetSetting.MAXBET_HDP)
        {
            return;
        }

        maxbet.Handicap = MaxBetSetting.MAXBET_ET;
        maxbet.OverUnder = MaxBetSetting.MAXBET_ET;
    }


}