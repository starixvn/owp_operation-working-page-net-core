using OWP.AutoAssignment.Models.InternalApi;

namespace OWP.AutoAssignment.Services.InternalApi
{
    public interface ExportService
    {
        Task<IList<ExportMatchModel>> BuildExportModelByDate(DateTime eventDate, string requestId);
    }

    public class ExportServiceImp : ExportService
    {
        private readonly LABuilderService lABuilderService;

        private readonly TraderBuilderService traderBuilderService;

        private readonly MatchInfoBuilderService matchInfoService;

        private readonly ProviderBuilderService providerBuilderService;

        private readonly LeagueSettingBuilderService leagueSettingBuilderService;

        private readonly Repository repository;

        public IList<ExportMatchModel> ExportModels { get; set; } = new List<ExportMatchModel>();

        private IList<AutoAssignmentResultModel> autoAssignModels = new List<AutoAssignmentResultModel>();

        public ExportServiceImp(
            LABuilderService lABuilderService,
            TraderBuilderService traderBuilderService,
             MatchInfoBuilderService matchInfoService,
            ProviderBuilderService providerBuilderService,
            LeagueSettingBuilderService leagueSettingBuilderService,
             Repository repository)
        {
            this.lABuilderService = lABuilderService;
            this.traderBuilderService = traderBuilderService;
            this.matchInfoService = matchInfoService;
            this.providerBuilderService = providerBuilderService;
            this.leagueSettingBuilderService = leagueSettingBuilderService;
            this.repository = repository;
        }

        public async Task<IList<ExportMatchModel>> BuildExportModelByDate(DateTime eventDate, string requestId)
        {
            autoAssignModels = (await repository.GetAutoAssignmentResultByDate(eventDate)).ToList();
            await BuildMatchInfo();
            await BuildLAInfo();
            await BuildTraderInfo();
            await BuildProviderInfo(requestId);
            await BuildLeagueSettingInfo(requestId);
            return ExportModels
                .OrderBy(x => DateTime.Parse(x.EventDateTime))
                .ToList();
        }

        private async Task BuildMatchInfo()
            => await matchInfoService.Build(ExportModels, autoAssignModels);

        private async Task BuildLAInfo()
            => await lABuilderService.Build(ExportModels, autoAssignModels);

        private async Task BuildTraderInfo()
            => await traderBuilderService.Build(ExportModels, autoAssignModels);

        private async Task BuildProviderInfo(string requestId)
            => await providerBuilderService.Build(ExportModels, autoAssignModels, requestId);

        private async Task BuildLeagueSettingInfo(string requestId)
            => await leagueSettingBuilderService.Build(ExportModels, autoAssignModels, requestId);
    }
}