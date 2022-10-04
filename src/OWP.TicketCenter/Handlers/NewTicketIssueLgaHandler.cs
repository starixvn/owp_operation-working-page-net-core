using MediatR;
using AutoMapper;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers;

public class NewTicketIssueLgaRequest : NewTicketModelBase, IRequest<int>
{
    public int OwpMatchId { get; set; }
}
public class NewTicketIssueLgaHandler : IRequestHandler<NewTicketIssueLgaRequest, int>
{
    private readonly Repository repository;
    private readonly RecipientService recipientService;
    private readonly IMapper mapper;

    public NewTicketIssueLgaHandler(Repository repository, RecipientService recipientService, IMapper mapper)
    {
        this.repository = repository;
        this.recipientService = recipientService;
        this.mapper = mapper;
    }

    public async Task<int> Handle(NewTicketIssueLgaRequest request, CancellationToken cancellationToken)
    {
        var ticketId = await repository.BuildTicketId(request.TicketType, request.SubType);
        request.TicketId = ticketId;
        await repository.AddNewTicketIssueLga(request);
        var messages = new List<NewMessageRequestModel>();
        foreach (var file in request.Files)
        {
            messages.Add(new()
            {
                TicketId = ticketId,
                CreatorId = request.CreatorId,
                Message = file.Title,
                FileId = file.FileId
            });
        }
        var newMessage = mapper.Map<NewMessageRequestModel>(request);
        messages.Add(newMessage);

        foreach (var message in messages)
        {
            await repository.AddNewMessage(message);
        }
        var criteria = new InsertIssueCreatorCriteria
        {
            TicketId = ticketId,
            CreatorId = request.CreatorId
        };

        await repository.InsertIssueCreator(criteria);

        await recipientService.InsertManyRecipient(request.Recipients, request.TicketId);

        await recipientService.InsertManyRecipientReader(request.Recipients, request.TicketId, request.CreatorId);

        return ticketId;
    }
}