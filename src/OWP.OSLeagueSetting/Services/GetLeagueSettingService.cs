using OWP.OSLeagueSetting.Constants;
using OWP.OSLeagueSetting.Handlers;
using OWP.OSLeagueSetting.Models;
using AutoMapper;
using OWP.OSLeagueSetting.Helper;

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
            league.SummaryInfo = OSLeagueSettingHelper.CalculateSummaryInfo(leagueDict[key]).ToList();
            league.SummaryRemark = OSLeagueSettingHelper.CalculateSummaryRemark(leagueDict[key]).ToList();
            leagueSettings.Add(league);
        }

        return SortLeagueSetting(leagueSettings);
    }

    private static IList<LeagueSettingModel> SortLeagueSetting(List<LeagueSettingModel> leagueSetting)
        => leagueSetting
                .OrderBy(x => x.OSLeagueName)
                .ThenByDescending(x => x.MaxBetHDP)
                .ThenByDescending(x => x.MaxBetOU)
                .ToList();
}