
using Fanex.NAP.Client.Helpers.Web;
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

    public Task<UserFeedbackInfo> Get()
    {
        var criteria = new GetUserFeedbackInfoCriteria
        {
            sessionId = session.UserId
        };
        return repository.GetUserFeedbackInfo(criteria);
    }
}
