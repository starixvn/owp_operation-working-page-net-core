using OWP.AutoAssignment.Models.InternalApi;

namespace OWP.AutoAssignment.Services.InternalApi
{
    using System.Globalization;

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
                exportMatchModel.MatchOffer = leagueSettingInfo.MatchOffer;
                exportMatchModel.LSTNote = leagueSettingInfo.LSTNote;
                exportMatchModel.MaxBetHDP = FormatMaxbet(leagueSettingInfo.MaxBetHDP);
                exportMatchModel.SpreadHDP = FormatSpread(leagueSettingInfo.SpreadHDP);
                exportMatchModel.SpreadOU = FormatSpread(leagueSettingInfo.SpreadOU);
                exportMatchModel.NoHDP = leagueSettingInfo.NoHDP;
                exportMatchModel.MaxBetOU = FormatMaxbet(leagueSettingInfo.MaxBetOU);
                exportMatchModel.MaxBet1HHDP = FormatMaxbet(leagueSettingInfo.MaxBet1HHDP);
                exportMatchModel.MaxBet1HOU = FormatMaxbet(leagueSettingInfo.MaxBet1HOU);
                exportMatchModel.Spread1HHDP = FormatSpread(leagueSettingInfo.Spread1HHDP);
                exportMatchModel.Spread1HOU = FormatSpread(leagueSettingInfo.Spread1HOU);
                exportMatchModel.MaxBet1X2 = FormatMaxbet(leagueSettingInfo.MaxBet1X2);
                exportMatchModel.Spread1X2 = FormatSpread(leagueSettingInfo.Spread1X2);
                exportMatchModel.MaxBet1H1X2 = FormatMaxbet(leagueSettingInfo.MaxBet1H1X2);
            }
        }

        private async Task<IEnumerable<LeagueSettingInfo>> LeagueSettingInfo(IList<int> queryInfo, string requestId)
            => await repository.GetLeagueSettingInfo(queryInfo, requestId);

        private LeagueSettingInfo GetInfoLeagueSettingById(IList<LeagueSettingInfo> leagueSettingExportModel, int id)
            => leagueSettingExportModel
                .FirstOrDefault(x => x.MatchId == id);

        private IList<int> GetMatchIdQueryInfo(IList<AutoAssignmentResultModel> autoAssignModels)
            => autoAssignModels
                .Select(i => i.InfoSiteMatchId).ToList();

        private static string FormatMaxbet(decimal? maxbet)
            => maxbet.HasValue ? (maxbet.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture) + "K" : string.Empty;

        public static string FormatSpread(decimal? spread)
            => spread.HasValue ? (spread.Value * 100).ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
    }
}