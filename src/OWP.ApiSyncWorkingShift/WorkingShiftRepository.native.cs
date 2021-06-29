using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using OWP.ApiSyncWorkingShift.Criteria;
using OWP.ApiSyncWorkingShift.Models;

namespace OWP.ApiSyncWorkingShift
{
    public interface WorkingShiftRepository
    {
        Task<IEnumerable<WorkingShiftModel>> Query(QueryWorkingShiftRequest request);
    }

    public class WorkingShiftRepositoryImp : WorkingShiftRepository
    {
        private readonly IDynamicRepository dynamicRepository;

        public WorkingShiftRepositoryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<WorkingShiftModel>> Query(QueryWorkingShiftRequest request)
            => await dynamicRepository.FetchAsync<WorkingShiftModel>(new GetEmployeeWorkingShiftCriteria
            {
                FromDate = request.From,
                ToDate = request.To
            });
    }
}
