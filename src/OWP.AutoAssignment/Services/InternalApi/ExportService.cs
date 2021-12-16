using OWP.AutoAssignment.Models.InternalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.InternalApi
{
    public interface ExportService
    {
        Task<IList<ExportMatchModel>> BuildExportModelByDate(DateTime eventDate);
    }

    public class ExportServiceImp : ExportService
    {
        private readonly LABuilderService lABuilderService;

        private readonly TraderBuilderService traderBuilderService;

        private readonly MatchInfoBuilderService matchInfoService;

        private readonly Repository repository;

        public IList<ExportMatchModel> ExportModels { get; set; } = new List<ExportMatchModel>();

        private IList<AutoAssignmentResultModel> autoAssignModels = new List<AutoAssignmentResultModel>();

        public ExportServiceImp(
            LABuilderService lABuilderService,
            TraderBuilderService traderBuilderService,
             MatchInfoBuilderService matchInfoService,
             Repository repository)
        {
            this.lABuilderService = lABuilderService;
            this.traderBuilderService = traderBuilderService;
            this.matchInfoService = matchInfoService;
            this.repository = repository;
        }

        public async Task<IList<ExportMatchModel>> BuildExportModelByDate(DateTime eventDate)
        {
            autoAssignModels = (await repository.GetAutoAssignmentResultByDate(eventDate)).ToList();
            await BuildMatchInfo();
            await BuildLAInfo();
            await BuildTraderInfo();
            return ExportModels;
        }

        private async Task BuildMatchInfo()
            => await matchInfoService.Build(ExportModels, autoAssignModels);

        private async Task BuildLAInfo()
            => await lABuilderService.Build(ExportModels, autoAssignModels);

        private async Task BuildTraderInfo()
            => await traderBuilderService.Build(ExportModels, autoAssignModels);
    }
}