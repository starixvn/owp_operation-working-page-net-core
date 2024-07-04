using MediatR;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;


namespace OWP.TicketCenter.Handlers;

public class GetUserInfoFeedbackRequest : IRequest<UserFeedbackInfo>
{

}

public class GetUserInfoFeedbackHandler: IRequestHandler<GetUserInfoFeedbackRequest, UserFeedbackInfo>
{
    private readonly GetInfoFeedback serviceFeedback;

    public GetUserInfoFeedbackHandler(GetInfoFeedback serviceFeedback)
    {
        this.serviceFeedback = serviceFeedback;
    }
    public async Task<UserFeedbackInfo> Handle(GetUserInfoFeedbackRequest request, CancellationToken cancellationToken)
    {
        return await serviceFeedback.Get();
    }
}
