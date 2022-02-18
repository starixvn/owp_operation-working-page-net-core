namespace OWP.OvertimeReport.Handlers
{
    using MediatR;
    using OWP.OvertimeReport.Models;
    using OWP.Services;

    public class GetLocationsRequest : IRequest<IEnumerable<Location>>
    {
    }

    public class GetLocationsHandler : IRequestHandler<GetLocationsRequest, IEnumerable<Location>>
    {
        private readonly RefResourceService refResourceService;

        public GetLocationsHandler(RefResourceService refResourceService)
        {
            this.refResourceService = refResourceService;
        }

        public async Task<IEnumerable<Location>> Handle(GetLocationsRequest request, CancellationToken cancellationToken)
        {
            var resource = await refResourceService.GetRefResourcesByName("LocationCode");

            return resource.Select(x => new Location { Label = x.Value, Value = x.Value });
        }
    }
}