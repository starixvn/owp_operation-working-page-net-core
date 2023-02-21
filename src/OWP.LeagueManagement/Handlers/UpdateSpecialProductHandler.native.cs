using OWP.LeagueManagement.Services;

namespace OWP.LeagueManagement.Handlers;

using MediatR;
using Models;

public class UpdateSpecialProductRequest : IRequest<ProductInfo>
{
    public SpecialProduct Model { get; set; }
}
public class UpdateSpecialProductHandler : IRequestHandler<UpdateSpecialProductRequest, ProductInfo>
{
    private readonly SpecialProductService specialProductService;
    public UpdateSpecialProductHandler(SpecialProductService specialProductService)
    {
        this.specialProductService = specialProductService;
    }
    public async Task<ProductInfo> Handle(UpdateSpecialProductRequest request, CancellationToken cancellationToken)
        => await specialProductService.UpdateSpecialProduct(request.Model);

}