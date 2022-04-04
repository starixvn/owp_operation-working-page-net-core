using MediatR;
using OWP.Services;
using OfficeOpenXml;

using OWP.MatchCountLiveReport.Models;
using Epplus.Mapper.Extensions;

namespace OWP.MatchCountLiveReport.Handlers
{
  public class ExportRequest : IRequest<ExcelPackage>
  {
    public IList<ReportInfo> DataSource { get; set; }

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
      reportSheet.ApplyVertical(request.DataSource, 3);
      SetStyleSectionHeader();

    }

    private void SetStyleSectionHeader()
    {
      var dimension=reportSheet.Dimension;
      var start = dimension.Start;
      var end = dimension.End;
      for (var index = start.Row; index < end.Row; index++)
      {
        var value = reportSheet.Cells[index, 0].Text;
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