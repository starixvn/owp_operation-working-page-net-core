
using System.Security.Cryptography.X509Certificates;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.SignalR;
using OWP.Criteria;
using OWP.Hubs;
using OWP.Models;

namespace OWP.Services;

public interface OldNotificationService
{
    Task Notify(string sessionId);

    Task Notify(string sessionId, int preMatchIssueId);

    Task Notify();
}

public class OldNotificationServiceImp(IDynamicRepository repository,
        IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> changedInfoHub)
    : OldNotificationService
{
    public async Task Notify()
    {
        var sessionIds = await repository.FetchAsync<string>(new GetNotificationQueueCriteria());
        foreach (var sessionId in sessionIds)
        {
            await NotifyInternal(sessionId);
        }
    }

    public async Task Notify(string sessionId)
    {
        await NotifyInternal(sessionId);
    }

    public async Task Notify(string sessionId, int preMatchIssueId)
    {
        await NotifyInternal(sessionId, preMatchIssueId);
    }

    private async Task NotifyInternal(string sessionId, int preMatchIssueId = 0)
    {
        var tuple = await QueryBySession(sessionId);

        var messages = tuple.Item2.Select(x => new PreMatchIssueNotificationMessage
        {
            UserId = x.UserId,
            IsImportant = x.IsImportant,
            MessageType = x.MessageType,
            MessageContent = x.MessageContent,
            PreMatchIssueId = preMatchIssueId
        }).ToList();

        foreach (var arg in messages)
        {
            await changedInfoHub.Clients.Users(arg.UserId.ToString()).notifyChangedInformationData(new { message = arg });
        }
    }

    private async Task<Tuple<IList<User>, IList<Message>>> QueryBySession(string sessionId)
    {
        var criteria = new GetMessageBySessionCriteria
        {
            SessionId = sessionId
        };

        var result = await repository.FetchMultipleAsync<User, Message>(criteria);
        return new Tuple<IList<User>, IList<Message>>(result.Item1.ToList(), result.Item2.ToList());
    }
}