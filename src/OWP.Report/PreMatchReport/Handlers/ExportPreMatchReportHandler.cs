namespace OWP.Report.PreMatchReport.Handlers;

using MediatR;
using Models;
using OWP.Models;
using Services;

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
        var fileName = $"Prematch Assignment Record {request.Query.FromDate.ToString("ddMMMyyyy")}-{request.Query.ToDate.ToString("ddMMMyyyy")}.xlsx";

        return new FileExport(fileName, content);
    }
}