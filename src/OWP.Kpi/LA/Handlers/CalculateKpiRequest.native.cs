using MediatR;
using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Services;
using OWP.Kpi.LA.ViewModels;

namespace OWP.Kpi.LA.Handlers;

public class CalculateKpiRequest : IRequest<DataViewModel>
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string SectionIds { get; set; }
}

public class CalculateKpiHandler : IRequestHandler<CalculateKpiRequest, DataViewModel>
{
    private readonly KpiService kpiService;

    public CalculateKpiHandler(KpiService kpiService)
    {
        this.kpiService = kpiService;
    }

    public async Task<DataViewModel> Handle(CalculateKpiRequest request, CancellationToken cancellationToken)
    {
        var isGap = CalculateGapHelper.IsGap(request.FromDate, request.ToDate);
        var kpiInfo = await kpiService.BuildKpiInfo(request.FromDate, request.ToDate, request.SectionIds, isGap);

        return new DataViewModel
        {
            KpiLa = kpiInfo,
            IsGap = isGap
        };
    }

}
