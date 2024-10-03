
using Fanex.NAP.Client;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Services;
public interface GetInfoFeedback
{
    Task<UserFeedbackInfo> Get();
}

public class GetInfoFeedbackImp: GetInfoFeedback
{
    private readonly INapSession session;
    private readonly Repository repository;

    public GetInfoFeedbackImp(INapSession session, Repository repository)
    {
        this.session = session;
        this.repository = repository;
    }

    public async Task<UserFeedbackInfo> Get()
    {
        var criteria = new GetUserFeedbackInfoCriteria
        {
            sessionId = session.UserId
        };
        return await repository.GetUserFeedbackInfo(criteria);
    }
}
