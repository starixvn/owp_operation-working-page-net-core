using MediatR;
using OWP.Helpers;
using OWP.Maxbet.Builder;
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

    private const decimal SPREAD_HDP_PEN = (decimal)0.08;

    private const decimal SPREAD_OU_PEN = (decimal)0.1;

    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly TraderAssignmentBuilder traderAssignmentBuilder;

    private readonly SpreadBuilder spreadBuilder;

    private readonly MaxbetBuilder maxbetBuilder;

    private readonly IMediator mediator;

    public MainMatchServiceImp(MaxbetRepository syncMaxbetRepository,
    TraderAssignmentBuilder traderAssignmentBuilder,
        SpreadBuilder spreadBuilder,
        MaxbetBuilder maxbetBuilder,
        IMediator mediator)
    {
      this.syncMaxbetRepository = syncMaxbetRepository;
      this.traderAssignmentBuilder = traderAssignmentBuilder;
      this.spreadBuilder = spreadBuilder;
      this.maxbetBuilder = maxbetBuilder;
      this.mediator = mediator;
    }
    public async Task<int> GetMatchIdByAdminSiteId(int adminsiteId)
            => await syncMaxbetRepository.GetMatchIdByAdminSiteId(adminsiteId);

    public async Task<MaxbetInfo> GetMaxbetInfo(int owpMatchId, int adminsiteId)
    {
      var maxbetInfo = await BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

      var numberOfLines = await mediator.Send(new GetNumberOfLinesHandler.Request { OWPMatchId = owpMatchId });

      maxbetInfo.NumberOfLines = numberOfLines;
      return maxbetInfo;
    }

    public async Task<MaxbetInfo> GetMaxbetInfoET(int owpMatchId, int adminsiteId)
    {
      var maxbetInfo = await BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

      MaxbetHelper.SetDefaultMaxbetInfo(maxbetInfo);

      MaxbetHelper.UpdateMaxbetET(maxbetInfo.Maxbet);

      maxbetInfo.Ft1stLineHandicap = Ft1stLineHandicapHelper.BuildFt1stLineHandicap();
      return maxbetInfo;
    }

    public async Task<MaxbetInfo> GetMaxbetInfoPEN(int owpMatchId, int adminsiteId)
    {
      var maxbetInfo = await BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

      MaxbetHelper.SetDefaultMaxbetInfo(maxbetInfo);

      MaxbetHelper.UpdateMaxbetPEN(maxbetInfo.Maxbet);

      MaxbetHelper.UpdateSpreadPEN(maxbetInfo.Spread, SPREAD_HDP_PEN, SPREAD_OU_PEN);
      return maxbetInfo;
    }

    public async Task<MaxbetInfo> GetMaxBetInfoFutsalET(int owpMatchId, int adminsiteId)
    {
      var maxbetInfo = await BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

      MaxbetHelper.SetDefaultMaxbetInfo(maxbetInfo);

      maxbetInfo.Ft1stLineHandicap = Ft1stLineHandicapHelper.BuildFt1stLineHandicap();
      return maxbetInfo;
    }

    public async Task<MaxbetInfo> GetMaxBetInfoFutsalPEN(int owpMatchId, int adminsiteId)
    {
      var maxbetInfo = await BuildMatchInfoExcludeNumberOfLines(owpMatchId, adminsiteId);

      NumberOfLineHelper.UpdateNumberOfLine(maxbetInfo.NumberOfLines);

      MaxbetHelper.UpdateMaxbetPEN(maxbetInfo.Maxbet);

      MaxbetHelper.UpdateSpreadPEN(maxbetInfo.Spread, MaxBetSetting.Spread_Futsal_PEN, MaxBetSetting.Spread_Futsal_PEN);
      return maxbetInfo;
    }

    private async Task<MaxbetInfo> BuildMatchInfoExcludeNumberOfLines(int owpMatchId, int adminsiteId)
    {
      var traderAssignmentsSender = await traderAssignmentBuilder.BuildTraderAssignmentSender(owpMatchId);

      var spread = await spreadBuilder.BuildSpreadMainMatch(owpMatchId);

      var maxbet = await maxbetBuilder.BuildMaxbet(owpMatchId);

      var cn88Maxbet = new MaxbetSpread
      {
        FirstHalfHandicap = 0,
        FirstHalfOverUnder = 0,
        Handicap = 0,
        OverUnder = 0
      };

      var secondAcceptValue = await mediator.Send(new GetSecondAcceptValueHandler.Request { OWPMatchId = owpMatchId });

      return new MaxbetInfo
      {
        AdminsiteID = adminsiteId,
        Traders = traderAssignmentsSender,
        Spread = spread,
        Maxbet = maxbet,
        MaxbetCN88 = cn88Maxbet,
        SecondAcceptValue = secondAcceptValue
      };
    }
  }
}