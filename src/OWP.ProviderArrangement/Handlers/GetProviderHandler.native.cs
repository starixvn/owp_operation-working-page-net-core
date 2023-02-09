namespace OWP.ProviderArrangement.Handlers;

using Criteria;
using Fanex.Data.Repository;
using MediatR;
using Models;

public class GetProviderRequest : IRequest<IList<ProviderModel>>
{

}

public class GetProviderHandler : IRequestHandler<GetProviderRequest, IList<ProviderModel>>
{

    private readonly IDynamicRepository dynamicRepository;

    public GetProviderHandler(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<IList<ProviderModel>> Handle(GetProviderRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetProviderCriteria();
        return (await dynamicRepository.FetchAsync<ProviderModel>(criteria)).ToList();
    }
}
