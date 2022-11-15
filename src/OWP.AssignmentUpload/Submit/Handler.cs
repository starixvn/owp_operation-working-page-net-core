
namespace OWP.AssignmentUpload.Submit
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.NAP.Client.Helpers.Web;
    using MediatR;
    using Models;
    using Services;

    public class SubmitRequest : IRequest<string>
    {
        public string SessionId { get; set; }

        public DateTime EventDate { get; set; }

        public bool IsUploadFMAndAIO { get; set; }
    }

    public class Handler : IRequestHandler<SubmitRequest, string>
    {
        private readonly QueryRepository queryRepository;

        private readonly CommandRepository commandRepository;

        private readonly CacheManipulator cacheManipulator;

        private readonly INapSession currentSession;

        public Handler(
            QueryRepository queryRepository,
            CacheManipulator cacheManipulator,
            CommandRepository commandRepository,
            INapSession currentSession)
        {
            this.queryRepository = queryRepository;
            this.cacheManipulator = cacheManipulator;
            this.commandRepository = commandRepository;
            this.currentSession = currentSession;
        }

        public async Task<string> Handle(SubmitRequest request, CancellationToken cancellationToken)
        {
            var userWhoIsUploadingAssignment = await cacheManipulator.GetUserWhoIsUploadingAssignment(request.EventDate);
            if (!string.IsNullOrEmpty(userWhoIsUploadingAssignment))
            {
                return userWhoIsUploadingAssignment;
            }

            await TryHandle(request);

            return string.Empty;
        }

        private async Task TryHandle(SubmitRequest request)
        {
            try
            {
                await cacheManipulator.SaveUserWhoIsUploadingAssignment(request.EventDate, currentSession.UserName);
                await SaveDataAndCleanupCache(request);
            }
            finally
            {
                await cacheManipulator.DeleteUserWhoIsUploadingAssignment(request.EventDate);
            }
        }

        private async Task SaveDataAndCleanupCache(SubmitRequest request)
        {
            await SaveAssignmentSnapshot(request);
            await SaveAssignmentsToDatabase(request);
            await cacheManipulator.DeleteAssignments(request.SessionId);
        }

        public async Task SaveAssignmentsToDatabase(SubmitRequest request)
        {
            var oldMatchIds = await GetOldMatchIds(request);
            var assignments = await cacheManipulator.GetAssignments(request.SessionId);
            await commandRepository.Update(oldMatchIds, assignments, request);
        }

        private async Task SaveAssignmentSnapshot(SubmitRequest request)
        {
            var snapshot = await queryRepository.Query(request.EventDate);
            await cacheManipulator.SaveAssignmentSnapshot(request.SessionId, snapshot);
        }

        private async Task<List<OldMatch>> GetOldMatchIds(SubmitRequest request)
        {
            return await queryRepository.GetOldMatchIds(request.EventDate);
        }
    }
}
