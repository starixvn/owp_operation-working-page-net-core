using OWP.Api.Maxbet.Helpers;
using OWP.Api.Maxbet.Models;
using OWP.Api.Maxbet.Selector;
using OWP.Enums;

namespace OWP.Api.Maxbet.Services;

public interface SpecialProductService
{
    Task<MinorMatchInfo> GetMatchIdByAdminSiteId(int adminsiteId, int bettypeId);

    Task<MaxbetInfo> GetSpecialProductInfo(MinorMatchInfo owpMatch, int adminsiteId, int bettypeId);

    Task<MaxbetInfo> GetMaxbetInfoET(int adminsiteId, int bettypeId);
}

public class SpecialProductServiceImp : SpecialProductService
{
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly SpecialProductSelector specialProductSelector;

    public SpecialProductServiceImp(
        MaxbetRepository syncMaxbetRepository,
        SpecialProductSelector specialProductSelector)
    {
        this.syncMaxbetRepository = syncMaxbetRepository;
        this.specialProductSelector = specialProductSelector;
    }

    public async Task<MinorMatchInfo> GetMatchIdByAdminSiteId(int adminsiteId, int bettypeId)
        => await syncMaxbetRepository.GetMatchIdByAdminSiteId(adminsiteId, bettypeId);

    public async Task<MaxbetInfo> GetMaxbetInfoET(int adminsiteId, int bettypeId)
    {
        var minorMatch = await GetMatchIdByAdminSiteId(adminsiteId, bettypeId);
        if (minorMatch is null)
        {
            return null;
        }
        var maxbetInfo = await GetSpecialProductInfoExcludeNumberOfLines(minorMatch, adminsiteId, bettypeId);
        maxbetInfo.NumberOfLines = NumberOfLineHelper.UpdateNumberOfLine();
        maxbetInfo.Ft1stLineHandicap = Ft1stLineHandicapHelper.BuildFt1stLineHandicap();
        return maxbetInfo;
    }

    public async Task<MaxbetInfo> GetSpecialProductInfo(MinorMatchInfo owpMatch, int adminsiteId, int bettypeId)
    {
        var maxbetInfo = await GetSpecialProductInfoExcludeNumberOfLines(owpMatch, adminsiteId, bettypeId);
        var numberOfLines = await specialProductSelector.SelectNumberOfLines(owpMatch.MinorMatchId, bettypeId);

        maxbetInfo.NumberOfLines = numberOfLines;

        return maxbetInfo;
    }

    private async Task<MaxbetInfo> GetSpecialProductInfoExcludeNumberOfLines(MinorMatchInfo owpMatch, int adminsiteId, int bettypeId)
    {
        var traders = await specialProductSelector.SelectTrader(owpMatch.MinorMatchId, bettypeId);
        var spread = await specialProductSelector.SelectSpread(owpMatch.MinorMatchId, bettypeId);
        var maxbet = await specialProductSelector.SelectMaxbet(owpMatch, bettypeId);
        var maxbetMainMatch = (await syncMaxbetRepository.GetMaxbetByOwpMatchId(owpMatch.ParentMatchId))
            .FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap));
        var secondAcceptValue = await specialProductSelector.SelectSecondAcceptedValues(owpMatch.MinorMatchId, bettypeId, maxbetMainMatch?.Value);


        return new MaxbetInfo
        {
            AdminsiteID = adminsiteId,
            Traders = traders,
            Spread = spread,
            Maxbet = maxbet,
            SecondAcceptValue = secondAcceptValue
        };
    }
}