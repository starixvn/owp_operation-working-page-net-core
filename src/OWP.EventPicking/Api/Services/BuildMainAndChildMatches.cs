using Microsoft.Extensions.Options;
using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using OWP.Models;

namespace OWP.EventPicking.Api.Services;

public interface BuildMainAndChildMatches
{
    Task<List<EventPickingModel>> GetMainAndChildMatchEvent(DateTime date);

    Task<List<EventPickingModel>> GetMainEvent(DateTime date);

    Task<List<EventPickingModel>> GetChildEventEventPicking(DateTime date);
}

public class BuildMainAndChildMatchesImp: BuildMainAndChildMatches
{
    private readonly AppSettings appSettings;
    private readonly Repository repository;
    private readonly BuildChildMatchMaxBet buildChildMatchMaxBet;
    private readonly BuildRowColor buildRowColor;
    private readonly BuildMatchOfferByLiveSetting buildMatchOffer;
    private readonly BuildProvider buildProvider;

    public BuildMainAndChildMatchesImp(
        Repository repository,
        BuildChildMatchMaxBet buildChildMatchMaxBet,
        BuildRowColor buildRowColor,
        BuildMatchOfferByLiveSetting buildMatchOffer,
        BuildProvider buildProvider,
        IOptions<AppSettings> appSettingOptions,
        MaxbetSettingService maxbetSettingService)
    {
        appSettings = appSettingOptions.Value;
        this.repository = repository;
        this.buildChildMatchMaxBet = buildChildMatchMaxBet;
        this.buildRowColor = buildRowColor;
        this.buildMatchOffer = buildMatchOffer;
        this.buildProvider = buildProvider;
    }

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
}