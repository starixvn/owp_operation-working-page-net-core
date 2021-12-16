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

        public MatchInfoBuilderServiceImp(Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        public async Task Build(IList<ExportMatchModel> models, IList<AutoAssignmentResultModel> autoAssignModels)
        {
            var matchQueryInfo = BuildMatchQueryInfo(autoAssignModels);
            var matchExportModels = await QueryMatchInfo(matchQueryInfo);
            BuildExportMatchModel(models, matchExportModels.ToList());
        }

        private IList<MatchQueryInfo> BuildMatchQueryInfo(IList<AutoAssignmentResultModel> autoAssignModels)
        => autoAssignModels
                .Select(model => new MatchQueryInfo
                {
                    InfoSiteMatchId = model.InfoSiteMatchId,
                    BetType = model.Bettype
                }).ToList();

        private async Task<IEnumerable<InfoSiteMatchModel>> QueryMatchInfo(IList<MatchQueryInfo> queryInfo)
            => await repository.GetInfoSiteMatchInfo(queryInfo);

        private void BuildExportMatchModel(IList<ExportMatchModel> exportModels,
            List<InfoSiteMatchModel> matchModels)
            => matchModels.ForEach(m =>
            {
                var matchInfo = mapper.Map<ExportMatchModel>(m);
                exportModels.Add(matchInfo);
            });
    }
}
