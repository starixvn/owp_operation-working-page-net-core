using MediatR;
using OWP.LeagueManagement.Models;
using OWP.LeagueManagement.Service;

namespace OWP.LeagueManagement.Handlers
{
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
}