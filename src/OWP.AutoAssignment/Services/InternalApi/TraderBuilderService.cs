using OWP.AutoAssignment.Criteria.InternalApi;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Utils.XMLExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;

namespace OWP.AutoAssignment.Services.InternalApi
{
    
    public interface TraderBuilderService
    {
        Task<object> Build(IList<TraderExportQueryInfo> info);

        Task Build(
            IList<ExportMatchModel> models,
            IList<AutoAssignmentResultModel> autoAssignModels);
    }

    public class TraderBuilderServiceImp : TraderBuilderService
    {
        private readonly Repository repository;
        private readonly IMapper mapper;
        public TraderBuilderServiceImp(Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<object> Build(IList<TraderExportQueryInfo> info)
        {
            var criteria = new GetTraderExportInfoCriteria
            {
                Xml = info.ToXml().ToString(SaveOptions.DisableFormatting)
            };

            return await repository.GetTraderExportInfo(criteria);
        }

        public async Task Build(
            IList<ExportMatchModel> models,
            IList<AutoAssignmentResultModel> autoAssignModels)
        {
            var traderQueryInfo = BuildTraderExportQueryInfo(autoAssignModels);
            var traderExportInfo = (await QueryTraderExportInfo(traderQueryInfo)).ToList();

            foreach (var exportMatchModel in models)
            {
                var findTraderInfoById = traderExportInfo
                    .FirstOrDefault(x => x.InfoSiteMatchId == exportMatchModel.InfoSiteMatchId);
                exportMatchModel.Trader1HHDP = findTraderInfoById?.Trader1HHDP;
                exportMatchModel.Trader1HOU = findTraderInfoById?.Trader1HOU;
                exportMatchModel.TraderHDP = findTraderInfoById?.TraderFTHDP;
                exportMatchModel.TraderOU = findTraderInfoById?.TraderFTOU;
            }
        }

        private IList<TraderExportQueryInfo> BuildTraderExportQueryInfo(IList<AutoAssignmentResultModel> autoAssignModels)
        => autoAssignModels
                .Select(model => new TraderExportQueryInfo
                {
                    InfoSiteMatchId = model.InfoSiteMatchId,
                    FTHDPAssingeeId = model.UserId_FTHDP,
                    FIRSTHHDPAssingeeId = model.UserId_1HHDP,
                    FTOUAssingeeId = model.UserId_FTOU,
                    FIRSTHOUAssingeeId = model.UserId_1HOU,
                    Bettype = model.Bettype
                }).ToList();

        private async Task<IEnumerable<TraderExportInfo>> QueryTraderExportInfo(IList<TraderExportQueryInfo> queryInfo)
        {
            var criteria = new GetTraderExportInfoCriteria
            {
                Xml = queryInfo.ToXml().ToString(SaveOptions.DisableFormatting)
            };

            return await repository.GetTraderExportInfo(criteria);
        }
        private void BuildExportTraderModel(IList<ExportMatchModel> exportModels,
            List<TraderExportInfo> traderExportModel)
            => traderExportModel.ForEach(m =>
            {
                var traderInfo = mapper.Map<ExportMatchModel>(m);
                exportModels.Add(traderInfo);
            });
    }
}