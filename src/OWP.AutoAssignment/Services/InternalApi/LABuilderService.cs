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

    public interface LABuilderService
    {
        Task Build(
            IList<ExportMatchModel> models,
            IList<AutoAssignmentResultModel> autoAssignModels);
    }

    public class LABuilderServiceImp : LABuilderService
    {
        private readonly IMapper mapper;
        private readonly Repository repository;

        public LABuilderServiceImp(Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task Build(
            IList<ExportMatchModel> models,
            IList<AutoAssignmentResultModel> autoAssignModels)
        {
            var lAQueryInfo = BuildLAExportQueryInfo(autoAssignModels);
            var lAExportInfo = (await QueryLAExportInfo(lAQueryInfo)).ToList();
            foreach (var exportMatchModel in models)
            {
                
                exportMatchModel.LA = lAExportInfo
                    .FirstOrDefault(x => x.InfoSiteMatchId == exportMatchModel.InfoSiteMatchId)
                    ?.UserName;
            }

        }

        private IList<LAExportQueryInfo> BuildLAExportQueryInfo(IList<AutoAssignmentResultModel> autoAssignModels)
        => autoAssignModels
                .Select(model => new LAExportQueryInfo
                {
                    InfoSiteMatchId = model.InfoSiteMatchId,
                    LAAssingeeId = model.UserId_LA,
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