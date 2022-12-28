using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers;

public class GetNextListMessagesRequest : IRequest<(bool, IEnumerable<MessageDetailViewModel>)>
{
    public int MsgOfPage { get; set; }

    public int TicketId { get; set; }

    public int? FirstMsgId { get; set; }
}

public class GetNextListMessagesHandler : IRequestHandler<GetNextListMessagesRequest, (bool, IEnumerable<MessageDetailViewModel>)>
{
    private readonly Repository repository;

    public GetNextListMessagesHandler(Repository repository)
    {
        this.repository = repository;
    }

    public Task<(bool, IEnumerable<MessageDetailViewModel>)> Handle(GetNextListMessagesRequest request,
        CancellationToken cancellationToken)
        => repository.GetNextListMessages(new GetNextListMessageCriteria
        {
            TicketId = request.TicketId,
            FirstMsgId = request.FirstMsgId,
            MsgOfPage = request.MsgOfPage
        });
}

