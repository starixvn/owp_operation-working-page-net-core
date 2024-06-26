using Fanex.Data.Repository;
using MediatR;
using OWP.ProviderArrangement.Criteria;
using OWP.ProviderArrangement.Models;

namespace OWP.ProviderArrangement.Handlers;

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
        var result = await dynamicRepository.FetchAsync<ProviderModel>(criteria);

        return result.ToList();
    }
}
