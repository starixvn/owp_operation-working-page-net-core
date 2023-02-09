namespace OWP.AutoAssignment.Services.InternalApi;

using System.Globalization;
using Constant;
using Models.InternalApi;

public interface LeagueSettingBuilderService
{
    Task Build(
        IList<ExportMatchModel> models,
        IList<AutoAssignmentResultModel> autoAssignModels,
        string requestId);
}

public class LeagueSettingBuilderServiceImp : LeagueSettingBuilderService
{
    private readonly Repository repository;

    public LeagueSettingBuilderServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task Build(IList<ExportMatchModel> models, IList<AutoAssignmentResultModel> autoAssignModels, string requestId)
    {
        var matchIds = GetMatchIdQueryInfo(autoAssignModels);
        var leagueSettings = (await LeagueSettingInfo(matchIds, requestId)).ToList();

        BuildExportLeagueSetting(models, leagueSettings);
    }

    private void BuildExportLeagueSetting(IList<ExportMatchModel> exportModels,
        IList<LeagueSettingInfo> leagueSettings)
    {
        foreach (var exportMatchModel in exportModels)
        {
            var leagueSettingInfo = GetInfoLeagueSettingById(leagueSettings, exportMatchModel.InfoSiteMatchId);
            BuildExportMaxBet(exportMatchModel, leagueSettingInfo);
        }
    }

    private void BuildExportMaxBet(ExportMatchModel exportModel, LeagueSettingInfo leagueSetting)
    {
        if (exportModel.Bettype == BetType.NoCorner)
        {
            exportModel.MaxBetHDP = FormatMaxbet(leagueSetting.MaxbetNoOfCorner);
            exportModel.MaxBetOU = FormatMaxbet(leagueSetting.MaxbetNoOfCorner);
        }
        else if (exportModel.Bettype == BetType.NextCorner)
        {
            exportModel.MaxBetHDP = FormatMaxbet(leagueSetting.MaxBetNextCorner);
        }
        else
        {
            exportModel.MatchOffer = leagueSetting.MatchOffer;
            exportModel.LSTNote = leagueSetting.LSTNote;
            exportModel.NoHDP = leagueSetting.NoHDP;
            exportModel.MaxBetHDP = FormatMaxbet(leagueSetting.MaxBetHDP);
            exportModel.SpreadHDP = FormatSpread(leagueSetting.SpreadHDP);
            exportModel.SpreadOU = FormatSpread(leagueSetting.SpreadOU);
            exportModel.MaxBetOU = FormatMaxbet(leagueSetting.MaxBetOU);
            exportModel.MaxBet1HHDP = FormatMaxbet(leagueSetting.MaxBet1HHDP);
            exportModel.MaxBet1HOU = FormatMaxbet(leagueSetting.MaxBet1HOU);
            exportModel.Spread1HHDP = FormatSpread(leagueSetting.Spread1HHDP);
            exportModel.Spread1HOU = FormatSpread(leagueSetting.Spread1HOU);
            exportModel.MaxBet1X2 = FormatMaxbet(leagueSetting.MaxBet1X2);
            exportModel.Spread1X2 = FormatSpread(leagueSetting.Spread1X2);
            exportModel.MaxBet1H1X2 = FormatMaxbet(leagueSetting.MaxBet1H1X2);
        }
    }

    private async Task<IEnumerable<LeagueSettingInfo>> LeagueSettingInfo(IList<int> queryInfo, string requestId)
        => await repository.GetLeagueSettingInfo(queryInfo, requestId);

    private LeagueSettingInfo GetInfoLeagueSettingById(IList<LeagueSettingInfo> leagueSettingExportModel, int id)
        => leagueSettingExportModel
               .FirstOrDefault(x => x.MatchId == id)
           ?? new LeagueSettingInfo();

    private IList<int> GetMatchIdQueryInfo(IList<AutoAssignmentResultModel> autoAssignModels)
        => autoAssignModels
            .Select(i => i.InfoSiteMatchId).ToList();

    private static string FormatMaxbet(decimal? maxbet)
        => maxbet.HasValue ? (maxbet.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture) + "K" : string.Empty;

    public static string FormatSpread(decimal? spread)
        => spread.HasValue ? (spread.Value * 100).ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
}