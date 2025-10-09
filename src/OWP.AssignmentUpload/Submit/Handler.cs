
using Fanex.NAP.Client;
using MediatR;
using OWP.AssignmentUpload.Submit.Models;
using OWP.AssignmentUpload.Submit.Services;

namespace OWP.AssignmentUpload.Submit;

public class SubmitRequest : IRequest<SubmitResponse>
{
    public string SessionId { get; set; }

    public DateTime EventDate { get; set; }

    public bool IsUploadFMAndAIO { get; set; }
}

public class SubmitResponse
{
    public string Message { get; set; } = string.Empty;

    public bool IsSuccessful { get; set; } = true;
}

public class Handler(
    QueryRepository queryRepository,
    CacheManipulator cacheManipulator,
    CommandRepository commandRepository,
    INapSession currentSession)
    : IRequestHandler<SubmitRequest, SubmitResponse>
{
    public async Task<SubmitResponse> Handle(SubmitRequest request, CancellationToken cancellationToken)
    {
        var userWhoIsUploadingAssignment = await cacheManipulator.GetUserWhoIsUploadingAssignment(request.EventDate);
        if (!string.IsNullOrEmpty(userWhoIsUploadingAssignment))
        {
            return new SubmitResponse
            {
                Message = $"User {userWhoIsUploadingAssignment} is uploading another assignment file for date  {request.EventDate:MMM dd, yyyy}. Please try again later!",
                IsSuccessful = false
            };
        }

        await TryHandle(request);

        return new SubmitResponse
        {
            IsSuccessful = true,
            Message = "Event assignment is successfully submitted!"
        };
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