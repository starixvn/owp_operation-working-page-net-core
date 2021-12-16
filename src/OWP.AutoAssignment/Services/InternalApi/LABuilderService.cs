using OWP.AutoAssignment.Criteria.InternalApi;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Utils.XMLExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OWP.AutoAssignment.Services.InternalApi
{
    public interface LABuilderService
    {
        Task Build(
            IList<ExportMatchModel> models,
            IList<AutoAssignmentResultModel> autoAssignModels);
    }

    public class LABuilderServiceImp : LABuilderService
    {
        private readonly Repository repository;

        public LABuilderServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task Build(
            IList<ExportMatchModel> models,
            IList<AutoAssignmentResultModel> autoAssignModels)
        {
            var lAQueryInfo = BuildLAExportQueryInfo(autoAssignModels);
            var lAExportInfo = await QueryLAExportInfo(lAQueryInfo);

            // TODO UPDATE lAExportInfo TO ExportMatchModel;
        }

        private IList<LAExportQueryInfo> BuildLAExportQueryInfo(IList<AutoAssignmentResultModel> autoAssignModels)
        => autoAssignModels
                .Select(model => new LAExportQueryInfo
                {
                    InfoSiteMatchId = model.InfoSiteMatchId,
                   
                    Bettype = model.Bettype
                }).ToList();

        private async Task<IEnumerable<LAExportInfo>> QueryLAExportInfo(IList<LAExportQueryInfo> queryInfo)
        {
            var criteria = new GetLAExportInfoCriteria
            {
                Xml = queryInfo.ToXml().ToString(SaveOptions.DisableFormatting)
            };

            return await repository.GetLAExportInfo(criteria);
        }

    }
}