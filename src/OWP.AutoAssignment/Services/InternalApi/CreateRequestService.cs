using OWP.AutoAssignment.Criteria.InternalApi;
using OWP.AutoAssignment.Criteria.PublicApi;
using OWP.AutoAssignment.Enum;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Services.PublicApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.InternalApi
{
    public interface CreateRequestService
    {
        Task<int> GetRequestStatus(string requestId);

        Task CreateRequest(CreateRequestModel model);

        Task<bool> ExistMatchValid(DateTime eventDate);

        Task<bool> ExistsSuspenderValid(DateTime eventDate);

        Task<string> GetRequestIsWaiting(DateTime eventDate);

        Task<IList<SchedulingRequest>> GetRequests();
    }

    public class CreateRequestServiceImp : CreateRequestService
    {
        private readonly Repository repository;

        private readonly SchedulingMatchBuilderService schedulingMatchBuilderService;

        private readonly LAUserService lAUserService;

        private readonly TraderUserService traderUserService;

        public CreateRequestServiceImp(
            Repository repository,
            SchedulingMatchBuilderService schedulingMatchBuilderService,
            LAUserService lAUserService,
            TraderUserService traderUserService
            )
        {
            this.repository = repository;
            this.schedulingMatchBuilderService = schedulingMatchBuilderService;
            this.lAUserService = lAUserService;
            this.traderUserService = traderUserService;
        }

        public async Task CreateRequest(CreateRequestModel model)
        {
            var criteria = new CreateRequestCriteria
            {
                RequestId = Guid.NewGuid().ToString(),
                EventDate = model.EventDate,
                Status = StatusRequest.Create,
                CreatorId = model.CreatorId
            };
            await repository.CreateRequest(criteria);
        }

        public async Task<bool> ExistMatchValid(DateTime eventDate)
        {
            var match = await schedulingMatchBuilderService.GetMatchesInfo(eventDate);
            return match.Real.Any() || match.Saba.Any();
        }

        public async Task<bool> ExistsSuspenderValid(DateTime eventDate)
        {
            var existsLaUser = await lAUserService.ExistsLAUserValid(eventDate);
            var existsTraderUser = await traderUserService.ExistsTraderValid(eventDate);
            return existsLaUser || existsTraderUser;
        }

        public async Task<string> GetRequestIsWaiting(DateTime eventDate)
        {
            var requestIsWaiting = await repository.GetRequestIsWaiting(eventDate);
            return requestIsWaiting;
        }

        public async Task<IList<SchedulingRequest>> GetRequests()
        {
            var allRequest = await repository.GetRequestsInternal();
            return allRequest.Any()
                ? allRequest.OrderByDescending(r => r.CreatedDate).ToList()
                : new List<SchedulingRequest>();
        }

        public async Task<int> GetRequestStatus(string requestId)
            => await repository.GetRequestStatus(requestId);
       
    }
}
