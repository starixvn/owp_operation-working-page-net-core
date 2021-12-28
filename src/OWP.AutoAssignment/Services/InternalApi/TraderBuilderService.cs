using OWP.AutoAssignment.Criteria.InternalApi;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Utils.XMLExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OWP.AutoAssignment.Services.InternalApi
{

    public interface TraderBuilderService
    {
        Task Build(
            IList<ExportMatchModel> models,
            IList<AutoAssignmentResultModel> autoAssignModels);
    }

    public class TraderBuilderServiceImp : TraderBuilderService
    {
        private readonly Repository repository;
        public TraderBuilderServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task Build(
            IList<ExportMatchModel> models,
            IList<AutoAssignmentResultModel> autoAssignModels)
        {
            var traderQueryInfo = BuildTraderExportQueryInfo(autoAssignModels);
            var traderExportInfo = await QueryTraderExportInfo(traderQueryInfo);
            BuildExportTraderModel(models, traderExportInfo.ToList());

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
            IList<TraderExportInfo> traderExportModel)
        {
            foreach (var exportMatchModel in exportModels)
            {
                var traderInfo = GetSingleTraderInfoById(traderExportModel, exportMatchModel);
                exportMatchModel.Trader1HHDP = traderInfo?.Trader1HHDP;
                exportMatchModel.Trader1HOU = traderInfo?.Trader1HOU;
                exportMatchModel.TraderHDP = traderInfo?.TraderFTHDP;
                exportMatchModel.TraderOU = traderInfo?.TraderFTOU;
            }
        }

        private TraderExportInfo GetSingleTraderInfoById(IList<TraderExportInfo> traderExportModel,
            ExportMatchModel exportMatchModel)
        {
            var id = exportMatchModel.InfoSiteMatchId;
            var bettype = exportMatchModel.Bettype;
            return traderExportModel
                .FirstOrDefault(x => x.InfoSiteMatchId == id && x.Bettype == bettype);
        }


    }
}