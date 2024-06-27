using MediatR;
using OWP.Report.LAReport.Models;
using OWP.Report.LAReport.Services;
using OWP.Report.Models;

namespace OWP.Report.LAReport.Handlers;

public class LAReportRequest : QueryModel, IRequest<IEnumerable<LAReportModel>>;

public class LAReportHandler : IRequestHandler<LAReportRequest, IEnumerable<LAReportModel>>
{
    private readonly QueryRepository reportService;

    public LAReportHandler(QueryRepository reportService)
    {
        this.reportService = reportService;
    }

    public async Task<IEnumerable<LAReportModel>> Handle(LAReportRequest request, CancellationToken cancellationToken)
        => await reportService.Query(request);
}