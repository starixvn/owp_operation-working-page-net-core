namespace OWP.OtherSport.Submit
{
    using Fanex.NAP.Client.Helpers.Web;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;
    using OWP.OtherSport.Hubs;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Submit.Services;
    using OWP.OtherSport.Validation.Models;
    using OWP.Utils;

    public class SubmitRequest : IRequest<SubmitResponse>
    {
        public string SessionId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class SubmitResponse
    {
        public SubmitResponse()
        {
            Message = string.Empty;
            IsSuccessful = true;
        }

        public string Message { get; set; }

        public bool IsSuccessful { get; set; }
    }

    public class SubmitHandler : IRequestHandler<SubmitRequest, SubmitResponse>
    {
        private readonly CacheManipulator cacheManipulator;
        private readonly CommandRepository commandRepository;
        private readonly INapSession session;
        private readonly IHubContext<NewDataUploadNotifyHubImp, NewDataUploadNotifyHub> notifyHub;

        public SubmitHandler(
            CacheManipulator cacheManipulator,
            CommandRepository commandRepository,
            INapSession session,
            IHubContext<NewDataUploadNotifyHubImp, NewDataUploadNotifyHub> notifyHub)
        {
            this.cacheManipulator = cacheManipulator;
            this.commandRepository = commandRepository;
            this.session = session;
            this.notifyHub = notifyHub;
        }

        public async Task<SubmitResponse> Handle(SubmitRequest request, CancellationToken cancellationToken)
        {
            var newAssignments = await cacheManipulator.GetNewAssignments(request.SessionId);
            var eventDates = new List<DateTime>();
            var diffDays = request.ToDate - request.FromDate;

            for (int i = 0; i <= diffDays.TotalDays; i++)
            {
                eventDates.Add(request.FromDate.AddDays(i));
            }

            var userWhoIsUploadingAssignment = await GetUserWhoIsUploadingAssignment(eventDates);
            if (!userWhoIsUploadingAssignment.IsSuccessful)
            {
                return userWhoIsUploadingAssignment;
            }

            await TryHandle(eventDates, newAssignments, request);
            await notifyHub.Clients.All.NotifyUploadedNewData();

            return new SubmitResponse
            {
                IsSuccessful = true,
                Message = "Other sport assignment is successfully submitted!"
            };
        }

        private async Task<SubmitResponse> GetUserWhoIsUploadingAssignment(List<DateTime> eventDates)
        {
            foreach (var eventDate in eventDates)
            {
                var userWhoIsUploadingAssignment = await cacheManipulator.GetUserWhoIsUploadingAssignment(eventDate);
                if (!string.IsNullOrEmpty(userWhoIsUploadingAssignment))
                {
                    return new SubmitResponse
                    {
                        IsSuccessful = false,
                        Message = $"User {userWhoIsUploadingAssignment} is uploading another assignment file for date {eventDate:MMM dd, yyyy}. Please try again later!"
                    };
                }
            }

            return new SubmitResponse();
        }

        private async Task TryHandle(List<DateTime> eventDates, List<AssignmentModel> assignments, SubmitRequest request)
        {
            try
            {
                await cacheManipulator.SaveUserWhoIsUploadingAssignment(eventDates, session.UserName);

                await SaveDataAndCleanupCache(request, assignments);

                await cacheManipulator.SaveLastUpdate(
                    new LastUpdateInfo
                    {
                        Username = session.UserName,
                        Date = DateTimeExtensions.ToGmt8(DateTime.Now)
                    });
            }
            finally
            {
                await cacheManipulator.DeleteUserWhoIsUploadingAssignment(eventDates);
            }
        }

        private async Task SaveDataAndCleanupCache(SubmitRequest request, List<AssignmentModel> assignments)
        {
            var oldMatch = await commandRepository.GetOldMatchInfo(request.FromDate, request.ToDate);
            var oldMatchIds = oldMatch.Select(x => x.OSMatchId).Distinct().ToList();

            var hashKeyMap = await commandRepository.InsertOrUpdateMatch(assignments);
            var newMatchIds = hashKeyMap.Select(h => h.OSMatchId).Distinct().ToList();

            var deletedMatchIds = oldMatchIds.Except(newMatchIds);
            var deletedInfo = oldMatch.Where(x => deletedMatchIds.Any(y => x.OSMatchId.Equals(y))).ToList();
            var deletedMatchIdsInString = string.Join(",", deletedInfo.Select(i => i.OSMatchId));

            await SaveDeletedMatchesToSnapshot(deletedInfo);
            await SaveOldAssignmentsToSnapshot(request.FromDate, request.ToDate, request.SessionId);
            await SaveAssignmentsToDatabase(assignments, deletedMatchIdsInString);
            await cacheManipulator.DeleteAssignments(request.SessionId);
        }

        public async Task SaveOldAssignmentsToSnapshot(DateTime fromDate, DateTime toDate, string sessionId)
        {
            var oldSnapshot = await commandRepository.Query(fromDate, toDate);
            await cacheManipulator.SaveOldAssignmentSnapshot(oldSnapshot, sessionId);
        }

        public async Task SaveAssignmentsToDatabase(List<AssignmentModel> newAssignments, string deletedMatchIds)
            => await commandRepository.Update(deletedMatchIds, newAssignments);

        public async Task SaveDeletedMatchesToSnapshot(List<DeletedMatch> deletedMatches)
            => await cacheManipulator.SaveDeletedMatchesSnapshot(deletedMatches);
    }
}