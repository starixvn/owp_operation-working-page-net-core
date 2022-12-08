using MediatR;
using OWP.Models;
using OWP.Report.PreMatchReport.Models;
using OWP.Report.PreMatchReport.Services;

namespace OWP.Report.PreMatchReport.Handlers;

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
        var fileName = $"Prematch Assigment Record {request.Query.FromDate.ToString("ddMMMyyyy")}-{request.Query.ToDate.ToString("ddMMMyyyy")}.xlsx";

        return new FileExport(fileName, content);
    }
}