using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OfficeOpenXml;

namespace OWP.ProviderVerification.ProviderVerification.UploadAgent;

public record UploadAgentRequest(Stream Stream) : Query<AnalyzedResult>;

public class UploadAgentHandler: QueryHandler<UploadAgentRequest, AnalyzedResult>
{
    private const int ROW_HEADER = 1;
    private const int START_ROW_CONTENT = 2;
    public async Task<AnalyzedResult> Handle(UploadAgentRequest request, CancellationToken cancellationToken)
    {
        using var package = new ExcelPackage(request.Stream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        // TODO
        //if (worksheet == null)
        //{
        //    return Results.StatusCode(StatusCodes.Status415UnsupportedMediaType);
        //}

               
        var providerModel = new List<ExcelProviderModel>();
        var rowCount = worksheet.Dimension.Rows;
        var colCount = worksheet.Dimension.Columns;

        var headers = new Dictionary<string, int>();
        for (var col = ROW_HEADER; col <= colCount; col++)
        {
            headers[worksheet.Cells[1, col].Text] = col;
        }

        for (var row = START_ROW_CONTENT; row <= rowCount; row++) // Assuming the first row is the header
        {
            var model = new ExcelProviderModel();
            foreach (var header in headers)
            {
                var property = typeof(ExcelProviderModel).GetProperty(header.Key, BindingFlags.Public | BindingFlags.Instance);
                if (property is null || !property.CanWrite)
                {
                    continue;
                }
                var cellValue = worksheet.Cells[row, header.Value].Text;
                var convertedValue = Convert.ChangeType(cellValue, property.PropertyType);
                property.SetValue(model, convertedValue);
            }
            providerModel.Add(model);
        }

        return new AnalyzedResult
        {
            ProviderModel= providerModel,
            OriginalRecordCount = providerModel.Count
        };
    }
}