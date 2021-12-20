using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.InternalApi
{
    using AutoMapper;
    using Models.InternalApi;

    public interface MatchInfoBuilderService
    {
        Task Build(
            IList<ExportMatchModel> models,
            IList<AutoAssignmentResultModel> autoAssignModels
            );
    }
    public class MatchInfoBuilderServiceImp : MatchInfoBuilderService
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        private readonly ProviderService providerService;

        public MatchInfoBuilderServiceImp(Repository repository, IMapper mapper, ProviderService providerService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.providerService = providerService;
        }


        public async Task Build(IList<ExportMatchModel> models, IList<AutoAssignmentResultModel> autoAssignModels)
        {
            var matchQueryInfo = BuildMatchQueryInfo(autoAssignModels);
            var matchIds = GetMatchIdQueryInfo(autoAssignModels);
            var matchInfos = (await QueryMatchInfo(matchQueryInfo)).ToList();
            var providerInfos = (await QueryProviderInfo(matchIds)).ToList();
            var leagueSettings = (await LeagueSettingInfo(matchIds)).ToList();
            BuildExportMatchModel(models, matchInfos, providerInfos, leagueSettings, autoAssignModels);
        }

        private IList<MatchQueryInfo> BuildMatchQueryInfo(IList<AutoAssignmentResultModel> autoAssignModels)
        => autoAssignModels
                .Select(model => new MatchQueryInfo
                {
                    InfoSiteMatchId = model.InfoSiteMatchId,
                    BetType = model.Bettype
                }).ToList();

        private IList<int> GetMatchIdQueryInfo(IList<AutoAssignmentResultModel> autoAssignModels)
            => autoAssignModels
                .Select(i => i.InfoSiteMatchId).ToList();

        private async Task<IEnumerable<InfoSiteMatchModel>> QueryMatchInfo(IList<MatchQueryInfo> queryInfo)
            => await repository.GetInfoSiteMatchInfo(queryInfo);

        private async Task<IEnumerable<ProviderInfo>> QueryProviderInfo(IList<int> queryInfo)
            => await repository.GetProviderInfo(queryInfo);

        private async Task<IEnumerable<LeagueSettingInfo>> LeagueSettingInfo(IList<int> queryInfo)
            => await repository.GetLeagueSettingInfo(queryInfo);

        private void BuildExportMatchModel(IList<ExportMatchModel> models,
            List<InfoSiteMatchModel> matchInfos,
            IList<ProviderInfo> providerInfos,
            IList<LeagueSettingInfo> leagueSettingInfos,
            IList<AutoAssignmentResultModel> autoAssignModels)
        {
            foreach (var match in matchInfos)
            {
                var provider = providerInfos.FirstOrDefault(i => i.MatchId == match.InfoSiteMatchId);
                var leagueSetting = leagueSettingInfos.FirstOrDefault(x => x.MatchId == match.InfoSiteMatchId);
                var pcCode = autoAssignModels.FirstOrDefault(x => x.InfoSiteMatchId == match.InfoSiteMatchId)?.PcCode;
                var matchModel = mapper.Map<ExportMatchModel>(match);
                matchModel.Provider = providerService.Build(provider, pcCode);
                matchModel.OtherProvider = provider?.OtherProvider;
                matchModel.MatchOffer = leagueSetting?.MatchOffer;
                matchModel.LSTNote = leagueSetting?.LSTNote;
                matchModel.MaxBetHDP = leagueSetting?.MaxBetHDP;
                matchModel.SpreadHDP = leagueSetting?.SpreadHDP;
                matchModel.SpreadOU = leagueSetting?.SpreadOU;
                matchModel.NoHDP = leagueSetting?.NoHDP;
                matchModel.MaxBetOU = leagueSetting?.MaxBetOU;
                matchModel.MaxBet1HHDP = leagueSetting?.MaxBet1HHDP;
                matchModel.MaxBet1HOU = leagueSetting?.MaxBet1HOU;
                matchModel.Spread1HHDP = leagueSetting?.Spread1HHDP;
                matchModel.Spread1HOU = leagueSetting?.Spread1HOU;
                matchModel.MaxBet1X2 = leagueSetting?.MaxBet1X2;
                matchModel.Spread1X2 = leagueSetting?.Spread1X2;
                matchModel.MaxBet1H1X2 = leagueSetting?.MaxBet1H1X2;
                matchModel.Spread1H1X2 = leagueSetting?.Spread1H1X2;

                models.Add(matchModel);
            }

        }

    }
}
