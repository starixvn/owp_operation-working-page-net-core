using MediatR;
using OWP.Extensions;
using OWP.Models;
using OWP.PreMatchReport.Models;
using OWP.PreMatchReport.Services;

namespace OWP.PreMatchReport.Handlers;

public class ExportPreMatchReportRequest : IRequest<FileExport>
{
    public QueryModel Query { get; set; }
}

public class ExportPreMatchReportHandler : IRequestHandler<ExportPreMatchReportRequest, FileExport>
{
    private readonly PreMatchReportExportService reportExportService;

    public ExportPreMatchReportHandler(PreMatchReportExportService reportExportService)
    {
        this.reportExportService = reportExportService;
    }

    public async Task<FileExport> Handle(ExportPreMatchReportRequest request, CancellationToken cancellationToken)
    {
        var content = await reportExportService.Export(request.Query);
        var fileName = $"Pre-match Assigment Record {request.Query.FromDate.ToFormatDDMMyyyy()}-{request.Query.ToDate.ToFormatDDMMyyyy()}.xlsx";

        return new FileExport(fileName, content);
    }
}