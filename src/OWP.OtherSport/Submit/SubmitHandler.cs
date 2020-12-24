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

    public class SubmitRequest: IRequest<string>
    {
        public string SessionId { get; set; }
    }

    public class SubmitHandler: IRequestHandler<SubmitRequest, string>
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

        public async Task<string> Handle(SubmitRequest request, CancellationToken cancellationToken)
        {
            var newAssignments = await cacheManipulator.GetNewAssignments(request.SessionId);
            var eventDates = newAssignments.Select(a => a.DateTime).Distinct().ToList();

            var userWhoIsUploadingAssignment = await cacheManipulator.GetUserWhoIsUploadingAssignment(eventDates);
            if (!string.IsNullOrEmpty(userWhoIsUploadingAssignment))
            {
                return userWhoIsUploadingAssignment;
            }

            await TryHandle(eventDates, newAssignments, request.SessionId);

            return string.Empty;
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
