using Fanex.NAP.Client;
using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers;

public class GetAllMessageThreeDayRequest : IRequest<IEnumerable<MessageDetailViewModel>>
{
    public int TicketTypeId { get; set; }

    public int? SubTypeId { get; set; }

}

public class GetAllMessageThreeDayAgoHandler(MessageService messageService, Repository repository, INapSession session) :
    IRequestHandler<GetAllMessageThreeDayRequest, IEnumerable<MessageDetailViewModel>>
{
    public async Task<IEnumerable<MessageDetailViewModel>> Handle(GetAllMessageThreeDayRequest request, CancellationToken cancellationToken)
    {
        var result = await messageService.GetAllMessageThreeDayAgo(request.TicketTypeId, request.SubTypeId);
        await UpdateIsReadConsolidateTickets(result);

        return result;
    }

    private async Task UpdateIsReadConsolidateTickets(IEnumerable<MessageDetailViewModel> consolidateMessages)
    {
        var criteria = new UpdateSingleRecipientReaderCriteria
        {
            TicketIds = string.Join(",", consolidateMessages.Select(x => x.TicketId).Distinct()),
            IsRead = true,
            RecipientId = session.UserId
        };

        await repository.UpdateSingleRecipientReader(criteria);
    }
}
