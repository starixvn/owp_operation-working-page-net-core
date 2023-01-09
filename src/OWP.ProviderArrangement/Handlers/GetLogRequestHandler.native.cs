namespace OWP.ProviderArrangement.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Criteria;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using Models;
using Newtonsoft.Json;

public class GetLogRequest : IRequest<IList<ProviderModel>>
{

}

public class GetLogRequestHandler : IRequestHandler<GetLogRequest, IList<ProviderModel>>
{

    private readonly IDynamicRepository repository;

    public GetLogRequestHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<ProviderModel>> Handle(GetLogRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetLogProviderCriteria();
        return (await repository.FetchAsync<ProviderModel>(criteria)).ToList();
    }
}