using MediatR;
using OWP.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml;

using OWP.MatchCountLiveReport.Models;
using System.Collections.Generic;
using Epplus.Mapper.Extensions;
using OfficeOpenXml.Style;

namespace OWP.MatchCountLiveReport.Handlers
{
    public class ExportRequest : IRequest<ExcelPackage>
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

    }
    public class ExportHandler : IRequestHandler<ExportRequest, ExcelPackage>, IDisposable
    {
        private readonly ConfigurationService configurationService;

        private ExcelPackage excelPackage;

        private ExcelWorksheet reportSheet;

        public ExportHandler(ConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public async Task<ExcelPackage> Handle(ExportRequest request, CancellationToken cancellationToken)
        {
            var templatePath = GetReportTemplate();
            LoadTemplate(templatePath);
            BuildExcelBody(request);
            return excelPackage;
        }


        private void BuildExcelBody(ExportRequest request)
        {
            var dataSource = new List<ReportInfo>();
           
            reportSheet.ApplyVertical(dataSource, 3);

            var start = reportSheet.Dimension.Start;
            var end = reportSheet.Dimension.End;
            for (var index = start.Row; index < end.Row; index++)
            {
                var value= reportSheet.Cells[index, 0].Text;
                if (value is null)
                {
                    // TODO set style
                }
            }
        }


        private void LoadTemplate(string templatePath)
        {
            using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);

            excelPackage = new ExcelPackage(stream);
            reportSheet = excelPackage.Workbook.Worksheets[0];
        }

        private string GetReportTemplate()
        {
            var templatePath = @$"App_Data/Templates/MatchCountLiveReportTemplate.xlsx";
            return $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            excelPackage?.Dispose();
        }
    }


}