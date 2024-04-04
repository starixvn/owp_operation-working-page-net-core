
using Fanex.Data.Repository;
using MediatR;
using OWP.ProviderVerification.Models;

namespace OWP.ProviderVerification.Handlers;

public class GetProviderVerificationRequest : IRequest<IList<ProviderVerificationModel>>
{

}
public class GetProviderVerificationHandler: IRequestHandler<GetProviderVerificationRequest, IList<ProviderVerificationModel>>
{
    private readonly IDynamicRepository repository;

    public GetProviderVerificationHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }
    public async Task<IList<ProviderVerificationModel>> Handle(GetProviderVerificationRequest request, CancellationToken cancellationToken)
    {
        

        return new List<ProviderVerificationModel>();
    }
}
