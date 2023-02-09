namespace OWP.Report.LAReport.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Models;
using Report.Models;
using Services;

public class LAReportRequest : QueryModel, IRequest<IEnumerable<LAReportModel>>
{
}

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