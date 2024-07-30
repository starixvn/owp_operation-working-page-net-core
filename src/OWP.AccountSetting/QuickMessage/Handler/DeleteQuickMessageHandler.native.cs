using Fanex.Data.Repository;
using MediatR;

namespace OWP.AccountSetting.QuickMessage.Handler;

using Criteria;
using Model;

public class DeleteQuickMessageRequest : IRequest
{
    public int Id { get; set; }
}

public class DeleteQuickMessageHandler : IRequestHandler<DeleteQuickMessageRequest>
{
    private readonly IDynamicRepository dynamicRepository;

    public DeleteQuickMessageHandler(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task Handle(DeleteQuickMessageRequest request, CancellationToken cancellationToken)
    {
        var criteria = new DeleteQuickMessageCriteria { Id = request.Id };
        await dynamicRepository.FetchAsync<QuickMessage>(criteria);
    }
}
