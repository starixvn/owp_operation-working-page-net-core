namespace OWP.OtherSport.Submit
{
    using Fanex.NAP.Client.Helpers.Web;
    using MediatR;
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
        }

        public string Message { get; set; }

        public bool IsSuccessful => string.IsNullOrEmpty(Message);
    }

    public class SubmitHandler: IRequestHandler<SubmitRequest, SubmitResponse>
    {
        private readonly CacheManipulator cacheManipulator;
        private readonly CommandRepository commandRepository;
        private readonly INapSession session;

        public SubmitHandler(
            CacheManipulator cacheManipulator,
            CommandRepository commandRepository,
            INapSession session)
        {
            this.cacheManipulator = cacheManipulator;
            this.commandRepository = commandRepository;
            this.session = session;
        }

        public async Task<SubmitResponse> Handle(SubmitRequest request, CancellationToken cancellationToken)
        {
            var newAssignments = await cacheManipulator.GetNewAssignments(request.SessionId);
            var eventDates = newAssignments.Select(a => a.DateTime).Distinct().ToList();

            var userWhoIsUploadingAssignment = await GetUserWhoIsUploadingAssignment(eventDates);
            if (!userWhoIsUploadingAssignment.IsSuccessful)
            {
                return userWhoIsUploadingAssignment;
            }

            await TryHandle(eventDates, newAssignments, request.SessionId);

            return new SubmitResponse();
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
                        Message = $"User {userWhoIsUploadingAssignment} is uploading another assignment file for date {eventDate.ToShortDateString()}. Please try again later!"
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
