using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWP.AutoAssignment.Models.InternalApi;

namespace OWP.AutoAssignment.Services.InternalApi
{
    using System;
    using Builder;
    using Constant;

    public interface ProviderBuilderService
    {
        Task Build(
            IList<ExportMatchModel> models,
            IList<AutoAssignmentResultModel> autoAssignModels, string requestId);
    }
    public class ProviderBuilderServiceImp : ProviderBuilderService
    {
        private readonly Repository repository;

        public ProviderBuilderServiceImp(Repository repository)
        {
            this.repository = repository;
        }
        public async Task Build(IList<ExportMatchModel> models, IList<AutoAssignmentResultModel> autoAssignModels, string requestId)
        {
            var matchIds = GetMatchIdQueryInfo(autoAssignModels);
            var providerInfos = (await QueryProviderInfo(matchIds, requestId)).ToList();

            BuildExportProvider(models, providerInfos, autoAssignModels);
        }

        private void BuildExportProvider(IList<ExportMatchModel> exportModels,
            IList<ProviderInfo> providerInfos, IList<AutoAssignmentResultModel> autoAssignModels)
        {
            foreach (var exportMatchModel in exportModels)
            {
                var pcCode = GetPcCodeById(autoAssignModels, exportMatchModel.InfoSiteMatchId);
                var providerInfoById = GetInfoProviderById(providerInfos, exportMatchModel.InfoSiteMatchId);
                exportMatchModel.Provider = BuildProvider(providerInfoById, pcCode);
                exportMatchModel.OtherProvider = providerInfoById.OtherProvider;
            }
        }

        public string BuildProvider(ProviderInfo provider, string pcCode)
        {
            var reporter = GetReporter(provider.Provider);
            var firstProvider = FirstProviderBuilder.GetFirstProvider(provider.OtherProvider);
            var providerItems = new List<string>
            {
                provider.ProviderTv,
                firstProvider,
                reporter,
                pcCode
            };

            return string.Join("/", providerItems.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private static string GetReporter(string provider)
        {
            string reporter = string.Empty;
            if (provider.Contains(AutoConstant.Woo, StringComparison.OrdinalIgnoreCase))
            {
                reporter += AutoConstant.ReporterWoo;
            }
            if (provider.Contains(AutoConstant.Al, StringComparison.OrdinalIgnoreCase))
            {
                reporter += AutoConstant.ReporterAl;
            }
            if (provider.Contains(AutoConstant.Slr, StringComparison.OrdinalIgnoreCase))
            {
                reporter += AutoConstant.ReporterSlr;
            }

            return reporter;
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

        private async Task<IEnumerable<ProviderInfo>> QueryProviderInfo(IList<int> queryInfo, string requestId)
            => await repository.GetProviderInfo(queryInfo, requestId);
    }
}
