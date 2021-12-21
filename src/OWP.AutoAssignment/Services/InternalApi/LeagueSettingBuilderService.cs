using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWP.AutoAssignment.Models.InternalApi;

namespace OWP.AutoAssignment.Services.InternalApi
{
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
                exportMatchModel.MatchOffer = leagueSettingInfo?.MatchOffer;
                exportMatchModel.LSTNote = leagueSettingInfo?.LSTNote;
                exportMatchModel.MaxBetHDP = leagueSettingInfo?.MaxBetHDP;
                exportMatchModel.SpreadHDP = leagueSettingInfo?.SpreadHDP;
                exportMatchModel.SpreadOU = leagueSettingInfo?.SpreadOU;
                exportMatchModel.NoHDP = leagueSettingInfo?.NoHDP;
                exportMatchModel.MaxBetOU = leagueSettingInfo?.MaxBetOU;
                exportMatchModel.MaxBet1HHDP = leagueSettingInfo?.MaxBet1HHDP;
                exportMatchModel.MaxBet1HOU = leagueSettingInfo?.MaxBet1HOU;
                exportMatchModel.Spread1HHDP = leagueSettingInfo?.Spread1HHDP;
                exportMatchModel.Spread1HOU = leagueSettingInfo?.Spread1HOU;
                exportMatchModel.MaxBet1X2 = leagueSettingInfo?.MaxBet1X2;
                exportMatchModel.Spread1X2 = leagueSettingInfo?.Spread1X2;
                exportMatchModel.MaxBet1H1X2 = leagueSettingInfo?.MaxBet1H1X2;
                exportMatchModel.Spread1H1X2 = leagueSettingInfo?.Spread1H1X2;
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
    }
}
