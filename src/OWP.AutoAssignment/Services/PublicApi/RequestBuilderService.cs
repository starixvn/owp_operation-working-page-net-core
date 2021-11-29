using OWP.AutoAssignment.Enum;
using OWP.AutoAssignment.Models.PublicApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.PublicApi
{
    public interface RequestBuilder
    {
        Task<IEnumerable<RequestSender>> BuildRequestSenderModel(IEnumerable<RequestModel> request);
    }

    public class RequestBuilderImp : RequestBuilder
    {
        private readonly IList<RequestSender> sender = new List<RequestSender>();
        private IList<SchedulingMatch> schedulingMatch = new List<SchedulingMatch>();
        private IList<SchedulingLAUser> schedulingLAUser = new List<SchedulingLAUser>();
        private IList<TraderInfo> traderInfos = new List<TraderInfo>();

        private readonly Repository repository;
        private readonly SchedulingMatchBuilderService matchService;
        private readonly LAUserService laService;
        private readonly TraderUserService traderService;

        public RequestBuilderImp(
           Repository repository,
           SchedulingMatchBuilderService matchService,
           LAUserService laService,
           TraderUserService traderService)
        {
            this.repository = repository;
            this.matchService = matchService;
            this.laService = laService;
            this.traderService = traderService;
        }

        public async Task<IEnumerable<RequestSender>> BuildRequestSenderModel(IEnumerable<RequestModel> request)
        {
            foreach (var req in request)
            {
                schedulingMatch = await matchService.GetMatchesInfo(req.EventDate);
                schedulingLAUser = await laService.GetLAUsersInfo(req.EventDate);
                traderInfos = await traderService.GetTradersInfo(req.EventDate);

                if (schedulingMatch.Any() && (schedulingLAUser.Any() || traderInfos.Any()))
                {
                    sender.Add(BuildRequestSender(req, "New"));
                    await UpdateStatusRequest(req, StatusRequest.Running);
                }
                else
                {
                    await UpdateStatusRequest(req, StatusRequest.Success);
                }
            }
            return sender;
        }

        private async Task UpdateStatusRequest(RequestModel requestModel, StatusRequest status)
            => await repository.UpdateStatusRequest(new StatusRequestModel
            {
                RequestId = requestModel.RequestId,
                Status = status
            });

        private RequestSender BuildRequestSender(RequestModel model, string action)
        {
            return new RequestSender
            {
                Action = action,
                RequestId = model.RequestId,
                DataHolder = BuildDataHolder(),
                EventDate = model.EventDate
            };
        }

        private DataHolder BuildDataHolder()
           => new DataHolder
           {
               Matches = schedulingMatch,
               Suspenders = schedulingLAUser,
               Traders = traderInfos
           };
    }
}