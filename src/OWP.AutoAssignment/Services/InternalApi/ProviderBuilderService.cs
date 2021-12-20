using OWP.AutoAssignment.Models.InternalApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.InternalApi
{
    public interface ProviderBuilderService
    {
        Task Build(
            IList<ExportMatchModel> models,
            IList<AutoAssignmentResultModel> autoAssignModels);
    }
    public class ProviderBuilderServiceImp : ProviderBuilderService
    {
        private readonly Repository repository;
        private readonly ProviderService providerService;
        public ProviderBuilderServiceImp(Repository repository, ProviderService providerService)
        {
            this.repository = repository;
            this.providerService = providerService;
        }
        public async Task Build(IList<ExportMatchModel> models, IList<AutoAssignmentResultModel> autoAssignModels)
        {
            var matchIds = GetMatchIdQueryInfo(autoAssignModels);
            var providerInfos = (await QueryProviderInfo(matchIds)).ToList();

            BuildExportProvider(models, providerInfos, autoAssignModels);
        }

        private void BuildExportProvider(IList<ExportMatchModel> exportModels,
            IList<ProviderInfo> providerInfos, IList<AutoAssignmentResultModel> autoAssignModels)
        {
            foreach (var exportMatchModel in exportModels)
            {
                var pcCode = GetPcCodeById(autoAssignModels, exportMatchModel.InfoSiteMatchId);
                var providerInfoById = GetInfoProviderById(providerInfos, exportMatchModel.InfoSiteMatchId);
                exportMatchModel.Provider = providerService.Build(providerInfoById, pcCode);
                exportMatchModel.OtherProvider = providerInfoById.OtherProvider;
            }
        }

        private string GetPcCodeById(IList<AutoAssignmentResultModel> autoAssignmentResult, int id)
            => autoAssignmentResult
                .FirstOrDefault(x => x.InfoSiteMatchId == id)?.PcCode;

        private ProviderInfo GetInfoProviderById(IList<ProviderInfo> providerExportModel, int id)
            => providerExportModel
                .FirstOrDefault(x => x.MatchId == id);

        private IList<int> GetMatchIdQueryInfo(IList<AutoAssignmentResultModel> autoAssignModels)
            => autoAssignModels
                .Select(i => i.InfoSiteMatchId).ToList();
        private async Task<IEnumerable<ProviderInfo>> QueryProviderInfo(IList<int> queryInfo)
            => await repository.GetProviderInfo(queryInfo);
    }
}
