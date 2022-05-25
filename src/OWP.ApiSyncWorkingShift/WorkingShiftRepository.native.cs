using Fanex.Data.Repository;
using OWP.ApiSyncWorkingShift.Criteria;
using OWP.ApiSyncWorkingShift.Handlers;
using OWP.ApiSyncWorkingShift.Models;

namespace OWP.ApiSyncWorkingShift
{
    public interface WorkingShiftRepository
    {
        Task<IEnumerable<WorkingShiftModel>> Query(QueryWorkingShiftRequest request);

        Task<IEnumerable<WorkingShiftHRPPModel>> QueryWorkingShiftHRPP(QueryWorkingShiftToHRPPRequest request);
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

        public async Task<IEnumerable<WorkingShiftHRPPModel>> QueryWorkingShiftHRPP(QueryWorkingShiftToHRPPRequest request)
            => await dynamicRepository.FetchAsync<WorkingShiftHRPPModel>(new GetEmployeeWorkingShiftHRPPCriteria
            {
                FromDate = request.From,
                ToDate = request.To
            });
    }
}