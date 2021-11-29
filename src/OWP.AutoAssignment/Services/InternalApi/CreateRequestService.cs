using OWP.AutoAssignment.Criteria.InternalApi;
using OWP.AutoAssignment.Enum;
using OWP.AutoAssignment.Models.InternalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.InternalApi
{
    public interface CreateRequestService
    {
        Task CreateRequest(CreateRequestModel model);

        Task<bool> ExistMatchValid(CreateRequestModel model);

        Task<bool> ExistsTraderValid(CreateRequestModel model);

        Task<string> GetRequestIsWaiting(CreateRequestModel model);

        Task<IList<SchedulingRequest>> GetRequests();
    }

    public class CreateRequestServiceImp : CreateRequestService
    {
        private readonly Repository repository;
       

        public CreateRequestServiceImp(
            Repository repository
            )
        {
            this.repository = repository;
           
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

        public Task<bool> ExistMatchValid(CreateRequestModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsTraderValid(CreateRequestModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetRequestIsWaiting(CreateRequestModel model)
        {
            var requestIsWaiting = await repository.GetRequestIsWaiting(model.EventDate);
            return requestIsWaiting;
        }

        public async Task<IList<SchedulingRequest>> GetRequests()
        {
            var allRequest = await repository.GetRequestsInternal();
            return allRequest.Any()
                ? allRequest.OrderByDescending(r => r.CreatedDate).ToList()
                : new List<SchedulingRequest>();
        }
    }
}
