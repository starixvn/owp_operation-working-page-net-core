using MediatR;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Handlers;
using OWP.Maxbet.Models;
using OWP.Maxbet.Helpers;
using OWP.Maxbet.Constants;
using OWP.Services;

namespace OWP.Maxbet.Services
{
  public interface SabaMatchService
  {
    public Task<MaxbetInfo> GetMaxbetInfoSaba(int owpMatchId, int adminsiteId);
  }

  public class SabaMatchServiceImp : SabaMatchService
  {

    private readonly TraderAssignmentBuilder traderAssignmentBuilder;
    private readonly IMediator mediator;
    public SabaMatchServiceImp(
        TraderAssignmentBuilder traderAssignmentBuilder,
        IMediator mediator)
    {
      this.traderAssignmentBuilder = traderAssignmentBuilder;
      this.mediator = mediator;
    }
    public async Task<MaxbetInfo> GetMaxbetInfoSaba(int owpMatchId, int adminsiteId)
    {
      var traderAssignmentsSender = await traderAssignmentBuilder.BuildTraderAssignmentSender(owpMatchId);

      var secondAcceptValue = await mediator.Send(new GetSecondAcceptValueHandler.Request { OWPMatchId = owpMatchId });

      var spread = new MaxbetSpread
      {
        Handicap = MaxBetSetting.SPREAD_SABA,
        OverUnder = (decimal)MaxBetSetting.SPREAD_SABA
      };

      var maxbet = new MaxbetSpread
      {
        Handicap = MaxBetSetting.MAXBET_SABA,
        OverUnder = MaxBetSetting.MAXBET_SABA
      };

      var cn88Maxbet = new MaxbetSpread
      {
        Handicap = 0,
        OverUnder = 0
      };

      var maxbetInfo = new MaxbetInfo
      {
        AdminsiteID = adminsiteId,
        Traders = traderAssignmentsSender,
        Spread = spread,
        Maxbet = maxbet,
        MaxbetCN88 = cn88Maxbet,
        NumberOfLines = new NumberOfLines(),
        SecondAcceptValue = secondAcceptValue
      };

      MaxbetHelper.SetDefaultMaxbetInfo(maxbetInfo);

      return maxbetInfo;
    }
  }
}