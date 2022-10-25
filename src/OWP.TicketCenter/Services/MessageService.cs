using AutoMapper;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Services;

public interface MessageService
{
    Task<NewMessageRequestModel> InsertMessage(NewTicketModelBase request, int ticketId);
}

public class MessageServiceImp : MessageService
{
    private readonly Repository repository;

    private readonly IMapper mapper;

    public MessageServiceImp(Repository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<NewMessageRequestModel> InsertMessage(NewTicketModelBase request, int ticketId)
    {
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

        return newMessage;
    }
}
