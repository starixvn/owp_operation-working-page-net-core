using AutoMapper;
using Fanex.NAP.Client.Helpers.Web;
using NPOI.SS.Formula.Functions;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services;

public interface MessageService
{
    Task<NewMessageRequestModel> InsertMessage(NewTicketModelBase request, int ticketId);

    Task<(bool, List<MessageDetailViewModel>)> GetMessageListInLast2Days(int ticketId);

    Task<(bool, List<MessageDetailViewModel>)> GetMessageListByGreaterMessageId(int ticketId, int firstMessageId);

    Task BuildReactIconForMessage(IList<MessageDetailViewModel> messages);

    Task<List<MessageDetailViewModel>> GetAllMessageThreeDayAgo(int ticketTypeId, int? subTypeId);
}

public class MessageServiceImp : MessageService
{
    private readonly Repository repository;

    private readonly IMapper mapper;

    private readonly INapSession session;

    public MessageServiceImp(Repository repository, IMapper mapper, INapSession session)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.session = session;
    }

    public async Task<NewMessageRequestModel> InsertMessage(NewTicketModelBase request, int ticketId)
    {
        var messages = request.Files.Select(file =>
            new NewMessageRequestModel
            {
                TicketId = ticketId,
                CreatorId = request.CreatorId,
                Message = file.Title,
                FileId = file.FileId
            }).ToList();

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
            .DistinctBy(m => new { m.UserId, m.Username, m.CreatedDate, m.MessageId, m.Message, m.IsSaved, m.Extension })
            .OrderBy(m => m.CreatedDate)
            .ToList());
    }

    public async Task<(bool, List<MessageDetailViewModel>)> GetMessageListByGreaterMessageId(int ticketId, int firstMessageId)
    {
        var (canShowMoreMessage, messages) = await repository.GetMessageListByGreaterMessageId(ticketId, firstMessageId);

        return (canShowMoreMessage, messages
            .DistinctBy(m => new { m.UserId, m.Username, m.CreatedDate, m.MessageId, m.Message, m.IsSaved, m.Extension })
            .OrderBy(m => m.CreatedDate)
            .ToList());
    }

    public async Task BuildReactIconForMessage(IList<MessageDetailViewModel> messages)
    {
        var messageIds = string.Join(",", messages.Select(x => x.MessageId));
        var messageReactIcons = await repository.GetMessageReactIcon(messageIds);

        foreach (var item in messages)
        {
            var findByMessageId = messageReactIcons.Where(x => x.MessageId == item.MessageId).ToList();
            item.MessageReactIcons = findByMessageId;
        }
    }

    public async Task<List<MessageDetailViewModel>> GetAllMessageThreeDayAgo(int ticketTypeId, int? subTypeId)
    {
        var message = (await repository.GetAllMessageThreeDayAgo(session.UserId, ticketTypeId, subTypeId)).ToList();
        await BuildReactIconForMessage(message);
        BuildBorderColorMessageByTicketId(message);
        return message.ToList();
    }

    private void BuildBorderColorMessageByTicketId(List<MessageDetailViewModel> messages)
    { 
        var rand = new Random();
        
        var ticketColors = messages
            .GroupBy(x => x.TicketId)
            .ToDictionary(
                group => group.Key,
                _ => String.Format("#{0:X6}", rand.Next(0x1000000))
            );
        
        foreach (var message in messages)
        {
            message.BorderColor = ticketColors[message.TicketId];
        }
    }
}
