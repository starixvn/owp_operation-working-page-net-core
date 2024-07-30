using MediatR;
using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.Services;
using OWP.Kpi.LA.ViewModels;

namespace OWP.Kpi.LA.Handlers;

public class CalculateKpiRequest : QueryModel, IRequest<DataViewModel>;

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
        var kpiInfo = await kpiService.BuildKpiInfo(request, isGap);

        return new DataViewModel
        {
            KpiLa = kpiInfo,
            IsGap = isGap
        };
    }

}
