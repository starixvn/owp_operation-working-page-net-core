using MediatR;
using OWP.Maxbet.Handlers;
using OWP.Maxbet.Helpers;
using OWP.Maxbet.Models;
using OWP.Maxbet.Constants;

namespace OWP.Maxbet.Services
{
  public interface FutsalMatchService
  {
    public Task<MaxbetInfo> GetMaxBetInfoFutsalET(int owpMatchId, int adminsiteId);

    public Task<MaxbetInfo> GetMaxBetInfoFutsalPEN(int owpMatchId, int adminsiteId);

  }
  public class FutsalMatchServiceImp : FutsalMatchService
  {
    private readonly IMediator mediator;

    private readonly ServiceCommon serviceCommon;

    public FutsalMatchServiceImp(IMediator mediator, ServiceCommon serviceCommon)
    {
      this.syncMaxbetRepository = syncMaxbetRepository;
      this.mediator = mediator;
      this.serviceCommon = serviceCommon;
    }

    public async Task<MaxbetInfo> GetMaxBetInfoFutsalET(int owpMatchId, int adminsiteId)
    {
      var maxbetInfo = await serviceCommon.BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

      MaxbetHelper.SetDefaultMaxbetInfo(maxbetInfo);

      maxbetInfo.Ft1stLineHandicap = Ft1stLineHandicapHelper.BuildFt1stLineHandicap();
      return maxbetInfo;
    }

    public async Task<MaxbetInfo> GetMaxBetInfoFutsalPEN(int owpMatchId, int adminsiteId)
    {
      var maxbetInfo = await serviceCommon.BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

      MaxbetHelper.SetDefaultMaxbetInfo(maxbetInfo);

      MaxbetHelper.UpdateMaxbetPEN(maxbetInfo.Maxbet);

      MaxbetHelper.UpdateSpreadPEN(maxbetInfo.Spread, MaxBetSetting.SPREAD_FUTSAL_PEN, MaxBetSetting.SPREAD_FUTSAL_PEN);
      return maxbetInfo;
    }

  }
}