namespace OWP.OtherSport.Submit
{
    using Fanex.NAP.Client.Helpers.Web;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;
    using OWP.OtherSport.Hubs;
    using OWP.OtherSport.Submit.Services;
    using OWP.OtherSport.Validation.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class SubmitRequest: IRequest<SubmitResponse>
    {
        public string SessionId { get; set; }
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

    public class SubmitHandler: IRequestHandler<SubmitRequest, SubmitResponse>
    {
        private readonly CacheManipulator cacheManipulator;
        private readonly CommandRepository commandRepository;
        private readonly INapSession session;
        private readonly IHubContext<NewDataUploadNotifyHubImp, NewDataUploadNotifyHub> notifyHub;
        private readonly Notification.Services.NotifyServices notifyServices;

        public SubmitHandler(
            CacheManipulator cacheManipulator,
            CommandRepository commandRepository,
            INapSession session,
            IHubContext<NewDataUploadNotifyHubImp, NewDataUploadNotifyHub> notifyHub,
            Notification.Services.NotifyServices notifyServices)
        {
            this.cacheManipulator = cacheManipulator;
            this.commandRepository = commandRepository;
            this.session = session;
            this.notifyHub = notifyHub;
            this.notifyServices = notifyServices;
        }

        public async Task<SubmitResponse> Handle(SubmitRequest request, CancellationToken cancellationToken)
        {
            var newAssignments = await cacheManipulator.GetNewAssignments(request.SessionId);
            var eventDates = newAssignments.Select(a => a.DateTime.Date).Distinct().ToList();

            var userWhoIsUploadingAssignment = await GetUserWhoIsUploadingAssignment(eventDates);
            if (!userWhoIsUploadingAssignment.IsSuccessful)
            {
                return userWhoIsUploadingAssignment;
            }

            await TryHandle(eventDates, newAssignments, request.SessionId);
            await notifyHub.Clients.All.NotifyUploadedNewData();

            var messages = new List<Notification.Models.Message>
            {
                new(){
                    MessageType = 101,
                    MessageContent = "You have NEW ASSIGNMENT bettype in #555 - League - Event updated by #sa",
                    IsImportant = true,
                    ReceivedUserIdList = new List<string> { "8", "360418" }
                },
                new(){
                    MessageType = 103,
                    MessageContent = "You have been REMOVED <b>bettype</b> in #555 - League - Event updated by #sa",
                    IsImportant = true,
                    ReceivedUserIdList = new List<string> { "8", "360418" }
                },
                new(){
                    MessageType = 108,
                    MessageContent = "Match Status Updated from Postponed to Live",
                    IsImportant = true,
                    ReceivedUserIdList = new List<string> { "8", "360418" }
                }
            };
            await notifyServices.Notify(messages);

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

        private async Task TryHandle(List<DateTime> eventDates, List<AssignmentModel> assignments, string sessionId)
        {
            try
            {
                await cacheManipulator.SaveUserWhoIsUploadingAssignment(eventDates, session.UserName);
                await SaveDataAndCleanupCache(eventDates, assignments, sessionId);
            }
            finally
            {
                await cacheManipulator.DeleteUserWhoIsUploadingAssignment(eventDates);
            }
        }

        private async Task SaveDataAndCleanupCache(List<DateTime> eventDates, List<AssignmentModel> assignments, string sessionId)
        {
            var eventDateInString = string.Join(',', eventDates);

            await SaveOldAssignmentsToSnapshot(eventDateInString, sessionId);
            await SaveAssignmentsToDatabase(assignments, eventDateInString);
        }

        public async Task SaveOldAssignmentsToSnapshot(string eventDateInString, string sessionId)
        {
            var oldSnapshot = await commandRepository.Query(eventDateInString);
            await cacheManipulator.SaveOldAssignmentSnapshot(oldSnapshot, sessionId);
        }

        public async Task SaveAssignmentsToDatabase(List<AssignmentModel> newAssignments, string eventDateInString)
            => await commandRepository.Update(eventDateInString, newAssignments);
    }
}
