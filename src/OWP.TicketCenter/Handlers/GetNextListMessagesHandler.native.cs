using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers;

public class GetNextListMessagesRequest : IRequest<ShowMoreMessageModel>
{
    public int MsgOfPage { get; set; }

    public int TicketId { get; set; }

    public int? FirstMsgId { get; set; }
}

public class GetNextListMessagesHandler : IRequestHandler<GetNextListMessagesRequest, ShowMoreMessageModel>
{
    private readonly Repository repository;
    private readonly MessageService messageService;

    public GetNextListMessagesHandler(Repository repository, MessageService messageService)
    {
        this.repository = repository;
        this.messageService = messageService;
    }

    public async Task<ShowMoreMessageModel> Handle(GetNextListMessagesRequest request,
        CancellationToken cancellationToken)
    {
        var (canShowMoreMessage, messages) = await repository.GetNextListMessages(new GetNextListMessageCriteria
        {
            TicketId = request.TicketId,
            FirstMsgId = request.FirstMsgId,
            MsgOfPage = request.MsgOfPage
        });

        await messageService.BuildReactIconForMessage(messages);

        return new ShowMoreMessageModel
        {
            CanShowMoreMessage = canShowMoreMessage,
            Messages = messages
        };
    }
}

