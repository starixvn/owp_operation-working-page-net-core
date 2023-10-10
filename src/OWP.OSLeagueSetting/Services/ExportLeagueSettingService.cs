

using AutoMapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using OWP.OSLeagueSetting.Constants;
using OWP.OSLeagueSetting.Handlers;
using OWP.OSLeagueSetting.Helper;
using OWP.OSLeagueSetting.Models;

namespace OWP.OSLeagueSetting.Services;
public interface ExportLeagueSettingService
{
    Task<IList<LeagueSettingExport>> ExportLeagueSetting(ExportLeagueSettingRequest request);
}
public class ExportLeagueSettingServiceImp : ExportLeagueSettingService
{
    private readonly Repository repository;
    private readonly IMapper mapper;

    public ExportLeagueSettingServiceImp(Repository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IList<LeagueSettingExport>> ExportLeagueSetting(ExportLeagueSettingRequest request)
    {
        var modelRequest = mapper.Map<GetLeagueInfoHandler.Request>(request);
        var leagueInfoRaw = await repository.GetLeagueSettingInfo(modelRequest);
        var leagueInfos = leagueInfoRaw.Where(x => x.IsActiveMaxBet);
        var leagueDict = leagueInfos.GroupBy(x => x.LeagueSettingId)
            .ToDictionary(g => g.Key, g => g.ToList());
        var leagueSettings = new List<LeagueSettingExport>();
        foreach (var key in leagueDict.Keys)
        {
            var leagueFirst = leagueDict[key].FirstOrDefault();
            var offerLines = BuildOfferLines(leagueDict[key]);
            var betDelays = BuildBetDelay(leagueDict[key]);
            var bettypes = BuildNumberBettype(leagueDict[key]);

            var league = mapper.Map<LeagueSettingExport>(leagueFirst);
            league.OfferLines = offerLines;
            league.BetDelays = betDelays;
            league.RoundName = OSLeagueSettingHelper.CalculateRoundName(leagueDict[key]);
            league.SummaryFirst = OSLeagueSettingHelper.CalculateSummaryExportFirst(leagueFirst);
            league.SummarySecond = OSLeagueSettingHelper.CalculateSummaryExportSecond(offerLines, bettypes, betDelays);
            league.Remark = OSLeagueSettingHelper.CalculateRemarkExport(leagueDict[key]);
            leagueSettings.Add(league);
        }

        return SortLeagueSetting(leagueSettings);
    }

    private static IList<LeagueSettingExport> SortLeagueSetting(IEnumerable<LeagueSettingExport> leagueSetting)
        => leagueSetting
            
            .OrderBy(x => x.OSLeagueName)
            .ThenByDescending(x => x.MaxBetHDP)
            .ThenByDescending(x => x.MaxBetOU)
            .ToList();

    private static string BuildOfferLines(List<LeagueSettingInfo> leagueSettingInfos)
    {
        var offerLines = leagueSettingInfos.Select(x => x.OfferLines).Distinct();
        return OSLeagueSettingHelper.FormatToStringExport(offerLines);
    }

    private static string BuildBetDelay(List<LeagueSettingInfo> leagueSettingInfos)
    {
        var betDeyLays = leagueSettingInfos.Select(x => x.BetDelay).Distinct();
        return OSLeagueSettingHelper.FormatToStringExport(betDeyLays);
    }

    private static string BuildNumberBettype(List<LeagueSettingInfo> leagueSettingInfos)
    {
        var betTypes = leagueSettingInfos
            .Select(x => x.BetType)
            .Distinct()
            .Count(x => !string.IsNullOrEmpty(x));
        return betTypes != 0 ? $"{betTypes} Bet Types" : string.Empty;
    }
}