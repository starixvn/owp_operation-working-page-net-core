using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OWP.OSLeagueSetting.Constants;
using OWP.OSLeagueSetting.Handlers;
using OWP.OSLeagueSetting.Models;

namespace OWP.OSLeagueSetting.Services;

public interface GetLeagueSettingService
{
    Task<IList<LeagueSettingModel>> GetLeagueSetting(GetLeagueInfoHandler.Request request);
}

public class GetLeagueSettingServiceImp : GetLeagueSettingService
{
    private readonly Repository repository;
    private readonly IMapper mapper;

    public GetLeagueSettingServiceImp(Repository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IList<LeagueSettingModel>> GetLeagueSetting(GetLeagueInfoHandler.Request request)
    {
        var leagueInfo = await repository.GetLeagueSettingInfo(request);

        var leagueDict = leagueInfo.GroupBy(x => x.LeagueSettingId)
            .ToDictionary(g => g.Key, g => g.ToList());
        var leagueSettings = new List<LeagueSettingModel>();
        foreach (var key in leagueDict.Keys)
        {
            var league = mapper.Map<LeagueSettingModel>(leagueDict[key].FirstOrDefault());
            league.SummaryInfo = CalculateSummaryInfo(leagueDict[key]);
            league.SummaryRemark = CalculateSummaryRemark(leagueDict[key]);
            leagueSettings.Add(league);
        }

        return leagueSettings;
    }

    private static string CalculateSummaryInfo(IList<LeagueSettingInfo> leagueSettingInfos)
    {
        var offerLines = leagueSettingInfos.Select(x => x.OfferLines).Distinct();
        var betTypes = leagueSettingInfos.Select(x => x.BetType).Distinct();
        var betDeyLay = leagueSettingInfos.Select(x => x.BetDelay).Distinct();

        var offerLineFormat = FormatToString(offerLines, LeagueSettingConst.OfferLines);
        var betTypeFormat = FormatNumberBetType(betTypes);
        var betDeyLayFormat = FormatToString(betDeyLay, LeagueSettingConst.BetDelay);
        var manualUpdate = leagueSettingInfos.Any(x => x.ManualUpdateScore)
            ? $"({LeagueSettingConst.ManualUpdate})"
            : string.Empty;

        return $"{offerLineFormat}{betTypeFormat}{betDeyLayFormat}{manualUpdate}";
    }

    private static string CalculateSummaryRemark(IList<LeagueSettingInfo> leagueSettingInfos)
    {
        var weekMatchs = leagueSettingInfos.Select(x => x.WeekMatch).Distinct();
        var providers = leagueSettingInfos.Select(x => x.Provider).Distinct();

        var weekMatchFormat = FormatToString(weekMatchs, LeagueSettingConst.WeekMatch);
        var providerFormat = FormatToString(providers, LeagueSettingConst.Provider);
        var timeFormat = FormatDateTime(leagueSettingInfos.FirstOrDefault());

        var IPTV = leagueSettingInfos.Any(x => x.IPTV)
            ? $"({LeagueSettingConst.IPTV})"
            : string.Empty;

        return $"{weekMatchFormat}{providerFormat}{timeFormat}{IPTV}";
    }

    private static string FormatToString(IEnumerable<string> values, string property)
    {
        var totalOption = LeagueSettingConst.SummaryFullOption.GetValueOrDefault(property);

        return values.Count() == totalOption
             ? string.Empty
             : $"(<{string.Concat('/', values)}>)";
    }

    private static string FormatNumberBetType(IEnumerable<string> betTypes)
        => betTypes.Count() == LeagueSettingConst.NumberBetType
            ? string.Empty
            : $"(<{betTypes.Count()} Bet Types>)";

    private static string FormatDateTime(LeagueSettingInfo leagueInfo)
        => $"{leagueInfo.KOFromTime.ToString("hh:mm")} - {leagueInfo.KOToTime.ToString("hh:mm")}";
}