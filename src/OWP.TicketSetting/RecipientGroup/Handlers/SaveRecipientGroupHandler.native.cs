using MediatR;
using OWP.TicketSetting.RecipientGroup.Criteria;
using OWP.TicketSetting.RecipientGroup.Models;
using OWP.TicketSetting.RecipientGroup.Services;

namespace OWP.TicketSetting.RecipientGroup.Handlers;

public class SaveRecipientGroupRequest : IRequest<string>
{
    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public IEnumerable<Recipient> Recipients { get; set; }
}

public class SaveRecipientGroupHandler : IRequestHandler<SaveRecipientGroupRequest, string>
{
    private readonly CommandRepository commandRepository;

    public SaveRecipientGroupHandler(CommandRepository commandRepository)
    {
        this.commandRepository = commandRepository;
    }

    public async Task<string> Handle(SaveRecipientGroupRequest request, CancellationToken cancellationToken)
    {
        var recipientOldIds = await GetRecipientIdOldInGroup(request.GroupId);
        var recipientNewIds = request.Recipients.Select(r => r.RecipientId);

        var recipientDiff = DetectNewRecipient(recipientNewIds.ToList(), recipientOldIds);

        await InsertRecipientToMessageReader(recipientDiff.ToList(), request.GroupId);

        await commandRepository.SaveRecipientGroup(request);

        await RemoveToMessageReader(request);
        return string.Empty;
    }

    private async Task<IEnumerable<int>> GetRecipientIdOldInGroup(int groupId)
    {
        var allGroup = await commandRepository.GetRecipientGroup();
        var group = allGroup.Where(g => g.GroupId == groupId);
        return group.Select(g => g.UserId).ToList();
    }

    private static IEnumerable<int> DetectNewRecipient(IEnumerable<int> recipientIdSource, IEnumerable<int> recipientIdDest)
        => recipientIdSource.Except(recipientIdDest).ToList();

    public async Task InsertRecipientToMessageReader(List<int> userIds, int groupId)
    {
        if (userIds.Any())
        {
            var criteria = new InsertMessageReaderCriteria
            {
                GroupId = groupId,
                UserIds = string.Join(",", userIds)
            };

            await commandRepository.InsertRecipientToMessageReader(criteria);
        }
    }

    public async Task RemoveToMessageReader(SaveRecipientGroupRequest request)
    {
        var userId = request.Recipients.Select(x => x.RecipientId);

        var criteria = new RemoveMessageReaderCriteria
        {
            GroupId = request.GroupId,
            UserIds = string.Join(",", userId)
        };

        await commandRepository.RemoveRecipientToMessageReader(criteria);
    }
}