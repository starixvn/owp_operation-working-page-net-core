namespace OWP.AssignmentUpload.Validation;

using System.Globalization;
using Exceptions;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using Models;
using OWP.Services;
using Services;

public class ValidationRequest : IRequest<AssignmentValidationResult>
{
    public ValidationRequest()
    {
        SessionId = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
    }

    public string SessionId { get; }

    public DateTime EventDate { get; set; }

    public Stream Stream { get; set; }

    public bool IsUploadFMAndAIO { get; set; }
}

public class Handler : IRequestHandler<ValidationRequest, AssignmentValidationResult>
{
    private readonly Analyzer analyzer;

    private readonly CacheManipulator cacheManipulator;

    private readonly ExcelValidator excelValidator;

    private readonly ConfigurationService configurationService;

    private readonly INapSession currentSession;

    public Handler(
        Analyzer analyzer,
        ExcelValidator excelValidator,
        ConfigurationService configurationService,
        CacheManipulator cacheManipulator,
        INapSession currentSession)
    {
        this.analyzer = analyzer;
        this.cacheManipulator = cacheManipulator;
        this.configurationService = configurationService;
        this.excelValidator = excelValidator;
        this.currentSession = currentSession;
    }

    public async Task<AssignmentValidationResult> Handle(ValidationRequest request, CancellationToken cancellationToken)
    {
        var result = new AssignmentValidationResult
        {
            SessionId = request.SessionId
        };
        try
        {
            result.RecordCount = await ProcessAssignmentUploading(result.SessionId, request);
        }
        catch (CriticalValidationException e)
        {
            result.Exception = e.Message;
        }
        catch (InvalidAssignmentRowException e)
        {
            result.ValidationResults = e.ValidationResults;
        }

        await LogExcelFile(request.SessionId, request.Stream);

        return result;
    }

    private async Task<int> ProcessAssignmentUploading(string sessionId, ValidationRequest request)
    {
        excelValidator.Validate(request.Stream);
        var analyzedResult = await analyzer.Analyze(request);
        await cacheManipulator.SaveAssignments(sessionId, analyzedResult.Assignments);

        return analyzedResult.OriginalRecordCount;
    }

    private async Task LogExcelFile(string sessionId, Stream file)
    {
        try
        {
            var folderPath = @$"App_Data{Path.DirectorySeparatorChar}Files";
            var absoluteFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}{folderPath}";

            if (!Directory.Exists(absoluteFolderPath))
            {
                Directory.CreateDirectory(absoluteFolderPath);
            }

            var fileName = $"{sessionId}-{currentSession.UserName}-New.xlsx";
            var fullPath = Path.Combine(absoluteFolderPath, fileName);
            using (var stream = File.Create(fullPath))
            {
                file.Seek(0, SeekOrigin.Begin);
                await file.CopyToAsync(stream);
            }
        }
        catch
        {
            // SonarQube
        }
    }
}