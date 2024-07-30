using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;

namespace OWP.AccountSetting.QuickMessage.Handler;

using Criteria;
using Model;

public class GetQuickMessageRequest : IRequest<List<QuickMessage>>;

public class GetQuickMessageHandler : IRequestHandler<GetQuickMessageRequest, List<QuickMessage>>
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession session;

    public GetQuickMessageHandler(IDynamicRepository dynamicRepository, INapSession session)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
    }

    public async Task<List<QuickMessage>> Handle(GetQuickMessageRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetQuickMessageCriteria { UserId = session.UserId };
        var result = await dynamicRepository.FetchAsync<QuickMessage>(criteria);

        return result.ToList();
    }
}
