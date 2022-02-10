namespace OWP.PreMatchReport
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Data.Repository;
    using OWP.PreMatchReport.Models;

    public interface Repository
    {
        Task<IEnumerable<QueryPrematchReportModel>> GetReportForLeague(QueryModel queryModel);
    }

    public class RepositoryImp : Repository
    {
        private readonly IDynamicRepository dynamicRepository;
        public RepositoryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<QueryPrematchReportModel>> GetReportForLeague(QueryModel queryModel)
        {
            var criteria = new GetReportForLeagueCriteria
            {
                FromDate = queryModel.FromDate,
                ToDate = queryModel.ToDate,
                KeyWord = queryModel.KeyWord
            };
            return await dynamicRepository.FetchAsync<QueryPrematchReportModel>(criteria);
        }
    }
}