using Olympus.OWP.Log.Criteria;

namespace OWP.ProviderVerification.ProviderVerification.UploadAgent;

public record UploadAgentValidationRequest(Stream Stream, string SessionId) : Query<ResultT<AnalyzedResult>>;

public class UploadAgentValidationHandler(ICache cache, IDynamicRepository dynamicRepository) : QueryHandler<UploadAgentValidationRequest, ResultT<AnalyzedResult>>
{
    private const int ROW_HEADER = 1;
    private const int START_ROW_CONTENT = 2;
    private const int CACHE_EXPIRED_IN_DAYS = 1;

    public async Task<ResultT<AnalyzedResult>> Handle(UploadAgentValidationRequest request, CancellationToken cancellationToken)
    {
        using var package = new ExcelPackage(request.Stream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
        var analyzer = new Analyzer();
        var rowCount = worksheet.Dimension.Rows;
        var colCount = worksheet.Dimension.Columns;

        if (colCount != 2 ||
            !string.Equals(worksheet.Cells[1, 1].Text?.Trim(), "Provider", StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(worksheet.Cells[1, 2].Text?.Trim(), "ProviderMatchID", StringComparison.OrdinalIgnoreCase))
        {
            return ResultT<AnalyzedResult>.Failure(new List<Error>
            {
                Error.Failure(ErrorCode.PROVIDER_TEMPLATE_REQUIRED, "The uploaded file doesn’t match the template", row: null)
            });
        }

        var headers = new Dictionary<string, int>();
        for (var col = ROW_HEADER; col <= colCount; col++)
        {
            headers[worksheet.Cells[1, col].Text] = col;
        }

        for (var row = START_ROW_CONTENT; row <= rowCount; row++)
        {
            var model = new ExcelProviderModel()
            {
                Row = row
            };
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

            analyzer.Add(model);
        }

        var validationResult = analyzer.Validate();
        if (validationResult.IsSuccess)
        {
            await SaveAgentUploadInfo(request.SessionId, analyzer);
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "UploadAgentFile", JsonConvert.SerializeObject(analyzer));
        }
        else
        {
            return ResultT<AnalyzedResult>.Failure(validationResult.Errors);
        }
        return ResultT<AnalyzedResult>.Success(new AnalyzedResult(request.SessionId, analyzer.Count));
    }

    private async Task SaveAgentUploadInfo(string sessionId, IList<ExcelProviderModel> providerModel)
    {
        var expiredDate = DateTime.Today.AddDays(CACHE_EXPIRED_IN_DAYS);
        var absoluteTimeSpan = expiredDate - DateTime.Now;
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(absoluteTimeSpan);
        var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
        var assignmentsJson = JsonConvert.SerializeObject(providerModel);
        await cache.SetAsync(cacheKey, assignmentsJson, cachePolicy);
    }

    private async Task LogDetailedPayload(DateTime CreatedDate, string Action, string Payload)
    {
        await dynamicRepository.ExecuteAsync(new OwpLogCriteria
        {
            CreatedDate = CreatedDate,
            Action = Action,
            Payload = Payload
        });
    }
}