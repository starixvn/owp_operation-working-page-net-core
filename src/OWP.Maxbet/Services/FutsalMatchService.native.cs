using OWP.Maxbet.Constants;
using OWP.Maxbet.Helpers;
using OWP.Maxbet.Models;

namespace OWP.Maxbet.Services;

public interface FutsalMatchService
{
    public Task<MaxbetInfo> GetMaxBetInfoFutsalET(int owpMatchId, int adminsiteId);

    public Task<MaxbetInfo> GetMaxBetInfoFutsalPEN(int owpMatchId, int adminsiteId);
}

public class FutsalMatchServiceImp : FutsalMatchService
{
    private readonly ServiceCommon serviceCommon;

    public FutsalMatchServiceImp(ServiceCommon serviceCommon)
    {
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