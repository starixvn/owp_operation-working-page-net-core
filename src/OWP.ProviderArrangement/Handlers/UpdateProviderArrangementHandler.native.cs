using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using Newtonsoft.Json;
using OWP.ProviderArrangement.Criteria;
using OWP.ProviderArrangement.Models;

namespace OWP.ProviderArrangement.Handlers;

public class UpdateProviderArrangementRequest : IRequest<Unit>
{
    public List<ProviderModel> Providers { get; set; }
}

public class UpdateProviderArrangementHandler : IRequestHandler<UpdateProviderArrangementRequest, Unit>
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly INapSession session;

    public UpdateProviderArrangementHandler(IDynamicRepository dynamicRepository, INapSession session)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
    }

    public async Task<Unit> Handle(UpdateProviderArrangementRequest request, CancellationToken cancellation)
    {
        var criteria = new UpdateProviderArrangementCriteria
        {
            Providers = JsonConvert.SerializeObject(request.Providers),
            ActorId = session.UserId
        };

        await dynamicRepository.ExecuteAsync(criteria, cancellation);

        return Unit.Value;
    }
}