using OWP.Maxbet.Models;
using OWP.Maxbet.Selector;
using OWP.Services;

namespace OWP.Maxbet.Services
{
  public interface SpecialProductService
  {
    Task<MinorMatchInfo> GetMatchIdByAdminSiteId(int adminsiteId, int bettypeId);

    Task<MaxbetInfo> GetSpecialProductInfo(MinorMatchInfo owpMatch, int adminsiteId, int bettypeId);
  }

  public class SpecialProductServiceImp : SpecialProductService
  {
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly SpecialProductSelector specialProductSelector;

    private readonly LogServiceWrapper logServiceWrapper;

    public SpecialProductServiceImp(
        MaxbetRepository syncMaxbetRepository,
        SpecialProductSelector specialProductSelector,
        LogServiceWrapper logServiceWrapper)
    {
      this.syncMaxbetRepository = syncMaxbetRepository;
      this.specialProductSelector = specialProductSelector;
      this.logServiceWrapper = logServiceWrapper;
    }
    public async Task<MinorMatchInfo> GetMatchIdByAdminSiteId(int adminsiteId, int bettypeId)
            => await syncMaxbetRepository.GetMatchIdByAdminSiteId(adminsiteId, bettypeId);

    public async Task<MaxbetInfo> GetSpecialProductInfo(MinorMatchInfo owpMatch, int adminsiteId, int bettypeId)
    {
      var traders = await specialProductSelector.SelectTrader(owpMatch.MinorMatchId, bettypeId);
      var spread = await specialProductSelector.SelectSpread(owpMatch.MinorMatchId, bettypeId);
      var maxbet = await specialProductSelector.SelectMaxbet(owpMatch, bettypeId);
      var secondAcceptValue = await specialProductSelector.SelectSecondAcceptedValues(owpMatch.MinorMatchId, bettypeId);
      var numberOfLines = await specialProductSelector.SelectNumberOfLines(owpMatch.MinorMatchId, bettypeId);

      await logServiceWrapper.Log("=====GetSpecialProductInfo request======", owpMatch, adminsiteId, bettypeId);
      await logServiceWrapper.Log("=====GetSpecialProductInfo======", traders, spread, maxbet, secondAcceptValue, numberOfLines);

      var maxbetCN88 = new MaxbetSpread
      {
        FirstHalfHandicap = 0,
        FirstHalfOverUnder = 0,
        Handicap = 0,
        OverUnder = 0
      };

      return new MaxbetInfo
      {
        AdminsiteID = adminsiteId,
        Traders = traders,
        Spread = spread,
        Maxbet = maxbet,
        MaxbetCN88 = maxbetCN88,
        SecondAcceptValue = secondAcceptValue,
        NumberOfLines = numberOfLines
      };
    }
  }
}