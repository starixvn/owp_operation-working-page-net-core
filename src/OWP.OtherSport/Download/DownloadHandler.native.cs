namespace OWP.OtherSport.Download
{
    using MediatR;
    using OfficeOpenXml;
    using OWP.OtherSport.Download.Services;
    
    
    

    public class DownloadAssignmentRequest : IRequest<ExcelPackage>
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Employee { get; set; }

        public string Sports { get; set; }

        public string OfferLives { get; set; }

        public string MatchStatuses { get; set; }

        public string LinkedMatches { get; set; }
    }

    public class Handler : IRequestHandler<DownloadAssignmentRequest, ExcelPackage>
    {
        private readonly ExportExcelService exportExcelService;

        public Handler(ExportExcelService exportExcelService)
        {
            this.exportExcelService = exportExcelService;
        }

        public async Task<ExcelPackage> Handle(DownloadAssignmentRequest request, CancellationToken cancellationToken)
        {
            return await exportExcelService.CreateExcelPackage(request);
        }
    }
}
