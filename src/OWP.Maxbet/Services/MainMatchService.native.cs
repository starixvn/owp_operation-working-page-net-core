using MediatR;
using OWP.Maxbet.Handlers;
using OWP.Maxbet.Helpers;
using OWP.Maxbet.Models;
using OWP.Maxbet.Constants;

namespace OWP.Maxbet.Services
{
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

    private readonly IMediator mediator;

    private readonly ServiceCommon serviceCommon;

    public MainMatchServiceImp(MaxbetRepository syncMaxbetRepository,
        IMediator mediator,
        ServiceCommon serviceCommon)
    {
      this.syncMaxbetRepository = syncMaxbetRepository;
      this.mediator = mediator;
      this.serviceCommon = serviceCommon;
    }
    public async Task<int> GetMatchIdByAdminSiteId(int adminsiteId)
            => await syncMaxbetRepository.GetMatchIdByAdminSiteId(adminsiteId);

    public async Task<MaxbetInfo> GetMaxbetInfo(int owpMatchId, int adminsiteId)
    {
      var maxbetInfo = await serviceCommon.BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

      var numberOfLines = await mediator.Send(new GetNumberOfLinesHandler.Request { OWPMatchId = owpMatchId });

      maxbetInfo.NumberOfLines = numberOfLines;
      return maxbetInfo;
    }

    public async Task<MaxbetInfo> GetMaxbetInfoET(int owpMatchId, int adminsiteId)
    {
      var maxbetInfo = await serviceCommon.BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

      UpdateMaxbetET(maxbetInfo.Maxbet);

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
      if (maxbet.Handicap == MaxBetSetting.MAXBET_HDP)
      {
        maxbet.Handicap = MaxBetSetting.MAXBET_ET;
        maxbet.OverUnder = MaxBetSetting.MAXBET_ET;
      }
    }


  }
}