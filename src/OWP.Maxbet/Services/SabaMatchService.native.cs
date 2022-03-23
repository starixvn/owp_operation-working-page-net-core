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

      MaxbetHelper.UpdateTraderETPK(traderAssignmentsSender);

      var secondAcceptValue = await mediator.Send(new GetSecondAcceptValueHandler.Request { OWPMatchId = owpMatchId });

      var spread = new MaxbetSpread
      {
        Handicap = (decimal)MaxBetSetting.Spread_Saba,
        OverUnder = (decimal)MaxBetSetting.Spread_Saba,
        FirstHalfHandicap = null,
        FirstHalfOverUnder = null
      };

      var maxbet = new MaxbetSpread
      {
        Handicap = MaxBetSetting.Maxbet_Saba,
        OverUnder = MaxBetSetting.Maxbet_Saba,
        FirstHalfHandicap = null,
        FirstHalfOverUnder = null
      };

      var cn88Maxbet = new MaxbetSpread
      {
        FirstHalfHandicap = null,
        FirstHalfOverUnder = null,
        Handicap = 0,
        OverUnder = 0
      };

      var numberOfLines = new NumberOfLines
      {
        CreditFt = (int)NumberOfLinesETPEN.CreditFt,
        CreditFirstHalf = (int)NumberOfLinesETPEN.CreditFirstHalf,
        LicenseeFt = (int)NumberOfLinesETPEN.LicenseeFt,
        LicenseeFirstHalf = (int)NumberOfLinesETPEN.LicenseeFirstHalf
      };

      return new MaxbetInfo
      {
        AdminsiteID = adminsiteId,
        Traders = traderAssignmentsSender,
        Spread = spread,
        Maxbet = maxbet,
        MaxbetCN88 = cn88Maxbet,
        NumberOfLines = numberOfLines,
        SecondAcceptValue = secondAcceptValue
      };
    }
  }
}