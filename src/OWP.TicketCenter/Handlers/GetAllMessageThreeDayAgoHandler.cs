using MediatR;
using OWP.TicketCenter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers;

public class GetAllMessageThreeDayRequest : IRequest<IEnumerable<MessageDetailViewModel>>
{
    public int TicketTypeId { get; set; }

    public int? SubTypeId { get; set; }

}

public class GetAllMessageThreeDayAgoHandler(MessageService messageService) : IRequestHandler<GetAllMessageThreeDayRequest, IEnumerable<MessageDetailViewModel>>
{
    public async Task<IEnumerable<MessageDetailViewModel>> Handle(GetAllMessageThreeDayRequest request, CancellationToken cancellationToken)
    {
        var result = await messageService.GetAllMessageThreeDayAgo(request.TicketTypeId, request.SubTypeId);
        return result;
    }
}
