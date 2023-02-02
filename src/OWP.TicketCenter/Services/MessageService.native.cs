using AutoMapper;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services;

public interface MessageService
{
    Task<NewMessageRequestModel> InsertMessage(NewTicketModelBase request, int ticketId);

    Task<(bool, List<MessageDetailViewModel>)> GetMessageListInLast2Days(int ticketId);

    Task<(bool, List<MessageDetailViewModel>)> GetMessageListByGreaterMessageId(int ticketId, int firstMessageId);
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

    public async Task<(bool, List<MessageDetailViewModel>)> GetMessageListInLast2Days(int ticketId)
    {
        var (canShowMoreMessage, messages) = await repository.GetMessageInLastTwoDays(ticketId);

        return (canShowMoreMessage, messages
            .DistinctBy(m => new { m.UserId, m.Username, m.CreatedDate, m.MessageId, m.Message, m.IsSaved })
            .OrderBy(m => m.CreatedDate)
            .ToList());
    }

    public async Task<(bool, List<MessageDetailViewModel>)> GetMessageListByGreaterMessageId(int ticketId, int firstMessageId)
    {
        var (canShowMoreMessage, messages) = await repository.GetMessageListByGreaterMessageId(ticketId, firstMessageId);

        return (canShowMoreMessage, messages
            .DistinctBy(m => new { m.UserId, m.Username, m.CreatedDate, m.MessageId, m.Message, m.IsSaved })
            .OrderBy(m => m.CreatedDate)
            .ToList());
    }
}
