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
            var newAssignments = await cacheManipulator.GetNewAssignments();
            var eventDates = newAssignments.Select(a => a.EventDate).Distinct().ToList();

            var userWhoIsUploadingAssignment = await cacheManipulator.GetUserWhoIsUploadingAssignment(eventDates);
            if (!string.IsNullOrEmpty(userWhoIsUploadingAssignment))
            {
                return userWhoIsUploadingAssignment;
            }

            await TryHandle(eventDates, newAssignments);

            return string.Empty;
        }

        private async Task TryHandle(List<DateTime> eventDates, List<AssignmentModel> assignments)
        {
            try
            {
                await cacheManipulator.SaveUserWhoIsUploadingAssignment(eventDates, session.UserName);
                await SaveDataAndCleanupCache(eventDates, assignments);
            }
            finally
            {
                await cacheManipulator.DeleteUserWhoIsUploadingAssignment(eventDates);
            }
        }

        private async Task SaveDataAndCleanupCache(List<DateTime> eventDates, List<AssignmentModel> assignments)
        {
            var eventDateInString = string.Join(',', eventDates);

            await SaveOldAssignmentsToSnapshot(eventDateInString);
            await SaveAssignmentsToDatabase(assignments, eventDateInString);
        }

        public async Task SaveOldAssignmentsToSnapshot(string eventDateInString)
        {
            var oldSnapshot = await commandRepository.Query(eventDateInString);
            await cacheManipulator.SaveOldAssignmentSnapshot(oldSnapshot);
        }

        public async Task SaveAssignmentsToDatabase(List<AssignmentModel> newAssignments, string eventDateInString)
            => await commandRepository.Update(eventDateInString, newAssignments);
    }
}
