using Microsoft.Extensions.Options;
using OWP.EventPicking.Api.Helper;
using OWP.Models;

namespace OWP.EventPicking.Api.Services;

public interface BuildMainAndChildMatches
{
    Task<List<EventPickingModel>> GetMainAndChildMatchEvent(DateTime date);

    Task<List<EventPickingModel>> GetMainEvent(DateTime date);

    Task<List<EventPickingModel>> GetChildEventEventPicking(DateTime date);
}

public class BuildMainAndChildMatchesImp(
    Repository repository,
    BuildChildMatchMaxBet buildChildMatchMaxBet,
    BuildRowColor buildRowColor,
    BuildMatchOfferByLiveSetting buildMatchOffer,
    BuildProvider buildProvider,
    IOptions<AppSettings> appSettingOptions,
    SyncEventPickingInfo syncEventPickingInfo) : BuildMainAndChildMatches
{
    private readonly AppSettings appSettings = appSettingOptions.Value;

    public async Task<List<EventPickingModel>> GetMainAndChildMatchEvent(DateTime date)
    {
        var mainMatches = await repository.QueryMainMatch(date);
        var childMatches = await repository.QueryChildMatch(date);
        var maxBet = await buildChildMatchMaxBet.GetMaxBetDefaultSetting();

        buildChildMatchMaxBet.BuildExport(mainMatches, childMatches, maxBet);
        buildMatchOffer.Build(mainMatches, childMatches, maxBet);
        var filteredMainMatches = MainMatchHelper.Filter(mainMatches, appSettings.ShowTestMatchEventPicking);
        var allMatches = SpecialProductHelper.CombineMainAndChildMatchAndAddMatchNo(filteredMainMatches, childMatches);
        buildRowColor.BuildRowColor(allMatches);
        await buildProvider.Build(allMatches);

        await SyncInfoFromLeagueSetting(allMatches);

        return allMatches;
    }

    public async Task<List<EventPickingModel>> GetMainEvent(DateTime date)
    {
        var mainMatches = repository.QueryMainMatch(date);
        var maxBet = buildChildMatchMaxBet.GetMaxBetDefaultSetting();
        var rAndDInfo = repository.GetRAndDUpdateInfo(date);
        var (movedEventDateMatches, movedEventDateMatchesRnD) = await repository.QueryMovedEventDateMatch(date);
        await Task.WhenAll(mainMatches, maxBet, rAndDInfo);

        if (movedEventDateMatches.Count > 0)
        {
            var existedMatchIds = mainMatches.Result.Select(m => m.InfoSiteMatchId);
            movedEventDateMatches.RemoveAll(x => existedMatchIds.Any(i => i == x.InfoSiteMatchId));
            movedEventDateMatchesRnD.RemoveAll(x => existedMatchIds.Any(i => i == x.InfositeId));

            mainMatches.Result.AddRange(movedEventDateMatches);
            rAndDInfo.Result.AddRange(movedEventDateMatchesRnD);
        }

        buildMatchOffer.BuildForMainMatch(mainMatches.Result, maxBet.Result);
        var filteredMainMatches = MainMatchHelper.Filter(mainMatches.Result, appSettings.ShowTestMatchEventPicking);

        var filteredMainMatchesSorted = SortEventPicking.SortList(filteredMainMatches);

        RAndDInfoHelper.Build(filteredMainMatchesSorted, rAndDInfo.Result);
        buildRowColor.BuildRowColor(filteredMainMatchesSorted);
        await buildProvider.Build(filteredMainMatchesSorted);
        return filteredMainMatchesSorted;
    }

    public async Task<List<EventPickingModel>> GetChildEventEventPicking(DateTime date)
    {
        var generatedChildMatch = await repository.QueryEventPickingChildMatch(date);
        SpecialProductHelper.BuildSpecialProduct(generatedChildMatch);
        buildRowColor.BuildRowColor(generatedChildMatch, true);
        await buildProvider.Build(generatedChildMatch);

        return generatedChildMatch;
    }

    private async Task SyncInfoFromLeagueSetting(
       List<EventPickingModel> filteredMatches)
    {
        var leagueSetting = (await repository.QueryLeagueSetting()).ToList();
        var tvList = (await repository.GetTVList()).ToList();

        syncEventPickingInfo.Execute(filteredMatches, leagueSetting, tvList);
    }
}