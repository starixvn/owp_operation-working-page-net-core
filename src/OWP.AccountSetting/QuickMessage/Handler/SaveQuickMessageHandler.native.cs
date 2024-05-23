using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;

namespace OWP.AccountSetting.QuickMessage.Handler;

using Criteria;
using Model;

public class SaveQuickMessageRequest : IRequest
{
    public int Id { get; set; }

    public string Shortcut { get; set; }

    public string Message { get; set; }

    public int UserId { get; set; }
}

public class SaveQuickMessageHandler : IRequestHandler<SaveQuickMessageRequest>
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession session;

    public SaveQuickMessageHandler(IDynamicRepository dynamicRepository, INapSession session)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
    }

    public async Task Handle(SaveQuickMessageRequest request, CancellationToken cancellationToken)
    {
        var criteria = new SaveQuickMessageCriteria
        {
            UserId = session.UserId,
            Id = request.Id,
            Shortcut = request.Shortcut,
            Message = request.Message,
        };
        await dynamicRepository.FetchAsync<QuickMessage>(criteria, cancellationToken);
    }
}
